using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using com.halo.framework.common;
using com.snake.framework.runtime;
using HaloFramework.Common;
using HaloFramework.Runtime;
using UnityEngine;
using UnityEngine.Networking;
using Debuger = HaloFramework.Runtime.Debuger;
using Utility = com.snake.framework.runtime.Utility;

namespace com.halo.framework
{
    namespace runtime
    {
        /// <summary>
        /// 热更管理器
        /// </summary>
        public class PatchManager : com.snake.framework.runtime.BaseManager
        {
            /// <summary>
            /// 本地差异文件diff.json
            /// 热更过程中该字段的值为旧值，热更完成后会被远端的RemoteDiff覆盖
            /// </summary>
            public SourceInfoDictionary LocalDiff { get; private set; }

            /// <summary>
            /// 远端差异文件diff.json
            /// </summary>
            private SourceInfoDictionary RemoteDiff = null;

            /// <summary>
            /// 版本库信息列表
            /// </summary>
            public VersionRecord AllRepositoryInfoList { get; private set; }

            /// <summary>
            /// 本地版本信息
            /// </summary>
            public LocalInfo LocalInfo { get; private set; }

            /// <summary>
            /// 包体所对应的版本库信息
            /// </summary>
            public RemoteInfo DownloadedRemoteInfo { get; private set; }

            /// <summary>
            /// 包体信息
            /// </summary>
            public ApplicationInfo applicationInfo { get; private set; }

            /// <summary>
            /// 需要下载的列表
            /// </summary>
            public Dictionary<string, SourceInfo> needDownloadDict;

            /// <summary>
            /// 预加载进度
            /// </summary>
            private float _preloadProgress = 0;

            /// <summary>
            /// 渠道名字
            /// </summary>
            public string ChannelName { get; private set; }

            /// <summary>
            /// 底包版本
            /// </summary>
            public int BasicVersion { get; set; }

            /// <summary>
            /// 当前版本
            /// </summary>
            public int LocalResVersion { get; set; }

            /// <summary>
            /// 当初始化
            /// </summary>
            protected override void onInitialization()
            {
                var applicationContent = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(PathConst.APP_INFO_FILENAME)).text;
                applicationInfo = Utility.Json.FromJson<ApplicationInfo>(applicationContent);
                HaloFramework.Runtime.Debuger.SetLevel(applicationInfo.LogLevel);
                CheckNeedClearPersistentData(applicationInfo.BuildVersionCode);
                BasicVersion = applicationInfo.BasicResVersion;
                ChannelName = applicationInfo.Channel;
#if UNITY_EDITOR
                if (AppConst.ASSET_BUNDLE_MODE)
                {
                    //ChannelName = EditorPrefs.GetString(PathConst.Editor_Runtime_Channel, string.Empty);
                    //BasicVersion = EditorPrefs.GetInt(PathConst.Editor_Runtime_Basic_Version_Num, -1);
                    LocalResVersion = BasicVersion;
                    PathConst.SetStreamingAssets(BasicVersion, ChannelName);
                }
#endif
                LocalInfo = new LocalInfo { ResVersion = BasicVersion };
                LocalResVersion = LocalInfo.ResVersion;
                if (AppConst.ASSET_BUNDLE_MODE && applicationInfo.Patcher)
                {
                    var path = Path.Combine(PathConst.PERSISTENT_DATA_PATH, PathConst.LOCAL_FILENAME);
                    var exitsInPersistent = File.Exists(path);
                    if (exitsInPersistent)
                    {
                        var localJson = EncryptHelper.ReadAllTextByAES(path);
                        LocalInfo = Utility.Json.FromJson<LocalInfo>(localJson);
                        LocalResVersion = LocalInfo.ResVersion;
                    }
                }
                Debuger.InfoFormat("初始化时localresversion为：{0}", LocalResVersion);
                var localDiffPath = Path.Combine(PathConst.PERSISTENT_DATA_PATH, PathConst.REMOTE_DIFF_FILENAME);
                var exits = File.Exists(localDiffPath);
                if (exits)
                {
                    //var content = File.ReadAllText(localDiffPath);
                    var content = EncryptHelper.ReadAllTextByAES(localDiffPath);
                    LocalDiff = Utility.Json.FromJson<SourceInfoDictionary>(content);
                }
                else
                {
                    LocalDiff = new SourceInfoDictionary();
                }
                _preloadProgress = 1;
            }

