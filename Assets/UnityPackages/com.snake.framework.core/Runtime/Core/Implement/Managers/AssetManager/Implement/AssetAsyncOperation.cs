using com.snake.framework;
using UnityEngine;

namespace com.halo.framework
{
    namespace runtime
    {
        public class AssetAsyncOperation : BaseAsyncOperation
        {
            public string mAssetPath { get; protected set; }

            public System.Type mAssetType { get; protected set; }

            public override UnityEngine.Object mResult
            {
                get
                {
                    if (this._assetBundleRequest == null)
                        return null;
                    return this._assetBundleRequest.asset;
                }
            }

            public override bool mIsDone
            {
                get
                {
                    if (this._assetBundleRequest == null)
                        return false;
                    return this._assetBundleRequest.isDone;
                }
            }

            public override float mProgress
            {
                get
                {
                    if (this._assetBundleRequest == null)
                        return 0.0f;
                    return this._assetBundleRequest.progress;
                }
            }


            protected AssetBundleRequest _assetBundleRequest;


            public AssetAsyncOperation()
            {
                OnReferenceClear();
            }
            static public AssetAsyncOperation Create(string assetPath, System.Type assetType, BundleOperationCollection collection, System.Action<IAssetAsyncOperation> completed)
            {
                AssetAsyncOperation assetAsyncOperation = ReferencePool.Take<AssetAsyncOperation>();
                assetAsyncOperation._SetBundleOperation(collection);
                assetAsyncOperation._SetCompleted(completed);
                assetAsyncOperation.mAssetPath = assetPath;
                assetAsyncOperation.mAssetType = assetType;
                return assetAsyncOperation;
            }

            public static void Destroy(AssetAsyncOperation baseAsyncOperation)
            {
                ReferencePool.Return(baseAsyncOperation);
            }

            /// <summary>
            /// 入对象池前
            /// </summary>
            public override void OnReferenceClear()
            {
                this.mAssetPath = string.Empty;
                this.mAssetType = null;
                this._assetBundleRequest = null;
                base.OnReferenceClear();
            }

            /// <summary>
            /// 加载完成前
            /// </summary>
            protected override void onBeforeCompleted()
            {
                base.onBeforeCompleted();

#if UNITY_EDITOR
                if (this.mResult is GameObject gameObject)
                    AssetModify.ModifyAsset(gameObject);
#endif
            }

            public override void Update()
            {
                if (_assetBundleRequest != null|| AllBundlePrepared() == false) return;
                AssetBundle assetBundle = mCollection.mMainBundleAsyncOperation.GetAssetBundle();
                _assetBundleRequest = assetBundle.LoadAssetAsync(this.mAssetPath, this.mAssetType);
            }

        }
    }
}
