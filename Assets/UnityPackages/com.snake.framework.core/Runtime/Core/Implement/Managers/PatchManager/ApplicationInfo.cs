using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.halo.framework
{
    namespace runtime
    {
        /// <summary>
        /// 包体信息
        /// </summary>
        [System.Serializable]
        public class ApplicationInfo
        {
            /// <summary>
            /// 渠道名
            /// </summary>
            public string Channel;

            /// <summary>
            /// 构建模式
            /// </summary>
            public string BuildMode;

            /// <summary>
            /// 底包版本
            /// </summary>
            public string AppVersion;

            /// <summary>
            /// 资源版本
            /// </summary>
            public int BasicResVersion;

            /// <summary>
            /// 构建版本数字(只往上自增的那个)
            /// </summary>
            public int BuildVersionCode;

            /// <summary>
            /// 是否热更
            /// </summary>
            public bool Patcher;

            /// <summary>
            /// 包体标识（用于获取服务器的时候）
            /// </summary>
            public int ServerTag = 1;

            /// <summary>
            /// 日志等级
            /// </summary>
            public int LogLevel = 1;
        }
    }
}