using System;
using com.snake.framework;
using HaloFramework.Runtime;

namespace com.halo.framework
{
    namespace runtime
    {
        public struct BundleOperationCollection
        {
            public BundleAsyncOperation mMainBundleAsyncOperation;
            public BundleAsyncOperation[] mDependAsyncOperation;
        }

        public abstract class BaseAsyncOperation : IAsyncOperation, IReference, IAssetAsyncOperation
        {
            public abstract bool mIsDone { get; }

            public abstract float mProgress { get; }

            public int mPriority { get; protected set; }

            public Action<IAssetAsyncOperation> mCompleted { get; protected set; }



            public BundleOperationCollection mCollection { get; protected set; }
            public string mAssetPath { get; }
            public Type mAssetType { get; }
            public virtual UnityEngine.Object mResult { get; set; }
            protected virtual void onBeforeCompleted() { }
            protected virtual void onAfterCompleted() { }

            public virtual void Update(){}

            //protected virtual void LoadAsset(){}

            protected void _SetBundleOperation(BundleOperationCollection collection)
            {
                this.mCollection = collection;
            }
            protected void _SetCompleted(Action<IAssetAsyncOperation> completed)
            {
                this.mCompleted = completed;
            }
            public void DoCompleted() 
            {
                onBeforeCompleted();
#if DEBUG
                try
                {
                    mCompleted?.Invoke(this);
                }
                catch (Exception e)
                {
                    Debuger.Error($"加载回调异常\n异常信息:{e.Message}\n异常堆栈:{e.StackTrace}\n堆栈展示结束");
                }
#else
                mCompleted?.Invoke(this);
#endif
                onAfterCompleted();
            }
            public virtual void OnReferenceClear()
            {
                this.mCollection = default;
                this.mPriority = 0;
                this.mCompleted = null;
            }

            public bool AllBundlePrepared()
            {
                if (!AppConst.ASSET_BUNDLE_MODE)
                {
                    return true;
                }
                if (mCollection.mMainBundleAsyncOperation.GetIsDone() == false)
                {
                    return false;
                }
                var count = mCollection.mDependAsyncOperation.Length;
                for (var index = 0; index < count; index++)
                {
                    if (mCollection.mDependAsyncOperation[index].GetIsDone()) continue;
                    return false;
                }
                return true;
            }
        }
    }
}