            /// <summary>
            /// 生命周期 - 开始预加载时
            /// </summary>
            protected override void onPreload()
            {

            }

            /// <summary>
            /// 文件是否存在于热更目录
            /// </summary>
            /// <param name="bundleName"></param>
            /// <returns></returns>
            public bool IsExtInPatcher(string path, bool checkMD5 = false)
            {
                if (LocalDiff == null)
                {
                    //说明没有热更过
                    return false;
                }
                var result = LocalDiff.Data.TryGetValue(path, out var sourceInfo);
                if (!result)
                {
                    return false;
                }
                if (checkMD5)
                {
                    var patcherPath = $"{PathConst.PERSISTENT_DATA_PATH}/{path}";
                    if (sourceInfo.md5 != Utility.FileMD5(patcherPath))
                    {
                        Debuger.ErrorFormat("{0}校验MD5失败", patcherPath);
                        return false;
                    }
                }
                return sourceInfo.resVersion > applicationInfo.BasicResVersion;
            }


            /// <summary>
            /// 获取预加载进度(0.0-1.0)
            /// </summary>
            /// <returns></returns>
            public override float GetPreloadProgress()
            {
                return _preloadProgress;
            }


            /// <summary>
            /// 检查热更
            /// </summary>
            public void CheckPatcher(Action<FrameworkStateCode, SourceInfoDictionary> callback)
            {
                if (!applicationInfo.Patcher)
                {
                    callback?.Invoke(FrameworkStateCode.NoUpdate, null);
                    return;
                }
                SourceInfoDictionary needDownloadDiff = null;
                var compareResultCode = FrameworkStateCode.Error;
                var taskList = new List<Action<Action<FrameworkStateCode>>>();
                taskList.Add(cb =>
                {
                    BuildInUIHelper.ShowLoadingPanel("检查版本中", 0);
                    CheckVersion((c) =>
                    {
                        compareResultCode = (FrameworkStateCode)c;
                        var code = FrameworkStateCode.Error;
                        if (compareResultCode == FrameworkStateCode.NeedUpdate)
                        {
                            //需要更新的情况、要再计算出差异列表
                            code = FrameworkStateCode.Succeed;
                            cb?.Invoke(code);
                            return;
                        }
                        cb?.Invoke(code);
                    });
                });
                taskList.Add((cb) =>
                {
                    BuildInUIHelper.ShowLoadingPanel("下载远端文件配置中", 0);
                    DownloadRemoteDiff(DownloadedRemoteInfo.ResVersion, (code, diff) =>
                    {
                        RemoteDiff = diff;
                        cb?.Invoke(code);
                    });
                });
                taskList.Add((cb) =>
                {
                    BuildInUIHelper.ShowLoadingPanel("对比差异中", 0);
                    CalNeedDownloadFileList(LocalResVersion, RemoteDiff, (code, result) =>
                     {
                         needDownloadDiff = result;
                         cb?.Invoke(code);
                     });
                });
                void Final(FrameworkStateCode code)
                {
                    BuildInUIHelper.ShowLoadingPanel("热更检查完成", 0);
                    void CancelCallback() => callback?.Invoke(FrameworkStateCode.ExitGame, null);
                    switch (compareResultCode)
                    {
                        case FrameworkStateCode.NoUpdate:
                            callback?.Invoke(compareResultCode, null);
                            break;
                        case FrameworkStateCode.NeedUpdate:
                            {
                                if (needDownloadDiff.Data.Count == 0)
                                {
                                    callback?.Invoke(FrameworkStateCode.NoUpdate, null);
                                    break;
                                }
                                var sizeStr = string.Empty;
                                var size = CalNeedDownloadSize(needDownloadDiff);
                                sizeStr = Utility.Text.DownloadSize2String(size);
                                BuildInUIHelper.ShowDialogBoxPanel(
                                    "提示",
                                    $"需要更新大小：{sizeStr}\n是否下载？",
                                    () => callback?.Invoke(compareResultCode, needDownloadDiff),
                                    CancelCallback);
                                break;
                            }
                        case FrameworkStateCode.BaseVersionIsLow:
                            BuildInUIHelper.ShowDialogBoxPanel(
                                "提示",
                                $"底包版本过低,请前往应用商店下载",
                                CancelCallback,
                                CancelCallback);
                            break;
                        default:
                            BuildInUIHelper.ShowDialogBoxPanel(
                                "提示",
                                $"检查更新异常,错误码:{code}\n是否重试？",
                                () => callback?.Invoke(FrameworkStateCode.CheckPatcherAgain, needDownloadDiff), CancelCallback);
                            break;
                    }
                }
                taskList.Run(Final);
            }

