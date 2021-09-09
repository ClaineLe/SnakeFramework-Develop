using UnityEditor;
using Newtonsoft.Json;
using System.Data;
using UnityEngine;
using UnityEditorInternal;
using Newtonsoft.Json.Linq;

namespace com.snake.framework
{
    namespace custom.editor
    {
        public class SyncUPMMenu
        {
            private const string UPM_PATH_ROOT = "Assets/UnityPackages";

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
                    Debug.LogError("目录不存在：" + foldInfo.FullName);
                    return;
                }
                Utility.Fold.ClearFold(repositoriePath, repoIgnores);
                Utility.Fold.CopyFold(fullPath, repositoriePath, copyIgnores);

                Debug.Log((debug ? "调试" : "发布") + unityPackageName);
            }
        }
    }
}