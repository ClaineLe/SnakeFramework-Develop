#if UNITY_EDITOR
using com.halo.framework.runtime;
using com.snake.framework;
using UnityEngine.SceneManagement;
namespace HaloFramework.Editor
{
    public class EditorSceneAsyncOperation : SceneAsyncOperation
    {
        static new public EditorSceneAsyncOperation Create(string scenePath, BundleOperationCollection collection, System.Action<IAssetAsyncOperation> completed, bool additive = false)
        {
            EditorSceneAsyncOperation sceneAsyncOperation = ReferencePool.Take<EditorSceneAsyncOperation>();
            sceneAsyncOperation._SetBundleOperation(collection);
            sceneAsyncOperation._SetCompleted(completed);
            sceneAsyncOperation.mSceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            sceneAsyncOperation.mScenePath = scenePath;
            sceneAsyncOperation.mLoadSceneMode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            return sceneAsyncOperation;
        }
        public override void Update()
        {
            if (mAsyncOperation == null)
            {
                mAsyncOperation = UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsync(this.mSceneName, mLoadSceneMode);
            }
        }
    }
}
#endif
