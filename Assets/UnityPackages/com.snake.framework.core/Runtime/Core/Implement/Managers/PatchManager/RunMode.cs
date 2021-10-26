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
        /// 运行模式
        /// </summary>
        public enum RunMode
        {
            /// <summary>
            /// 发布
            /// </summary>
            Release = 0,

            /// <summary>
            /// 白名单
            /// </summary>
            White = 1,
        }
    }
}
