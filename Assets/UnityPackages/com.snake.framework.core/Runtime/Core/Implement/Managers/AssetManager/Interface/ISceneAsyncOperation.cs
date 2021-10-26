using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.halo.framework
{
    namespace runtime
    {
        public interface ISceneAsyncOperation
        {
            string mSceneName { get; }
            string mScenePath { get; }
            Scene mScene { get; }
            LoadSceneMode mLoadSceneMode { get; }
            AsyncOperation mAsyncOperation { get; }
        }
    }
}