using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace com.halo.framework
{
    namespace runtime
    {
        [System.Serializable]
        public class RemoteInfo
        {
            /// <summary>
            /// 应用版本
            /// </summary>
            public string AppVersion;

            /// <summary>
            /// 构建应用版本号(与游戏热更毫无关系，是打包的时候自增的一个数)
            /// </summary>
            public int BuildNumber;

            /// <summary>
            /// 构建类型(与游戏热更毫无关系，是打整包的时候包的类型，例如debug、release)
            /// </summary>
            public string BuildType;

            /// <summary>
            /// 当前热更版本
            /// </summary>
            public int ResVersion;

            /// <summary>
            /// 底包版本
            /// </summary>
            public int BasicVersion;
        }
    }
}