            /// <summary>
            /// 检查版本
            /// 因为LUA层需要调用到，所以分多个检查版本
            /// </summary>
            /// <param name="callback">所以返回int</param>
            public void CheckVersion(Action<int> callback)
            {
                if (!applicationInfo.Patcher || !AppConst.ASSET_BUNDLE_MODE)
                {
                    callback?.Invoke((int)FrameworkStateCode.NoUpdate);
                    return;
                }
                var taskList = new List<Action<Action<FrameworkStateCode>>>();
                taskList.Add((cb) =>
                {
                    DownloadRepositoryInfoList((code, repositoryInfoList) =>
                    {
                        AllRepositoryInfoList = repositoryInfoList;
                        cb?.Invoke(code);
                    });
                });
                taskList.Add((cb) =>
                {
                    MatchSelfRepositoryInfo(ChannelName, AllRepositoryInfoList, (code, selfRepositoryInfo) =>
                    {
                        DownloadedRemoteInfo = selfRepositoryInfo;
                        cb?.Invoke(code);
                    });
                });
                taskList.Add((cb) =>
                {
                    CompareVersion(DownloadedRemoteInfo, (code) =>
                    {
                        cb?.Invoke(code);
                    });
                });
                taskList.Run((code) =>
                {
                    callback?.Invoke((int)code);
                });
            }

