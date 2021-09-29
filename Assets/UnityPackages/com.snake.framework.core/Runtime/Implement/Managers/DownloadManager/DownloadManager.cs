using System.Collections.Generic;
using System.Linq;

namespace com.snake.framework
{
    namespace runtime
    {
        /// <summary>
        /// 下载管理器
        /// </summary>
        public class DownloadManager : BaseManager
        {
            /// <summary>
            /// 管理器状态
            /// </summary>
            public enum STATE 
            {
                /// <summary>
                /// 空闲
                /// </summary>
                free,
                /// <summary>
                /// 下载中
                /// </summary>
                loading,
            }

            /// <summary>
            /// 当前状态
            /// </summary>
            public STATE mState { get; private set; }

            /// <summary>
            /// 当前正在下载的任务列表
            /// </summary>
            private List<DownloadTask> _downloadingList;

            /// <summary>
            /// 需要下载的列表
            /// </summary>
            private List<DownloadConfig> _downloadFilePathList;

            /// <summary>
            /// 完成下载列表
            /// </summary>
            private List<DownloadConfig> _downloadCompletedList;

            /// <summary>
            /// 下载失败列表
            /// </summary>
            private List<DownloadConfig> mDownloadFailList;

            /// <summary>
            /// 记录时间
            /// </summary>
            private float _updateDownloadSpeedInterval = 1;

            /// <summary>
            /// 下载速度
            /// </summary>
            public long mDownloadSpeed { get; private set; } = 0;

            /// <summary>
            /// 并行下载数量
            /// </summary>
            public int mMaxDownloadTaskCount { get; private set; } = 0;

            /// <summary>
            /// 获取已经下载的大小
            /// </summary>
            public long mDownloadedSize { get; private set; } = 0;

            /// <summary>
            /// 下载的总大小
            /// </summary>
            public long mTotalDownloadSize { get; private set; } = 0;

            private float _prevTickDownloadSpeedTime;
            private long _prevTickDownloadedSize;

            protected override void onInitialization()
            {
                this._downloadingList = new List<DownloadTask>();
                this._downloadFilePathList = new List<DownloadConfig>();
                this._downloadCompletedList = new List<DownloadConfig>();
                this.mDownloadFailList = new List<DownloadConfig>();
            }
          
            /// <summary>
            /// 重置下载状态
            /// </summary>
            private void reset()
            {
                mState = STATE.free;
                _downloadingList.Clear();
                _downloadFilePathList.Clear();
                _downloadCompletedList.Clear();
                mDownloadFailList.Clear();
                mDownloadSpeed = 0;
                this._updateDownloadSpeedInterval = 1;
                this.mMaxDownloadTaskCount = 5;
                this.mDownloadedSize = 0;
                this.mTotalDownloadSize = 0;
                this._prevTickDownloadSpeedTime = 0;
                this._prevTickDownloadedSize = 0;
            }

            /// <summary>
            /// 计算已经完成下载的大小
            /// </summary>
            private void internalOperationDownloadedSize()
            {
                long downloadingSize = _downloadCompletedList.Sum(a => a.size);
                for (var i = 0; i < _downloadingList.Count; i++)
                {
                    if (_downloadingList[i].mState != DownloadTask.STATE.downloading)
                        continue;
                    downloadingSize += _downloadingList[i].GetDownloadedSize();
                }
                mDownloadedSize = downloadingSize;
            }

            /// <summary>
            /// 更新下载速度
            /// </summary>
            /// <param name="realElapseSeconds"></param>
            private void internalUpdateDownloadSpeed(float realElapseSeconds) 
            {
                if (realElapseSeconds > _prevTickDownloadSpeedTime + _updateDownloadSpeedInterval)
                    return;

                long detailSize = mDownloadedSize - _prevTickDownloadedSize;
                this.mDownloadSpeed = (long)(detailSize / _updateDownloadSpeedInterval);

                _prevTickDownloadSpeedTime = realElapseSeconds;
                _prevTickDownloadedSize = mDownloadedSize;
            }

            /// <summary>
            /// 当LateTick
            /// </summary>
            /// <param name="elapseSeconds"></param>
            /// <param name="realElapseSeconds"></param>
            private void onTick(int frameCount, float time, float deltaTime, float unscaledTime, float realElapseSeconds)
            {
                internalOperationDownloadedSize();
                internalUpdateDownloadSpeed(realElapseSeconds);
            }

            /// <summary>
            /// 开始下载
            /// </summary>
            /// <param name="downloadFilePathQueue"></param>
            /// <param name="downloadingCallback"></param>
            /// <param name="finishCallback"></param>
            public bool EnqueueDownload(string url, string savePath, long size)
            {
                if (mState == STATE.loading)
                    return false;

                DownloadConfig downloadConfig = ReferencePool.Take<DownloadConfig>();
                downloadConfig.downloadPath = url;
                downloadConfig.savePath = savePath;
                downloadConfig.size = size;
                _downloadFilePathList.Add(downloadConfig);
                this.mTotalDownloadSize += downloadConfig.size;
                return true;
            }

            /// <summary>
            /// 开始下载
            /// </summary>
            /// <param name="asyncDownloadTaskCount">下载线程数量，默认：5， 最高：10</param>
            /// <param name="updateDownloadSpeedInterval">下载速度更新频率，单位：秒，默认为1秒</param>
            /// <returns></returns>
            public bool StartDownload(int asyncDownloadTaskCount = 5, float updateDownloadSpeedInterval = 1.0f)
            {
                if (mState == STATE.loading || _downloadFilePathList.Count == 0 || mTotalDownloadSize == 0)
                    return false;
                reset();
                mState = STATE.loading;
                this.mMaxDownloadTaskCount = UnityEngine.Mathf.Clamp(asyncDownloadTaskCount,1,10);
                this._updateDownloadSpeedInterval = updateDownloadSpeedInterval;
                return true;
            }

            /// <summary>
            /// 获取下载出错的信息
            /// </summary>
            /// <returns></returns>
            public string[] GetDownloadErrorInfos()
            {
                if (this.mDownloadFailList.Count == 0)
                    return new string[0];

                string[] errors = new string[this._downloadFilePathList.Count];
                for (int i = 0; i < this._downloadFilePathList.Count; i++)
                {
                    errors[i] = this._downloadFilePathList[i].downloadPath;
                }
                return errors;
            }
        }
    }
}