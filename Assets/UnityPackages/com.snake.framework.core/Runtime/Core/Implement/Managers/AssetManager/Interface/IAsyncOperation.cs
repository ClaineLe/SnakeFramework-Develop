using System;
using UnityEngine;


namespace com.halo.framework
{
    namespace runtime
    {
        public interface IAsyncOperation
        {
            bool mIsDone { get; }
            float mProgress { get; }
            int mPriority { get; }
        }
    }
}