            /// <summary>
            /// 下载差异资源
            /// </summary>
            public void DownloadDiffSource(SourceInfoDictionary needDownload, Action<FrameworkStateCode> finishCallback)
            {
                var taskList = new List<Action<Action<FrameworkStateCode>>>();
                taskList.Add((cb) =>
                {

                    /*
                    Debuger.Info("下载差异资源中");

                    var needDownloadFileArray = new Queue<NeedDownloadFile>();

                    var prefix = $"{PathConst.RUNTIME_REMOTE_PATH}/{PathConst.REMOTE_ROOT_FOLD}/{ChannelName}/{Utility.Platform.GetPlatformName()}/{PathConst.PACKER_PREFIX}";
                    foreach (var item in needDownload.Data)
                    {
                        var ndf = new NeedDownloadFile
                        {
                            downloadPath = $"{prefix}{item.Value.resVersion}/{item.Key}".FixSlash(),
                            savePath = Path.Combine(PathConst.PERSISTENT_DATA_PATH, item.Key),
                            size = item.Value.size
                        };
                        needDownloadFileArray.Enqueue(ndf);
                    }
                    mFramework.GetManager<DownloadManager>().Download(needDownloadFileArray, DownloadingCallback, cb);
                    */
                    DownloadManager downloadMgr = Singleton<AppFacade>.GetInstance().GetManager<DownloadManager>();
                    downloadMgr.SetDownloadSpeedMonitor(true);
                    void OnDownloading(int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
                    {
                        Debug.LogError("mDownloadSpeed  "+ downloadMgr.mDownloadSpeed);
                        Debug.LogError("mTotalDownloadSize  " + downloadMgr.mTotalDownloadSize);
                        DownloadingCallback(downloadMgr.GetReadyTaskCount(), downloadMgr.GetCurrDownLoadSize(),
                            downloadMgr.mTotalDownloadSize, downloadMgr.mDownloadSpeed);
                        if (!downloadMgr.mDownloading)
                        {
                            this.mFramework.mLifeCycle.mUpdateHandle.RemoveEventHandler(OnDownloading);
                            cb(FrameworkStateCode.Succeed);
                        }
                    }

                    Debuger.Info("下载差异资源中");

                    var prefix = $"{PathConst.RUNTIME_REMOTE_PATH}/{PathConst.REMOTE_ROOT_FOLD}/{ChannelName}/{Utility.Platform.GetPlatformName()}/{PathConst.PACKER_PREFIX}";
                    foreach (var item in needDownload.Data)
                    {
                        string url = $"{prefix}{item.Value.resVersion}/{item.Key}".FixSlash();
                        string savePath = Path.Combine(PathConst.PERSISTENT_DATA_PATH, item.Key);
                        downloadMgr.StartDownload(url, savePath);
                    }

                    this.mFramework.mLifeCycle.mUpdateHandle.AddEventHandler(OnDownloading);
                });
                taskList.Add((cb) =>
                {
                    Debuger.Info("覆盖本地LocalInfo中");
                    LocalInfo = new LocalInfo
                    {
                        ResVersion = DownloadedRemoteInfo.ResVersion
                    };
                    LocalResVersion = LocalInfo.ResVersion;
                    var content = Utility.Json.ToJson(LocalInfo);
                    var savePath = Path.Combine(PathConst.PERSISTENT_DATA_PATH, PathConst.LOCAL_FILENAME);
                    var fileInfo = new FileInfo(savePath);
                    if (!fileInfo.Directory.Exists)
                    {
                        fileInfo.Directory.Create();
                    }
                    if (fileInfo.Exists)
                    {
                        fileInfo.Attributes = FileAttributes.Normal;
                        fileInfo.Delete();
                    }

                    EncryptHelper.WriteAllTextByAES(savePath, content);
                    cb?.Invoke(FrameworkStateCode.Succeed);
                });
                taskList.Add((cb) =>
                {
                    Debuger.Info("覆盖本地LocalDiff中");
                    LocalDiff = RemoteDiff;
                    var localDiffPath = Path.Combine(PathConst.PERSISTENT_DATA_PATH, PathConst.REMOTE_DIFF_FILENAME);
                    var fileInfo = new FileInfo(localDiffPath);
                    if (!fileInfo.Directory.Exists)
                    {
                        fileInfo.Directory.Create();
                    }
                    var content = Utility.Json.ToJson(LocalDiff);
                    //File.WriteAllText(fileInfo.FullName, content);
                    EncryptHelper.WriteAllTextByAES(fileInfo.FullName, content);
                    cb?.Invoke(FrameworkStateCode.Succeed);
                });
                taskList.Add((cb) =>
                {
                    Debuger.Info("合并差异代码");
                    string path = Path.Combine(PathConst.PERSISTENT_DATA_PATH, PathConst.LUA_ENCRYPT_FILE_FOLD);
                    Debug.Log("合并路径：" + path);
                    if (Directory.Exists(path))
                    {
                        AppFacade.GetInstance().GetManager<LuaManager>().AppendHotfixFiles(path, (FrameworkStateCode code) =>
                        {
                            if (Directory.Exists(path))
                            {
                                DirectoryInfo dir = Directory.CreateDirectory(path);
                                DirectoryInfo[] dirs = dir.GetDirectories();
                                for (int i = 0; i < dirs.Length; i++)
                                {
                                    dirs[i].Delete(true);
                                }
                                FileInfo[] files = dir.GetFiles();
                                for (int i = 0; i < files.Length; i++)
                                {
                                    files[i].Delete();
                                }
                            }
                            cb?.Invoke(code);
                        });
                    }
                    else
                    {
                        cb?.Invoke(FrameworkStateCode.Succeed);
                    }


                });
                taskList.Run((code) =>
                {
                    if (code != FrameworkStateCode.Succeed)
                    {
                        UnityEngine.Debug.LogErrorFormat("下载流程报错，错误码:{0}", code);
                        BuildInUIHelper.ShowDialogBoxPanel(
                            "错误",
                            "下载资源出错，是否进行重试？",
                            () => { finishCallback?.Invoke(FrameworkStateCode.DownloadError); },
                            () => { finishCallback?.Invoke(FrameworkStateCode.ExitGame); });
                        return;
                    }
                    finishCallback?.Invoke(code);
                });
            }

            /// <summary>
            /// 清理持久化目录
            /// </summary>
            public void ClearPersistentData()
            {
                string path = PathConst.PERSISTENT_DATA_PATH;
                if (Directory.Exists(path))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(path);
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        dirs[i].Delete(true);
                    }
                    FileInfo[] files = dir.GetFiles();
                    for (int i = 0; i < files.Length; i++)
                    {
                        files[i].Delete();
                    }
                }

            }
            public void CheckNeedClearPersistentData(int versionCode)
            {
                LocalDataManager localDataMgr = Singleton<AppFacade>.GetInstance().GetManager<LocalDataManager>();
                int buildVersionCode = localDataMgr.GetInt("BUILD_VERSION_CODE", -1);
                if (buildVersionCode != versionCode)
                {
                    ClearPersistentData();
                    localDataMgr.SetInt("BUILD_VERSION_CODE", applicationInfo.BuildVersionCode);
                }
            }

