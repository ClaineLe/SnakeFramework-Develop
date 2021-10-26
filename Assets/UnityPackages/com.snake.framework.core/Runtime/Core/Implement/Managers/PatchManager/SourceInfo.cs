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
        public class SourceInfo
        {
            public int resVersion;
            public string md5;
            public long size;

            public SourceInfo()
            {
            }

            public SourceInfo(SourceInfo sourceInfo)
            {
                resVersion = sourceInfo.resVersion;
                md5 = sourceInfo.md5;
                size = sourceInfo.size;
            }
        }
    }
}