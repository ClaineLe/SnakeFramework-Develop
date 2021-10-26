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
        public class SourceInfoDictionary
        {
            public Dictionary<string, SourceInfo> Data;

            /// <summary>
            /// 无参构造
            /// </summary>
            public SourceInfoDictionary()
            {
                Data = new Dictionary<string, SourceInfo>();
            }

            /// <summary>
            /// 有参构造
            /// </summary>
            /// <param name="data"></param>
            public SourceInfoDictionary(Dictionary<string, SourceInfo> data)
            {
                Data = new Dictionary<string, SourceInfo>(data);
            }
        }
    }
}