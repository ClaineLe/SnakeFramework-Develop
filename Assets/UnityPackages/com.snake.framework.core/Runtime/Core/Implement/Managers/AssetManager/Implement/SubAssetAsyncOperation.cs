using com.snake.framework;

namespace com.halo.framework
{
    namespace runtime
    {
        public class SubAssetAsyncOperation : AssetAsyncOperation
        {
            public virtual UnityEngine.Object[] mResultArray
            {
                get
                {
                    if (this._assetBundleRequest == null)
                        return null;
                    return _assetBundleRequest.allAssets;
                }
            }

            public SubAssetAsyncOperation()
            {
                OnReferenceClear();
            }
            public static SubAssetAsyncOperation Create(string assetPath, System.Type assetType, BundleOperationCollection collection, System.Action<IAssetAsyncOperation> completed)
            {
                SubAssetAsyncOperation subAssetAsyncOperation = ReferencePool.Take<SubAssetAsyncOperation>();
                subAssetAsyncOperation._SetBundleOperation(collection);
                subAssetAsyncOperation._SetCompleted(completed);
                subAssetAsyncOperation.mAssetPath = assetPath;
                subAssetAsyncOperation.mAssetType = assetType;
                return subAssetAsyncOperation;
            }
            public static void Destroy(SubAssetAsyncOperation baseAsyncOperation)
            {
                ReferencePool.Return(baseAsyncOperation);
            }

            public override void OnReferenceClear()
            {
                base.OnReferenceClear();
            }

            protected override void onBeforeCompleted()
            {
                if (_assetBundleRequest == null) return;
                base.onBeforeCompleted();
            }

            public override void Update()
            {
                if (_assetBundleRequest != null || AllBundlePrepared() == false) return;
                var assetBundle = this.mCollection.mMainBundleAsyncOperation.GetAssetBundle();
                _assetBundleRequest = assetBundle.LoadAssetWithSubAssetsAsync(this.mAssetPath);
            }
        }
    }
}