            /// <summary>
            /// 下载中的处理
            /// </summary>
            /// <param name="remainingFileCount"></param>
            /// <param name="hadDownloadedSize"></param>
            /// <param name="allSize"></param>
            /// <param name="speed"></param>
            void DownloadingCallback(int remainingFileCount, long hadDownloadedSize, long allSize, long speed)
            {
                var hadDownloadedSizeStr = Utility.Text.DownloadSize2String(hadDownloadedSize);
                var allSizeStr = Utility.Text.DownloadSize2String(allSize);
                var speedStr = Utility.Text.DownloadSize2String(speed);
                var content = $"正在下载中 剩余文件总数量{remainingFileCount} ({hadDownloadedSizeStr}/{allSizeStr}) 下载速度:{speedStr}/s";
                float progress = 0;
                if(allSize!=0)
                 progress = (float)hadDownloadedSize / (float)allSize;
                BuildInUIHelper.ShowLoadingPanel(content, progress);
            }

            /// <summary>
            /// 下载远端版本库信息
            /// </summary>
            async private void DownloadRepositoryInfoList(Action<FrameworkStateCode, VersionRecord> callback)
            {
                var url = Path.Combine(PathConst.RUNTIME_REMOTE_PATH, PathConst.REMOTE_ROOT_FOLD, PathConst.VERSION_RECORD_FILENAME).FixSlash();
                Debuger.Info("下载版本库信息列表的地址:" + url);
                void Callback(UnityWebRequest.Result result, object encryptData)
                {
                    var code = result == UnityWebRequest.Result.Success ? FrameworkStateCode.Succeed : FrameworkStateCode.DownloadError;
                    if (code != FrameworkStateCode.Succeed)
                    {
                        callback?.Invoke(code, null);
                        return;
                    }
                    var AESKeyArray = Utility.Encryption.XORDecrypt(Encoding.Default.GetBytes(AppConfig.AESKey_Xor), AppConfig.XORKey);
                    var AESKey = Encoding.Default.GetString(AESKeyArray);
                    string content = Encoding.UTF8.GetString(Utility.Encryption.AESDecrypt((byte[])encryptData, AESKey, AppConfig.AESIV));
                    VersionRecord data = (VersionRecord)Utility.Json.FromJson(content, typeof(VersionRecord));
                    callback?.Invoke(code, data);
                }
                await Utility.Web.Get(url, typeof(byte[]), 5, Callback);
            }

            /// <summary>
            /// 匹配自身的版本
            /// </summary>
            /// <param name="callback"></param>
            private void MatchSelfRepositoryInfo(string channel, VersionRecord versionRecord, Action<FrameworkStateCode, RemoteInfo> callback)
            {
                var code = FrameworkStateCode.Succeed;
                if (versionRecord == null)
                {
                    code = FrameworkStateCode.RepositoryInfoListNull;
                    callback?.Invoke(code, null);
                    return;
                }
                if (!versionRecord.ChannelDict.ContainsKey(channel))
                {
                    Debuger.Error("无法正确匹配版本库中的渠道,请检查");
                    code = FrameworkStateCode.Error;
                    callback?.Invoke(code, null);
                    return;
                }
                var remoteInfo = versionRecord.ChannelDict[channel][Utility.Platform.GetPlatformName()][RunMode.Release];
                var mDeviceId = Singleton<AppFacade>.GetInstance().GetManager<SDKManager>().GetDeviceId();
                Debug.Log("本机设备ID为:" + mDeviceId);//必须输出
                var isWhite = versionRecord.WhiteDeviceInfoList.ContainsKey(mDeviceId);
                Debug.Log("是否为白名单:" + isWhite);//必须输出
                if (isWhite)
                    remoteInfo = versionRecord.ChannelDict[channel][Utility.Platform.GetPlatformName()][RunMode.White];
                callback?.Invoke(code, remoteInfo);
            }

            /// <summary>
            /// 比对版本
            /// </summary>
            /// <param name="remoteInfo"></param>
            /// <param name="callback"></param>
            private void CompareVersion(RemoteInfo remoteInfo, Action<FrameworkStateCode> callback)
            {
                var code = FrameworkStateCode.Succeed;
                Debuger.Info("当前APP底包版本:" + BasicVersion);
                if (BasicVersion < remoteInfo.BasicVersion)
                {
                    code = FrameworkStateCode.BaseVersionIsLow;
                    callback?.Invoke(code);
                    return;
                }
                if (BasicVersion >= remoteInfo.BasicVersion
                    && BasicVersion >= remoteInfo.ResVersion)
                {
                    Debug.Log("底包版本高，直接进入游戏 无需热更");
                    code = FrameworkStateCode.NoUpdate;
                    callback?.Invoke(code);
                    return;
                }
                Debug.LogFormat("版本库上最新版本:{0}", remoteInfo.ResVersion);
                Debug.LogFormat("本地记录的当前版本:{0}", LocalResVersion);
                if (remoteInfo.ResVersion <= LocalResVersion)
                {
                    Debug.Log("已是最新版本");
                    code = FrameworkStateCode.NoUpdate;
                    callback?.Invoke(code);
                    return;
                }
                code = FrameworkStateCode.NeedUpdate;
                callback?.Invoke(code);
            }

