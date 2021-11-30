using UnityEditor;
using UnityEditor.Build.Reporting;

namespace com.snake.framework
{
    namespace editor
    {
        public class SnakeBuilder
        {
            /// <summary>
            /// 提供给Jenkins调用的接口，用于构建APP
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