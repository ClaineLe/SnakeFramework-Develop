using com.snake.framework.editor;
using com.snake.framework.runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEngine;

namespace com.snake.framework
{
    namespace custom.editor
    {
        public class SyncUPMMenu
        {
            private const string UPM_PATH_ROOT = "Assets/UnityPackages";

            [MenuItem("SnakeTools/资源构建(Android)")]
            static public void Build_AssetBundle_for_Android()
            {
                BuildTarget buildTarget = BuildTarget.Android;
                string outputPath = "AssetBundle";
                string extOutputPath = "ext_source";
                SnakeBuildBundleOptions buildBundleOptions = new SnakeBuildBundleOptions(outputPath, buildTarget);
                if (string.IsNullOrEmpty(extOutputPath) == false)
                    buildBundleOptions.SetExtBundleNamePath(extOutputPath);
                AssetBundleBuilder.BuildAssetBundle(buildBundleOptions, null);
            }

            [MenuItem("SnakeTools/同步Using")]
            static public void SyncSnakeFramework_Using()
            {
                string repositoriePath = "../SnakeFramework-Using/Assets/";
                Utility.Fold.ClearFold(repositoriePath, null);
                Utility.Fold.CopyFold("Assets", repositoriePath, new[] { "UnityPackages" });
            }

            [MenuItem("SnakeTools/调试UPM/com.snake.framework.core")]
            static public void SyncSnakeFramework_Core_Debug()
            {
                _SyncSnakeFramework_Core(true);
            }

            [MenuItem("SnakeTools/调试UPM/com.snake.framework.imp-network")]
            static public void SyncSnakeFramework_ImpNetWork_Debug()
            {
                _SyncSnakeFramework_ImpNetWork(true);
            }

            [MenuItem("SnakeTools/调试UPM/com.snake.framework.imp-xlua")]
            static public void SyncSnakeFramework_ImpXLua_Debug()
            {
                _SyncSnakeFramework_ImpXLua(true);
            }
            [MenuItem("SnakeTools/发布UPM/com.snake.framework.core")]
            static public void SyncSnakeFramework_Core_Release()
            {
                _SyncSnakeFramework_Core(false);
            }

            [MenuItem("SnakeTools/发布UPM/com.snake.framework.imp-network")]
            static public void SyncSnakeFramework_ImpNetWork_Release()
            {
                _SyncSnakeFramework_ImpNetWork(false);
            }

            [MenuItem("SnakeTools/发布UPM/com.snake.framework.imp-xlua")]
            static public void SyncSnakeFramework_ImpXLua_Release()
            {
                _SyncSnakeFramework_ImpXLua(false);
            }


            [MenuItem("SnakeTools/同步UPM/com.snake.framework.fork-fairygui")]
            static public void SyncSnakeFramework_ForkFairyGUI_Release()
            {
                string unityPackageName = "com.snake.framework.fork-fairygui";
                string repositoriePath = "../SnakeFramework_ForkFairyGUI/";
                _CopyToGitRepo(unityPackageName, repositoriePath, new[] { "\\.git", }, null, true);

            }

            [MenuItem("SnakeTools/导出UPM到Halo项目中/com.snake.framework.core")]
            static private void _ExportSnakeFramework_Core()
            {
                string repositoriePath = @"F:\WorkSpace\gitlab\halo_windf_client\client\Packages\com.snake.framework.core";
                Utility.Fold.ClearFold(repositoriePath, null);
                Utility.Fold.CopyFold("Assets/UnityPackages/com.snake.framework.core", repositoriePath);
            }

            static private void _SyncSnakeFramework_Core(bool debug)
            {
                string unityPackageName = "com.snake.framework.core";
                string repositoriePath = "../SnakeFramework-Core/";
                _CopyToGitRepo(unityPackageName, repositoriePath, new[] { "\\.git", }, null, debug);
            }

            static private void _SyncSnakeFramework_ImpNetWork(bool debug)
            {
                string unityPackageName = "com.snake.framework.imp-network";
                string repositoriePath = "../SnakeFramework-ImpNetWork/";
                _CopyToGitRepo(unityPackageName, repositoriePath, new[] { "\\.git", }, null, debug);
            }

            static public void _SyncSnakeFramework_ImpXLua(bool debug)
            {
                string unityPackageName = "com.snake.framework.imp-xlua";
                string repositoriePath = "../SnakeFramework-ImpXLua/";
                _CopyToGitRepo(unityPackageName, repositoriePath, new[] { "\\.git", "\\Plugins", "\\XLua" }, new[] { "\\Plugins", "\\XLua" }, debug);
            }

            static private System.Version _GetVersion(string upPath)
            {
                string key = "version";
                string jsonPath = upPath + "/package.json";
                string json = System.IO.File.ReadAllText(jsonPath);
                JObject upmObject = JsonConvert.DeserializeObject<JObject>(json);
                return System.Version.Parse(upmObject.GetValue(key).Value<string>());
            }

            static private void _SetVersion(string upPath, int major, int minor, int build)
            {
                string key = "version";
                string jsonPath = upPath + "/package.json";
                string json = System.IO.File.ReadAllText(jsonPath);
                JObject upmObject = JsonConvert.DeserializeObject<JObject>(json);
                System.Version newVersion = new System.Version(major, minor, build);
                upmObject[key] = newVersion.ToString();
                json = JsonConvert.SerializeObject(upmObject, Formatting.Indented);
                System.IO.File.WriteAllText(jsonPath, json, System.Text.Encoding.Default);
            }

            static private void _CopyToGitRepo(string unityPackageName, string repositoriePath, string[] repoIgnores, string[] copyIgnores, bool debug)
            {
                string fullPath = UPM_PATH_ROOT + "/" + unityPackageName;
                System.Version version = _GetVersion(fullPath);
                if (debug)
                    _SetVersion(fullPath, version.Major, version.Minor, version.Build + 1);
                else
                    _SetVersion(fullPath, version.Major, version.Minor + 1, 0);
                System.IO.DirectoryInfo foldInfo = new System.IO.DirectoryInfo(repositoriePath);
                if (foldInfo.Exists == false)
                {
                    SnakeDebuger.Error("目录不存在：" + foldInfo.FullName);
                    return;
                }
                Utility.Fold.ClearFold(repositoriePath, repoIgnores);
                Utility.Fold.CopyFold(fullPath, repositoriePath, copyIgnores);

                SnakeDebuger.Log((debug ? "调试" : "发布") + unityPackageName);
            }
        }
    }
}