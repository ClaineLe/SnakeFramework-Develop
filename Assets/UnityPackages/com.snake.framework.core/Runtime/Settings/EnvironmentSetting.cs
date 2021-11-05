using UnityEngine;

namespace com.snake.framework
{
    namespace runtime
    {
        [UnityEngine.CreateAssetMenu(fileName = "EnvironmentSetting")]
        public class EnvironmentSetting : SnakeBasicSetting<EnvironmentSetting>
        {

            [Header("资源根目录路径")]
            public string mResRootPath;

#if UNITY_EDITOR
            [Header("资源打包规整路径")]
            public string mAssetRulesPath;
#endif
        }
    }
}