using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace com.snake.framework
{
    namespace runtime
    {
        internal class DownloadConfig : IReference
        {
            public string downloadPath;
            public string savePath;
            public long size;

            public void OnReferenceClear()
            {
                this.downloadPath = string.Empty;
                this.savePath = string.Empty;
                this.size = 0;
            }
        }
    }
}