using com.snake.framework.runtime;
using UnityEngine;

namespace com.snake.framework
{
    namespace editor
    {
        [UnityEngine.CreateAssetMenu(fileName = "BuilderSetting")]
        public class BuilderSetting : SnakeBasicSetting<BuilderSetting>
        {
            [Header("[Editor]��Դ�������·��")]
            public string mAssetRulesPath;


            static public BuilderSetting EditorGet()
            {
                EnvironmentSetting envSetting = EnvironmentSetting.Get();
                if (envSetting == null)
                    return null;
                return editorGet(envSetting.mBuilderSettingPath);
            }

        }
    }
}
