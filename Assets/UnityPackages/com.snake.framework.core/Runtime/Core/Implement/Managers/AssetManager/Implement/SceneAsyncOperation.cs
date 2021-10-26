using com.snake.framework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.halo.framework
{
    namespace runtime
    {
        public class SceneAsyncOperation : BaseAsyncOperation, ISceneAsyncOperation
        {
            public string mSceneName { get; protected set; }
            public string mScenePath { get; protected set; }
            public Scene mScene { get; protected set; }
            public LoadSceneMode mLoadSceneMode { get; protected set; }
            public AsyncOperation mAsyncOperation { get; protected set; }
            public override bool mIsDone
            {
                get
                {
                    if (mAsyncOperation == null)
                        return false;
                    return mAsyncOperation.isDone;
                }
            }
            public override float mProgress
            {
                get
                {
                    if (mAsyncOperation == null)
                        return 0.0f;
                    return mAsyncOperation.progress;
                }
            }


            public SceneAsyncOperation() { }
            static public SceneAsyncOperation Create(string scenePath, BundleOperationCollection collection, System.Action<IAssetAsyncOperation> completed, bool additive = false)
            {
                SceneAsyncOperation sceneAsyncOperation = ReferencePool.Take<SceneAsyncOperation>();
                sceneAsyncOperation._SetBundleOperation(collection);
                sceneAsyncOperation._SetCompleted(completed);
                sceneAsyncOperation.mSceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                sceneAsyncOperation.mScenePath = scenePath;
                sceneAsyncOperation.mLoadSceneMode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
                return sceneAsyncOperation;
            }
            static public void Destroy(SceneAsyncOperation sceneAsyncOperation)
            {
                ReferencePool.Return(sceneAsyncOperation);
            }
            public override void OnReferenceClear()
            {
                this.mSceneName = string.Empty;
                this.mScenePath = string.Empty;
                this.mAsyncOperation = null;
                this.mLoadSceneMode = default;
                this.mScene = default;
                base.OnReferenceClear();
            }
            protected override void onBeforeCompleted()
            {
                base.onBeforeCompleted();
                this.mScene = SceneManager.GetSceneByName(this.mSceneName);
#if UNITY_EDITOR
                AssetModify.ModifyScene(this.mScene);
#endif
            }

            public override void Update()
            {
                if (mAsyncOperation != null || !AllBundlePrepared())  return;
                mAsyncOperation = SceneManager.LoadSceneAsync(this.mSceneName, mLoadSceneMode);
            }
        }
    }
}