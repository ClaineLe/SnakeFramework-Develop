using System.Collections.Generic;
namespace com.halo.framework
{
    namespace runtime
    {
        public interface IAssetAsyncOperation
        {
            string mAssetPath { get; }
            System.Type mAssetType { get; }
            UnityEngine.Object mResult { get; set; }
        }
    }
}