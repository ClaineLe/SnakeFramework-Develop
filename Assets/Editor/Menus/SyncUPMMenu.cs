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
                _TickVersion(fullPath, 0, 0, 0, 1);
                System.IO.DirectoryInfo foldInfo = new System.IO.DirectoryInfo(repositoriePath);
                if (foldInfo.Exists == false)
                {
                    Debug.LogError("目录不存在：" + foldInfo.FullName);
                    return;
                }

                foreach (var a in foldInfo.GetFileSystemInfos("*", System.IO.SearchOption.AllDirectories))
                {
                    if (a.FullName.Contains("\\.git"))
                        continue;
                    if (a.Exists == false)
                        continue;
                    FileUtil.DeleteFileOrDirectory(a.FullName.FixSlash());
                }

                FileUtil.CopyFileOrDirectory(fullPath + "/Common", repositoriePath + "/Common");
                FileUtil.CopyFileOrDirectory(fullPath + "/Editor", repositoriePath + "/Editor");
                FileUtil.CopyFileOrDirectory(fullPath + "/Runtime", repositoriePath + "/Runtime");
                foreach (var a in System.IO.Directory.GetFiles(fullPath, "*", System.IO.SearchOption.TopDirectoryOnly))
                {
                    string fileName = System.IO.Path.GetFileName(a);
                    FileUtil.CopyFileOrDirectory(fullPath + "/" + fileName, repositoriePath + "/" + fileName);
                }

            }

            [MenuItem("SnakeTools/同步UPM/com.snake.framework.imp-network")]
            static public void SyncSnakeFramework_ImpNetWork()
            {
                string unityPackageName = "com.snake.framework.imp-network";
                string repositoriePath = "../SnakeFramework-ImpNetWork/";
                string fullPath = UPM_PATH_ROOT + "/" + unityPackageName;
                _TickVersion(fullPath, 0, 0, 0, 1);
                System.IO.DirectoryInfo foldInfo = new System.IO.DirectoryInfo(repositoriePath);
                if (foldInfo.Exists == false)
                {
                    Debug.LogError("目录不存在：" + foldInfo.FullName);
                    return;
                }

                foreach (var a in foldInfo.GetFileSystemInfos("*", System.IO.SearchOption.AllDirectories))
                {
                    if (a.FullName.Contains("\\.git"))
                        continue;
                    if (a.Exists == false)
                        continue;
                    FileUtil.DeleteFileOrDirectory(a.FullName.FixSlash());
                }

                FileUtil.CopyFileOrDirectory(fullPath + "/Runtime", repositoriePath + "/Runtime");
                FileUtil.CopyFileOrDirectory(fullPath + "/Sample", repositoriePath + "/Sample");
                foreach (var a in System.IO.Directory.GetFiles(fullPath, "*", System.IO.SearchOption.TopDirectoryOnly))
                {
                    string fileName = System.IO.Path.GetFileName(a);
                    FileUtil.CopyFileOrDirectory(fullPath + "/" + fileName, repositoriePath + "/" + fileName);
                }

            }

            [MenuItem("SnakeTools/同步UPM/com.snake.framework.imp-xlua")]
            static public void SyncSnakeFramework_ImpXLua()
            {
                string unityPackageName = "com.snake.framework.imp-xlua";
                string repositoriePath = "../SnakeFramework-ImpXLua/";
                string fullPath = UPM_PATH_ROOT + "/" + unityPackageName;
                _TickVersion(fullPath, 0, 0, 0, 1);
                System.IO.DirectoryInfo foldInfo = new System.IO.DirectoryInfo(repositoriePath);
                if (foldInfo.Exists == false)
                {
                    Debug.LogError("目录不存在：" + foldInfo.FullName);
                    return;
                }

                System.IO.Directory.Delete(repositoriePath + "/Runtime", true);
                System.IO.Directory.Delete(repositoriePath + "/Sample", true);
                foreach (var a in foldInfo.GetFiles("*", System.IO.SearchOption.TopDirectoryOnly))
                    a.Delete();

                FileUtil.CopyFileOrDirectory(fullPath + "/Runtime", repositoriePath + "/Runtime");
                FileUtil.CopyFileOrDirectory(fullPath + "/Sample", repositoriePath + "/Sample");
                foreach (var a in System.IO.Directory.GetFiles(fullPath, "*", System.IO.SearchOption.TopDirectoryOnly))
                {
                    string fileName = System.IO.Path.GetFileName(a);
                    FileUtil.CopyFileOrDirectory(fullPath + "/" + fileName, repositoriePath + "/" + fileName);
                }
            }

            static private System.Version _TickVersion(string upPath, int major, int minor, int build, int revision, bool autoSave = true)
            {
                string key = "version";
                string jsonPath = upPath + "/package.json";
                JObject upmObject = _ReadPackageJson(jsonPath);
                System.Version oldVersion = System.Version.Parse(upmObject.GetValue(key).Value<string>());
                System.Version newVersion = new System.Version(
                    oldVersion.Major + major,
                    oldVersion.Minor + minor,
                    oldVersion.Build + build,
                    oldVersion.Revision + revision);
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