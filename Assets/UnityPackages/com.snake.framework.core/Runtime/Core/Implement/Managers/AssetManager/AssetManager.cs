using System;
using HaloFramework;
using System.Collections.Generic;
using com.halo.framework.common;
using HaloFramework.Runtime;
#if UNITY_EDITOR
using HaloFramework.Editor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.halo.framework
{
    namespace runtime
    {
        public class AssetManager : com.snake.framework.runtime.BaseManager
        {
            private BundleManager _bundleMgr;
            private PatchManager _patchMgr;
            private Dictionary<string, string> _pathMapping;

            private List<BaseAsyncOperation> _asyncLoadingOperationList;
            private List<BaseAsyncOperation> _asyncLoadedOperationList;

            private int MaxLoadNum = 5;

            private float unloadDetail = 60;
            private float curTime = 0;

            protected override void onInitialization()
            {
                this._bundleMgr = this.mFramework.GetManager<BundleManager>();
                this._patchMgr = this.mFramework.GetManager<PatchManager>();
                this._asyncLoadingOperationList = new List<BaseAsyncOperation>(1024);
                this._asyncLoadedOperationList = new List<BaseAsyncOperation>(1024);
                this._pathMapping = new Dictionary<string, string>(1024);
            }

            protected override void onPreload()
            {
                base.onPreload();
                mFramework.mLifeCycle.mUpdateHandle.AddEventHandler(this.onTick);
            }
            private bool TryCreateBundleAsyncOperation(string assetPath, out BundleOperationCollection collection)
            {
                collection = default;
#if UNITY_EDITOR
                if (AppConst.ASSET_BUNDLE_MODE == false)
                    return false;
#endif
                if (_pathMapping.TryGetValue(assetPath, out string bundleName) == false)
                {
                    Debuger.Error("没有找到资源路径对应的BundleName");
                    return false;
                }
                _bundleMgr.AsyncLoadBundle(bundleName, out collection);
                return true;
            }

            public void SetPathMapping(Dictionary<string, string> pathMapping)
            {
                _pathMapping = pathMapping;
            }

            /// <summary>
            /// 获取视频路径
            /// </summary>
            /// <param name="videoName"></param>
            /// <returns></returns>
            public string GetVideoPath(string videoName)
            {
                var path = string.Empty;
                if (AppConst.ASSET_BUNDLE_MODE)
                {
                    path = $"videos/{videoName}";
                    var exits = _patchMgr.applicationInfo.Patcher && _patchMgr.IsExtInPatcher(path, true);
                    path = exits ? $"{PathConst.PERSISTENT_DATA_PATH}/{path}" : $"{PathConst.STREAMING_ASSET_PATH}/{path}";
                }
                else
                {
                    path = $"{Application.dataPath}/ResExport/Videos/{videoName}";
                }
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    path = "file://" + path;
                }
                return path;
            }

            public AssetAsyncOperation AsyncLoadAsset<T>(string assetPath, System.Action<T> onCompleted) where T : UnityEngine.Object
            {
                return AsyncLoadAsset(assetPath, typeof(T), (operation) =>
                {
                    onCompleted?.Invoke(operation.mResult as T);
                });
            }

            public AssetAsyncOperation AsyncLoadAsset(string assetPath, System.Type assetType, System.Action<IAssetAsyncOperation> onCompleted)
            {
                AssetAsyncOperation assetAsyncOperation = null;
                BundleOperationCollection collection = default;
                if (AppConst.ASSET_BUNDLE_MODE)
                {
                    if (TryCreateBundleAsyncOperation(assetPath, out collection) == true)
                    {
                        assetAsyncOperation = AssetAsyncOperation.Create(assetPath, assetType, collection, onCompleted);
                    }
                    else
                    {
                        var content = $"无法找到路径所对应的bundle，路径名为:{assetPath}";
                        throw new Exception(content);
                    }
                }
                else
                {
#if UNITY_EDITOR
                    assetAsyncOperation = EditorAssetAsyncOperation.Create(assetPath, assetType, collection, onCompleted);
#endif
                }
                this._asyncLoadingOperationList.Add(assetAsyncOperation);
                return assetAsyncOperation;
            }
            public SubAssetAsyncOperation AsyncLoadSubAsset<T>(string assetPath, System.Action<IAssetAsyncOperation> onCompleted)
            {
                return AsyncLoadSubAsset(assetPath, typeof(T), onCompleted);
            }

            public SubAssetAsyncOperation AsyncLoadSubAsset(string assetPath, System.Type assetType, System.Action<IAssetAsyncOperation> onCompleted)
            {
                SubAssetAsyncOperation subAssetAsyncOperation = null;
                BundleOperationCollection collection = default;
                if (AppConst.ASSET_BUNDLE_MODE)
                {
                    if (TryCreateBundleAsyncOperation(assetPath, out collection) == true)
                    {
                        subAssetAsyncOperation = SubAssetAsyncOperation.Create(assetPath, assetType, collection, onCompleted);
                    }
                    else
                    {
                        var content = $"无法找到路径所对应的bundle，路径名为:{assetPath}";
                        throw new Exception(content);
                    }
                }
                else
                {
#if UNITY_EDITOR
                    subAssetAsyncOperation = EditorSubAssetAsyncOperation.Create(assetPath, assetType, collection, onCompleted);
#endif
                }

                this._asyncLoadingOperationList.Add(subAssetAsyncOperation);
                return subAssetAsyncOperation;
            }

            public SceneAsyncOperation AsyncLoadScene(string scenePath, System.Action<IAssetAsyncOperation> completed, bool additive = false)
            {
                SceneAsyncOperation sceneAsyncOperation = null;
                BundleOperationCollection collection = default;
                if (AppConst.ASSET_BUNDLE_MODE)
                {
                    if (TryCreateBundleAsyncOperation(scenePath, out collection) == true)
                    {
                        sceneAsyncOperation = SceneAsyncOperation.Create(scenePath, collection, completed, additive);
                    }
                    else
                    {
                        var content = $"无法找到路径所对应的bundle，场景名为:{scenePath}";
                        throw new Exception(content);
                    }
                }
                else
                {
#if UNITY_EDITOR
                    sceneAsyncOperation = HaloFramework.Editor.EditorSceneAsyncOperation.Create(scenePath, collection, completed, additive);
#endif
                }

                sceneAsyncOperation = SceneAsyncOperation.Create(scenePath, collection, completed, additive);
                this._asyncLoadingOperationList.Add(sceneAsyncOperation);
                return sceneAsyncOperation;
            }

            protected void onTick(int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
            {
                curTime += deltaTime;
                if (curTime > unloadDetail)
                {
                    curTime = 0;
                    UnloadUnusedAssets();
                }
                var index = 0;
                for (var i = 0; i < _asyncLoadingOperationList.Count; i++)
                {
                    index++;
                    if (index > MaxLoadNum) return;
                    var operation = _asyncLoadingOperationList[i];
                    operation.Update();
                    if (!operation.mIsDone) continue;
                    operation.DoCompleted();
                    _asyncLoadingOperationList.RemoveAt(i--);
                    _asyncLoadedOperationList.Add(operation);
                }
            }

            public void ReleaseAsset(AssetAsyncOperation assetAsyncOperation)
            {
                if (assetAsyncOperation == null)
                {
                    return;
                }
                if (AppConst.ASSET_BUNDLE_MODE)
                {
                    _bundleMgr.ReleaseBundleCollection(assetAsyncOperation.mCollection);
                }
                _asyncLoadingOperationList.Remove(assetAsyncOperation);
                _asyncLoadedOperationList.Remove(assetAsyncOperation);
                AssetAsyncOperation.Destroy(assetAsyncOperation);
            }

            public bool ReleaseSubAsset(SubAssetAsyncOperation assetAsyncOperation)
            {
                if (assetAsyncOperation == null)
                {
                    return false;
                }

                var completelyRelease = false;
                if (AppConst.ASSET_BUNDLE_MODE)
                {
                    _bundleMgr.ReleaseBundleCollection(assetAsyncOperation.mCollection);
                    var count = assetAsyncOperation.mCollection.mMainBundleAsyncOperation.mReferenceCount;
                    if (count <= 0) completelyRelease = true;
                }
                _asyncLoadingOperationList.Remove(assetAsyncOperation);
                _asyncLoadedOperationList.Remove(assetAsyncOperation);
                SubAssetAsyncOperation.Destroy(assetAsyncOperation);
                return completelyRelease;
            }

            public void UnloadScene(SceneAsyncOperation sceneAsyncOperation)
            {
                if (AppConst.ASSET_BUNDLE_MODE)
                {
                    _bundleMgr.ReleaseBundleCollection(sceneAsyncOperation.mCollection);
                }
                _asyncLoadingOperationList.Remove(sceneAsyncOperation);
                _asyncLoadedOperationList.Remove(sceneAsyncOperation);
                SceneAsyncOperation.Destroy(sceneAsyncOperation);
            }


            public void UnloadUnusedAssets()
            {
                GC.Collect();
                Resources.UnloadUnusedAssets();
            }

            /// <summary>
            /// 获取资源版本   之前放assetmanager 先临时放这里
            /// </summary>
            /// <returns></returns>
            public int GetResVersion()
            {
                int version = 0;
                if (AppConst.ASSET_BUNDLE_MODE)
                {
                    version = _patchMgr.LocalInfo.ResVersion;
                }
                return version;
            }

            public void LoadStaticScene(string sceneName)
            {
                SceneManager.LoadScene("Boot/" + sceneName);
            }
        }
    }
}