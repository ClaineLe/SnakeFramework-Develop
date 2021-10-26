using UnityEngine;

namespace com.halo.framework
{
    namespace runtime
    {
        public sealed class BundleAsyncOperation
        {
            public string mBundleFullName;
            public AssetBundleCreateRequest mAssetBundleCreateRequest;
            public int mReferenceCount;
            public AssetBundle GetAssetBundle()
            {
                if (this.mAssetBundleCreateRequest == null)
                    return null;
                return this.mAssetBundleCreateRequest.assetBundle;
            }
            public bool GetIsDone()
            {
                if (mAssetBundleCreateRequest == null)
                    return false;
                return mAssetBundleCreateRequest.isDone;
            }
            public BundleAsyncOperation(string loadPath,string bundleName)
            {
                mAssetBundleCreateRequest = AssetBundle.LoadFromFileAsync(loadPath);
                mBundleFullName = bundleName;
            }

            public void Release(bool unloadAllLoadedObjects)
            {
                AssetBundle assetBundle = GetAssetBundle();
                assetBundle?.Unload(unloadAllLoadedObjects);
            }

            public void AddReferenceCount()
            {
                ++this.mReferenceCount;
            }

            public void ReduceReferenceCount()
            {
                --this.mReferenceCount;
            }
        }
    }
}