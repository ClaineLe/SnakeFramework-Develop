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

            [Header("[Editor]Bundle��Դ��StreamingAssets�µ����·��")]
            public string mDefBundleOutputPath;

            [Header("[Editor]ʹ������Դ��¼�ļ�·��")]
            public string mDefAssetRecordFile;

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
