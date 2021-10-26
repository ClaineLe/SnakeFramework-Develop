#if UNITY_EDITOR

using System.IO;
using com.halo.framework.runtime;
using com.snake.framework;
using HaloFramework.Runtime;

namespace HaloFramework.Editor
{
    public class EditorSubAssetAsyncOperation : SubAssetAsyncOperation
    {
        private UnityEngine.Object[] _mResultArray;
        public override UnityEngine.Object[] mResultArray
        {
            get
            {
                return _mResultArray;
            }
        }
        protected bool _isDone = false;
        protected bool _tickOneFrame = false;
        protected bool _error = false;
        public override bool mIsDone => _isDone;
        public EditorSubAssetAsyncOperation()
        {
            OnReferenceClear();
        }
        public static EditorSubAssetAsyncOperation Create(string assetPath, System.Type assetType, BundleOperationCollection collection, System.Action<IAssetAsyncOperation> completed)
        {
            EditorSubAssetAsyncOperation subAssetAsyncOperation = ReferencePool.Take<EditorSubAssetAsyncOperation>();
            subAssetAsyncOperation._SetCompleted(completed);
            subAssetAsyncOperation.mAssetPath = assetPath;
            subAssetAsyncOperation.mAssetType = assetType;
            return subAssetAsyncOperation;
        }

        public override void OnReferenceClear()
        {
            this._tickOneFrame = false;
            this._error = false;
            this._isDone = false;
            _mResultArray = null;
            base.OnReferenceClear();
        }

        public override void Update()
        {
            if (_mResultArray != null || _error != false) return;
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

            _mResultArray = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(this.mAssetPath);
           
            _isDone = true;
        }
    }
}
#endif