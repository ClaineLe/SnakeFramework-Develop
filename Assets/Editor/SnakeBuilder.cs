using UnityEditor;
using UnityEditor.Build.Reporting;

namespace com.snake.framework
{
    namespace editor
    {
        public class SnakeBuilder
        {
            /// <summary>
            /// �ṩ��Jenkins���õĽӿڣ����ڹ���APP
            /// </summary>
            public static void BuildForJenkins()
            {
                var buildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = new[]
                    {
                        "Assets/Scenes/BootScene.unity",
                    },
                    locationPathName = "test_SnakeFramework_Develop.apk",
                    target = BuildTarget.Android
                };

                BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
            }
        }
    }
}