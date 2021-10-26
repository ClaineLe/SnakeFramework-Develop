using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.halo.framework.runtime;

namespace com.halo.framework
{
    namespace runtime
    {
        /// <summary>
        /// 所有库的版本数据
        /// </summary>
        [Serializable]
        public class VersionRecord
        {
            /// <summary>
            /// 对应数据
            /// </summary>
            public Dictionary<string, Dictionary<string, Dictionary<RunMode, RemoteInfo>>> ChannelDict;

            /// <summary>
            /// 白名单
            /// </summary>
            public Dictionary<string, string> WhiteDeviceInfoList;


            /// <summary>
            /// 最后一次构建的版本号
            /// </summary>
            public int LatestBuildCode { get; set; }

            public VersionRecord()
            {
                ChannelDict = new Dictionary<string, Dictionary<string, Dictionary<RunMode, RemoteInfo>>>();
                WhiteDeviceInfoList = new Dictionary<string, string>();
            }
        }
    }
}
