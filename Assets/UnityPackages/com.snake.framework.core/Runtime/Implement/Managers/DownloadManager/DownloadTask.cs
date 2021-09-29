using System.Threading.Tasks;
using UnityEngine.Networking;

namespace com.snake.framework
{
    namespace runtime
    {
        /// <summary>
        /// 下载任务
        /// </summary>
        internal class DownloadTask : IReference
        {
            public enum STATE
            {
                none,
                success,
                fail,
                downloading,
            }

            /// <summary>
            /// 当前下载状态
            /// </summary>
            public STATE mState { get; private set; } = STATE.none;

            /// <summary>
            /// 错误信息
            /// </summary>
            public string mError { get; private set; } = string.Empty;

            /// <summary>
            /// 下载信息
            /// </summary>
            public DownloadConfig mDownloadConfig { get; private set; }

            /// <summary>
            /// 下载请求
            /// </summary>
            private UnityWebRequest _request;

            /// <summary>
            /// 下载进度
            /// </summary>
            public float mProgress => _request == null ? 0 : _request.downloadProgress;

            /// <summary>
            /// 获取已经下载的文件数据大小
            /// </summary>
            /// <returns></returns>
            public long GetDownloadedSize()
            {
                return (long)(mProgress * this.mDownloadConfig.size);
            }


            public void SetDownloadConfig(DownloadConfig downloadConfig) 
            {
                this.mDownloadConfig = downloadConfig;
            }

            /// <summary>
            /// 开始下载
            /// </summary>
            async public Task StartDownLoad()
            {
                _request.downloadHandler = new DownloadHandlerFile(mDownloadConfig.savePath, false);
                ((DownloadHandlerFile)_request.downloadHandler).removeFileOnAbort = true;
                this.mState = STATE.downloading;
                await _request.SendWebRequest();
                if (_request.result != UnityWebRequest.Result.Success)
                {
                    mError = $"下载文件错误\nErrorMsg:{_request.error}  Result:{_request.result} \nurl:{mDownloadConfig.downloadPath}";
                    this.mState = STATE.fail;
                }
                else
                {
                    this.mState = STATE.success;
                }
            }

            /// <summary>
            /// 对象回退对象池
            /// </summary>
            public void OnReferenceClear()
            {
                mState = STATE.none;
                mError = string.Empty;
                mDownloadConfig = null;
                if (_request != null)
                {
                    _request.Dispose();
                    _request = null;
                }
            }
        }
    }
}