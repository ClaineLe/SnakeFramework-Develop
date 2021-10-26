#if UNITY_EDITOR
using com.halo.framework.runtime;
using System.IO;
using com.snake.framework;
using HaloFramework.Runtime;

namespace HaloFramework.Editor
{
    public class EditorAssetAsyncOperation : AssetAsyncOperation
    {
        protected UnityEngine.Object _result;
        protected bool _isDone = false;
        protected bool _tickOneFrame = false;
        protected bool _error = false;
        public override UnityEngine.Object mResult => _result;
        public override bool mIsDone => _isDone;
        public override float mProgress => _result == null ? 0.0f : 1.0f;
        public EditorAssetAsyncOperation()
        {
            OnReferenceClear();
        }
        public static EditorAssetAsyncOperation Create(string assetPath, System.Type assetType, BundleOperationCollection collection, System.Action<IAssetAsyncOperation> completed)
        {
            EditorAssetAsyncOperation assetAsyncOperation = ReferencePool.Take<EditorAssetAsyncOperation>();
            assetAsyncOperation._SetCompleted(completed);
            assetAsyncOperation.mAssetPath = assetPath;
            assetAsyncOperation.mAssetType = assetType;
            return assetAsyncOperation;
        }

        public override void OnReferenceClear()
        {
            this._result = null;
            this._tickOneFrame = false;
            this._error = false;
            this._isDone = false;
            base.OnReferenceClear();
        }
        public override void Update()
        {
            if (_result != null || _error != false) return;
            if (_tickOneFrame == false)
            {
                _tickOneFrame = true;
                return;
            }


            FileInfo file = new FileInfo(mAssetPath);
            if (!PathChecker.CheckPathAndFileExits(file))
            {
                _error = true;
                Debuger.Error("资源加载异常：" + this.mAssetPath);
                _isDone = true;
                return;
            }

            _result = UnityEditor.AssetDatabase.LoadAssetAtPath(this.mAssetPath, this.mAssetType);
            if (_result == null)
            {
                _error = true;
                Debuger.Error("资源加载异常：" + this.mAssetPath);
            }
            _isDone = true;
        }
    }
}
#endif