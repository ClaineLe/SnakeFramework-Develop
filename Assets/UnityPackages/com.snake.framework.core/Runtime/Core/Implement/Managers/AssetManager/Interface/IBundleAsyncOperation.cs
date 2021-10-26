namespace com.halo.framework
{
    namespace runtime
    {
        public interface IBundleAsyncOperation
        {
            string mBundleName { get; }
            int mRefCount { get; }
        }
    }
}