            /// <summary>
            /// 下载远端资源差异列表
            /// </summary>
            /// <param name="callback"></param>
            async private void DownloadRemoteDiff(int version, Action<FrameworkStateCode, SourceInfoDictionary> callback)
            {
                var remoteDiff = Path.Combine(
                    PathConst.RUNTIME_REMOTE_PATH,
                    PathConst.REMOTE_ROOT_FOLD,
                    ChannelName,
                    Utility.Platform.GetPlatformName(),
                    (PathConst.PACKER_PREFIX + version),
                    PathConst.REMOTE_DIFF_FILENAME);
                remoteDiff = remoteDiff.FixSlash();
                Debuger.Info("下载远端差异资源列表diff " + remoteDiff);
                void Callback(UnityWebRequest.Result result, object encryptData)
                {
                    var code = result == UnityWebRequest.Result.Success ? FrameworkStateCode.Succeed : FrameworkStateCode.DownloadError;
                    if (code != FrameworkStateCode.Succeed)
                    {
                        callback?.Invoke(code, null);
                        return;
                    }
                    var AESKeyArray = Utility.Encryption.XORDecrypt(Encoding.Default.GetBytes(AppConfig.AESKey_Xor), AppConfig.XORKey);
                    var AESKey = Encoding.Default.GetString(AESKeyArray);
                    string content = Encoding.UTF8.GetString(Utility.Encryption.AESDecrypt((byte[])encryptData, AESKey, AppConfig.AESIV));
                    var jsonObj = Utility.Json.FromJson(content, typeof(SourceInfoDictionary));
                    callback?.Invoke(code, (SourceInfoDictionary)jsonObj);
                }
                await Utility.Web.Get(remoteDiff, typeof(byte[]), 5, Callback);
            }

            /// <summary>
            /// 比较差异列表
            /// </summary>
            /// <param name="local"></param>
            /// <param name="remote"></param>
            /// <param name="callback"></param>
            private void CalNeedDownloadFileList(int curVersion, SourceInfoDictionary remote, Action<FrameworkStateCode, SourceInfoDictionary> callback)
            {
                var needDownload = new SourceInfoDictionary();
                foreach (var kv in remote.Data)
                {
                    if (kv.Value.resVersion <= curVersion)
                    {
                        continue;
                    }
                    needDownload.Data.Add(kv.Key, kv.Value);
                }
                callback?.Invoke(FrameworkStateCode.Succeed, needDownload);
            }

            /// <summary>
            /// 计算下载大小
            /// </summary>
            /// <param name="addSources"></param>
            /// <returns></returns>
            private long CalNeedDownloadSize(SourceInfoDictionary addSources)
            {
                var totalDownloadSize = 0;
                if (addSources == null || addSources.Data == null || addSources.Data.Count <= 0) return totalDownloadSize;
                totalDownloadSize = addSources.Data.Sum((t) => (int)(t.Value.size));
                return totalDownloadSize;
            }



            /// <summary>
            /// 发送热更打点到BDC
            /// </summary>
            private void SendPatcherLogToBDC(string update_id)
            {
                var log = new BDCPatcherLog
                {
                    event_id = "20002",
                    user_id = "0",
                    server_id = "0",
                    role_id = "0",
                    role_key = "0",
                    event_name = "游戏更新",
                    device_key = "",
                    update_id = update_id,
                    update_state = "1",
                    duration_time = ""
                };
                var json = JsonUtility.ToJson(log);
                Singleton<AppFacade>.GetInstance().GetManager<SDKManager>().SendToBDC(json);
            }

            [Serializable]
            public class BDCPatcherLog
            {
                public string event_id;
                public string user_id;
                public string server_id;
                public string role_id;
                public string role_key;
                public string event_name;
                public string device_key;
                public string update_id;
                public string update_state;
                public string duration_time;
            }
        }
    }
}