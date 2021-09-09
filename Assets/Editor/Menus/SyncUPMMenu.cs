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

            [MenuItem("SnakeTools/同步UPM/com.snake.framework.core")]
            static public void SyncSnakeFramework_Core()
            {
                string unityPackageName = "com.snake.framework.core";
                string repositoriePath = "../SnakeFramework-Core/";
                string fullPath = UPM_PATH_ROOT + "/" + unityPackageName;
                _TickVersion(fullPath, 0, 0, 1);
                System.IO.DirectoryInfo foldInfo = new System.IO.DirectoryInfo(repositoriePath);
                if (foldInfo.Exists == false)
                {
                    Debug.LogError("目录不存在：" + foldInfo.FullName);
                    return;
                }

                Utility.Fold.ClearFold(repositoriePath, new string[]
                {
                    "\\.git",
                });

                Utility.Fold.CopyFold(fullPath, repositoriePath);
            }

            [MenuItem("SnakeTools/同步UPM/com.snake.framework.imp-network")]
            static public void SyncSnakeFramework_ImpNetWork()
            {
                string unityPackageName = "com.snake.framework.imp-network";
                string repositoriePath = "../SnakeFramework-ImpNetWork/";
                string fullPath = UPM_PATH_ROOT + "/" + unityPackageName;
                _TickVersion(fullPath, 0, 0, 1);
                System.IO.DirectoryInfo foldInfo = new System.IO.DirectoryInfo(repositoriePath);
                if (foldInfo.Exists == false)
                {
                    Debug.LogError("目录不存在：" + foldInfo.FullName);
                    return;
                }

                Utility.Fold.ClearFold(repositoriePath, new string[]
                {
                    "\\.git",
                });

                Utility.Fold.CopyFold(fullPath, repositoriePath );

            }

            [MenuItem("SnakeTools/同步UPM/com.snake.framework.imp-xlua")]
            static public void SyncSnakeFramework_ImpXLua()
            {
                string unityPackageName = "com.snake.framework.imp-xlua";
                string repositoriePath = "../SnakeFramework-ImpXLua/";
                string fullPath = UPM_PATH_ROOT + "/" + unityPackageName;
                _TickVersion(fullPath, 0, 0, 1);
                System.IO.DirectoryInfo foldInfo = new System.IO.DirectoryInfo(repositoriePath);
                if (foldInfo.Exists == false)
                {
                    Debug.LogError("目录不存在：" + foldInfo.FullName);
                    return;
                }

                Utility.Fold.ClearFold(repositoriePath, new string[]
                {
                    "\\.git","\\Plugins","\\XLua",
                });

                Utility.Fold.CopyFold(fullPath, repositoriePath, new string[]
                {
                    "\\Plugins","\\XLua",
                });
            }

            static private System.Version _TickVersion(string upPath, int major, int minor, int build, bool autoSave = true)
            {
                string key = "version";
                string jsonPath = upPath + "/package.json";
                JObject upmObject = _ReadPackageJson(jsonPath);
                System.Version oldVersion = System.Version.Parse(upmObject.GetValue(key).Value<string>());
                System.Version newVersion = new System.Version(
                    oldVersion.Major + major,
                    oldVersion.Minor + minor,
                    oldVersion.Build + build);
                upmObject[key] = newVersion.ToString();
                if (autoSave)
                    _WritePackageJson(jsonPath, upmObject);
                return newVersion;
            }

            static private JObject _ReadPackageJson(string jsonPath)
            {
                string json = System.IO.File.ReadAllText(jsonPath);
                return JsonConvert.DeserializeObject<JObject>(json);
            }
            static private void _WritePackageJson(string jsonPath, JObject upmObject)
            {
                string json = JsonConvert.SerializeObject(upmObject, Formatting.Indented);
                System.IO.File.WriteAllText(jsonPath, json, System.Text.Encoding.Default);
            }

        }
    }
}