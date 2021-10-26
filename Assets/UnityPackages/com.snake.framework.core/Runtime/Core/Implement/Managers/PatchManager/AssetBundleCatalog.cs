﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.halo.framework
{
    namespace runtime
    {
        [System.Serializable]
        public class AssetBundleCatalog
        {
            //AssetPath - AssetBundleName
            public Dictionary<string, string> mPathMapping = new Dictionary<string, string>();

            //AssetBundleName - Depens
            public Dictionary<string, string[]> mDepensMapping = new Dictionary<string, string[]>();
        }
    }
}