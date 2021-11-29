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

            [MenuItem("SnakeTools/BuildAssetBundle(Android)")]
            static public void Build_AssetBundle_for_Android()
            {
                BuildTarget buildTarget = BuildTarget.Android;
                string outputPath = "AssetBundle";
                string extOutputPath = "ext_source";
                //AssetBundleBuilder.BuildAssetBundle(outputPath, buildTarget, null, extOutputPath);
            }

            [MenuItem("SnakeTools/Debug-Using")]
            static public void SyncSnakeFramework_Using()
            {
                string repositoriePath = "../SnakeFramework-Using/Assets/";
                Utility.Fold.ClearFold(repositoriePath, null);
                Utility.Fold.CopyFold("Assets", repositoriePath, new[] { "UnityPackages" });
            }

            [MenuItem("SnakeTools/Debug-UPM/com.snake.framework.core")]
            static public void SyncSnakeFramework_Core_Debug()
            {
                _SyncSnakeFramework_Core(true);
            }

            [MenuItem("SnakeTools/Debug-UPM/com.snake.framework.imp-network")]
            static public void SyncSnakeFramework_ImpNetWork_Debug()
            {
                _SyncSnakeFramework_ImpNetWork(true);
            }

            [MenuItem("SnakeTools/Release-UPM/com.snake.framework.core")]
            static public void SyncSnakeFramework_Core_Release()
            {
                _SyncSnakeFramework_Core(false);
            }

            [MenuItem("SnakeTools/Release-UPM/com.snake.framework.imp-network")]
            static public void SyncSnakeFramework_ImpNetWork_Release()
            {
                _SyncSnakeFramework_ImpNetWork(false);
            }

            [MenuItem("SnakeTools/Sync-UPM/com.snake.framework.fork-fairygui")]
            static public void SyncSnakeFramework_ForkFairyGUI_Release()
            {
                string unityPackageName = "com.snake.framework.fork-fairygui";
                string repositoriePath = "../SnakeFramework_ForkFairyGUI/";
                _CopyToGitRepo(unityPackageName, repositoriePath, new[] { "\\.git", }, null, true);

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
                    SnakeDebuger.Error("fold is not exists" + foldInfo.FullName);
                    return;
                }
                Utility.Fold.ClearFold(repositoriePath, repoIgnores);
                Utility.Fold.CopyFold(fullPath, repositoriePath, copyIgnores);

                SnakeDebuger.Log((debug ? "debug" : "release") + unityPackageName);
            }
        }
    }
}