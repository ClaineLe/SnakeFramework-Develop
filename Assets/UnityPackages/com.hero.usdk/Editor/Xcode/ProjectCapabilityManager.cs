using System;
using System.IO;
using yx;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode.Custom.Extensions;

#if UNITY_XCODE_API_BUILD
namespace UnityEditor.iOS.Xcode
#else
namespace UnityEditor.iOS.Xcode.Custom

#endif
{
    /// <summary>
    /// The ProjectCapabilityManager class helps to add capabilities to the Xcode 
    /// project. This operation potentially involves modification of any of the 
    /// pbxproj file, the entitlements file(s) and Info.plist file(s). The 
    /// manager assumes ownership of all of these files until the last 
    /// WriteToFile() invocation.
    /// </summary>
    public class ProjectCapabilityManager
    {
        private readonly string m_BuildPath;
        private readonly string m_TargetGuid;
        private readonly string m_PBXProjectPath;
        private readonly string m_EntitlementFilePath;
        private PlistDocument m_Entitlements;
        private PlistDocument m_InfoPlist;
        protected internal PBXProject project;

        const string configPath = "Assets/Plugins/HeroUSDKForIOS/UnityForXcodeConfig.json";

        /// <summary>
        /// Creates a new instance of ProjectCapabilityManager. The returned 
        /// instance assumes ownership of the referenced pbxproj project file, 
        /// the entitlements file and project Info.plist files until the last 
        /// WriteToFile() call.
        /// </summary>
        /// <param name="pbxProjectPath">Path to the pbxproj file.</param>
        /// <param name="entitlementFilePath">Path to the entitlements file.</param>
        /// <param name="targetName">The name of the target to add entitlements for.</param>
        public ProjectCapabilityManager(string pbxProjectPath, string entitlementFilePath, string targetName,string sourcePath)
        {
            m_BuildPath = Directory.GetParent(Path.GetDirectoryName(pbxProjectPath)).FullName;

            m_EntitlementFilePath = entitlementFilePath;
            m_PBXProjectPath = pbxProjectPath;
            project = new PBXProject();
            project.ReadFromString(File.ReadAllText(m_PBXProjectPath));
            m_TargetGuid = project.TargetGuidByName(targetName);

            //读取配置文件
            string cpath = configPath;
            string json = File.ReadAllText(cpath);
            Hashtable table = json.hashtableFromJson();

            //lib
            SetLibs(project, table.SGet<Hashtable>("libs"));
            //framework
            SetFrameworks(project, table.SGet<Hashtable>("frameworks"));
            //embedFramework
            SetEmbedFrameworks(project, table.SGet<Hashtable>("embedframeworks"));
            //building setting
            SetBuildProperties(project, table.SGet<Hashtable>("properties"));
            //复制文件
            CopyFiles(project, pbxProjectPath, table.SGet<Hashtable>("files"));
            //复制文件夹
            CopyFolders(project, pbxProjectPath, table.SGet<Hashtable>("folders"));
            //文件编译符号
            SetFilesCompileFlag(project, table.SGet<Hashtable>("filesCompileFlag"));

            //plist
            PlistElementDict rootDict = GetOrCreateInfoDoc().root;
            SetPlist(project, rootDict, table.SGet<Hashtable>("plist"));

            //elements配置
            SetLibs(table.SGet<Hashtable>("xcodeelements"));

            //修改代码

            string fullPath = Path.GetFullPath(sourcePath);
            EditotCode(fullPath);

        }
        /// <summary>
		/// 修改相关代码
		/// </summary>
		/// <param name="fullPath"></param>
        public void EditotCode(string fullPath)
        {
            XClass UnityAppController = new XClass(fullPath + "/Classes/UnityAppController.mm");
            //导入融合SDK
            UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"", "#import <HeroUSDK/HeroUSDK.h>");
            //处理回调
            UnityAppController.WriteBelow("AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);", "\t[[HeroUPlatform sharedInstance] heroU_application:app handleOpenURL:url];");

#if (UNITY_2019_2_OR_NEWER)
                UnityAppController.WriteBelow("NSURL* url = userActivity.webpageURL;", "\t[[HeroUPlatform sharedInstance] handleOpenUniversalLink:userActivity];");
                UnityAppController.WriteBelow("return self;\n}", "- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation \r{ \r \t[[HeroUPlatform sharedInstance] heroU_application:application handleOpenURL:url] ; \r \t return YES; \r} \n- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url \r{ \r \t[[HeroUPlatform sharedInstance] heroU_application:application handleOpenURL:url] ; \r \t return YES; \r} \n ");

#else
            UnityAppController.WriteBelow("return self;\n}", "- (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation \r{ \r \t[[HeroUPlatform sharedInstance] heroU_application:application handleOpenURL:url] ; \r \t return YES; \r} \n- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url \r{ \r \t[[HeroUPlatform sharedInstance] heroU_application:application handleOpenURL:url] ; \r \t return YES; \r} \n - (BOOL)application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(void(^)(NSArray<id<UIUserActivityRestoring>> * __nullable restorableObjects))restorationHandler \r { \r \t [[HeroUPlatform sharedInstance] handleOpenUniversalLink:userActivity] ; \r \t return YES; \r } \n");

#endif
        }
        /// <summary>
        /// elements配置
        /// </summary>
        /// <param name="table"></param>
        public void SetLibs(Hashtable table)
        {
            if (table != null)
            {

                foreach (DictionaryEntry i in table)
                {
                    string key = i.Key.ToString();
                    string val = i.Value.ToString();
                    if (key == "Apple_Push" && val == "YES")
                    {
                        //添加推送
                        AddPushNotifications(false);
                    }
                    else if (key == "Apple_Iap" && val == "YES")
                    {
                        //添加内购
                        AddInAppPurchase();
                    }
                    else if (key == "Apple_SignInWithApple" && val == "YES")
                    {
                        //添加苹果登录
                        AddSignInWithApple(new string[] { "Default" });
                    }
                    else if (key == "Apple_KeyChain")
                    {
                        //添加Keychain
                        AddKeychainSharing(new string[] { val });
                    }
                    else if (key == "Apple_AssociatedDomains")
                    {
                        //添加link
                        AddAssociatedDomains(new string[] { val });
                    }
                }
            }
        }
        /// <summary>
		/// 添加系统库
		/// </summary>
		/// <param name="inst"></param>
		/// <param name="targetGuid"></param>
		/// <param name="lib"></param>
        public void AddLibToProject(PBXProject inst, string targetGuid, string lib)
        {
            string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.AddFileToBuild(targetGuid, fileGuid);
        }
        private static void RemoveLibFromProject(PBXProject inst, string targetGuid, string lib)
        {
            string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.RemoveFileFromBuild(targetGuid, fileGuid);
        }
        //设置frameworks
        private static void SetFrameworks(PBXProject proj, Hashtable table)
        {
            if (table != null)
            {
                string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                ArrayList addList = table["+"] as ArrayList;
                if (addList != null)
                {
                    foreach (string i in addList)
                    {
                        proj.AddFrameworkToProject(target, i, false);
                    }
                }
                ArrayList removeList = table["-"] as ArrayList;
                if (removeList != null)
                {
                    foreach (string i in removeList)
                    {
                        proj.RemoveFrameworkFromProject(target, i);
                    }
                }
            }
        }
        public void SetEmbedFrameworks(PBXProject proj, Hashtable table)
        {
            if (table != null)
            {
                string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                ArrayList addList = table["+"] as ArrayList;
                if (addList != null)
                {
                    foreach (string i in addList)
                    {
                        AddFileToEmbedFrameworksTO(i);
                    }
                }
                ArrayList removeList = table["-"] as ArrayList;
                if (removeList != null)
                {
                    foreach (string i in removeList)
                    {
                        proj.RemoveFrameworkFromProject(target, i);
                    }
                }
            }
        }
        //设置libs
        public void SetLibs(PBXProject proj, Hashtable table)
        {
            if (table != null)
            {
                string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                ArrayList addList = table["+"] as ArrayList;
                if (addList != null)
                {
                    foreach (string i in addList)
                    {
                        AddLibToProject(proj, target, i);
                    }
                }
                ArrayList removeList = table["-"] as ArrayList;
                if (removeList != null)
                {
                    foreach (string i in removeList)
                    {
                        RemoveLibFromProject(proj, target, i);
                    }
                }
            }
        }
        //设置编译属性
        private static void SetBuildProperties(PBXProject proj, Hashtable table)
        {
            if (table != null)
            {
                string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                Hashtable setTable = table.SGet<Hashtable>("=");
                foreach (DictionaryEntry i in setTable)
                {
                    proj.SetBuildProperty(target, i.Key.ToString(), i.Value.ToString());
                }
                Hashtable addTable = table.SGet<Hashtable>("+");
                foreach (DictionaryEntry i in addTable)
                {
                    ArrayList array = i.Value as ArrayList;
                    List<string> list = new List<string>();
                    foreach (var flag in array)
                    {
                        list.Add(flag.ToString());
                    }
                    proj.UpdateBuildProperty(target, i.Key.ToString(), list, null);
                }
                Hashtable removeTable = table.SGet<Hashtable>("-");
                foreach (DictionaryEntry i in removeTable)
                {
                    ArrayList array = i.Value as ArrayList;
                    List<string> list = new List<string>();
                    foreach (var flag in array)
                    {
                        list.Add(flag.ToString());
                    }
                    proj.UpdateBuildProperty(target, i.Key.ToString(), null, list);
                }
            }
        }
        //设置plist
        private static void SetPlist(PBXProject proj, PlistElementDict node, Hashtable arg)
        {
            if (arg != null)
            {
                foreach (DictionaryEntry i in arg)
                {
                    string key = i.Key.ToString();
                    object val = i.Value;
                    var vType = i.Value.GetType();
                    if (vType == typeof(string))
                    {
                        node.SetString(key, (string)val);
                    }
                    else if (vType == typeof(bool))
                    {
                        node.SetBoolean(key, (bool)val);
                    }
                    else if (vType == typeof(double))
                    {
                        int v = int.Parse(val.ToString());
                        node.SetInteger(key, v);
                    }
                    else if (vType == typeof(ArrayList))
                    {
                        var t = node.CreateArray(key);
                        var array = val as ArrayList;
                        SetPlist(proj, t, array);
                    }
                    else if (vType == typeof(Hashtable))
                    {
                        var t = node.CreateDict(key);
                        var table = val as Hashtable;
                        SetPlist(proj, t, table);
                    }
                }
            }
        }
        private static void SetPlist(PBXProject proj, PlistElementArray node, ArrayList arg)
        {
            if (arg != null)
            {
                foreach (object i in arg)
                {
                    object val = i;
                    var vType = i.GetType();
                    if (vType == typeof(string))
                    {
                        node.AddString((string)val);
                    }
                    else if (vType == typeof(bool))
                    {
                        node.AddBoolean((bool)val);
                    }
                    else if (vType == typeof(double))
                    {
                        int v = int.Parse(val.ToString());
                        node.AddInteger(v);
                    }
                    else if (vType == typeof(ArrayList))
                    {
                        var t = node.AddArray();
                        var array = val as ArrayList;
                        SetPlist(proj, t, array);
                    }
                    else if (vType == typeof(Hashtable))
                    {
                        var t = node.AddDict();
                        var table = val as Hashtable;
                        SetPlist(proj, t, table);
                    }
                }
            }
        }
        //复制文件
        private static void CopyFiles(PBXProject proj, string xcodePath, Hashtable arg)
        {
            foreach (DictionaryEntry i in arg)
            {
                string src = Path.Combine(System.Environment.CurrentDirectory, i.Key.ToString());
                string des = Path.Combine(xcodePath, i.Value.ToString());
                CopyFile(proj, xcodePath, src, des);
            }
        }
        //复制文件夹
        private static void CopyFolders(PBXProject proj, string xcodePath, Hashtable arg)
        {
            foreach (DictionaryEntry i in arg)
            {
                string src = Path.Combine(System.Environment.CurrentDirectory, i.Key.ToString());
                string des = Path.Combine(xcodePath, i.Value.ToString());
                CopyFolder(src, des);
                AddFolderBuild(proj, xcodePath, i.Value.ToString());
            }
        }
        private static void CopyFile(PBXProject proj, string xcodePath, string src, string des)
        {
            bool needCopy = NeedCopy(src);
            if (needCopy)
            {
                File.Copy(src, des);
                string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                proj.AddFileToBuild(target, proj.AddFile(des, des.Replace(xcodePath + "/", ""), PBXSourceTree.Absolute));
                AutoAddSearchPath(proj, xcodePath, des);
            }
        }
        private static void CopyFolder(string srcPath, string dstPath)
        {
            if (Directory.Exists(dstPath))
                Directory.Delete(dstPath);
            if (File.Exists(dstPath))
                File.Delete(dstPath);

            Directory.CreateDirectory(dstPath);

            foreach (var file in Directory.GetFiles(srcPath))
            {
                if (NeedCopy(Path.GetFileName(file)))
                {
                    File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));
                }
            }

            foreach (var dir in Directory.GetDirectories(srcPath))
                CopyFolder(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
        }
        private static void AddFolderBuild(PBXProject proj, string xcodePath, string root)
        {
            //获得源文件下所有目录文件
            string currDir = Path.Combine(xcodePath, root);
            if (root.EndsWith(".framework") || root.EndsWith(".bundle"))
            {
                string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                proj.AddFileToBuild(target, proj.AddFile(currDir, root, PBXSourceTree.Source));
                return;
            }
            List<string> folders = new List<string>(Directory.GetDirectories(currDir));
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string t_path = Path.Combine(currDir, name);
                string t_projPath = Path.Combine(root, name);
                if (folder.EndsWith(".framework") || folder.EndsWith(".bundle"))
                {
                    string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                    proj.AddFileToBuild(target, proj.AddFile(t_path, t_projPath, PBXSourceTree.Source));
                    AutoAddSearchPath(proj, xcodePath, t_path);
                }
                else
                {
                    AddFolderBuild(proj, xcodePath, t_projPath);
                }
            }
            List<string> files = new List<string>(Directory.GetFiles(currDir));
            foreach (string file in files)
            {
                if (NeedCopy(file))
                {
                    string name = Path.GetFileName(file);
                    string t_path = Path.Combine(currDir, name);
                    string t_projPath = Path.Combine(root, name);
                    string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
                    proj.AddFileToBuild(target, proj.AddFile(t_path, t_projPath, PBXSourceTree.Source));
                    AutoAddSearchPath(proj, xcodePath, t_path);
                    
                }
            }
        }
        //在复制文件加入工程时，当文件中有framework、h、a文件时，自动添加相应的搜索路径
        private static void AutoAddSearchPath(PBXProject proj, string xcodePath, string filePath)
        {
            if (filePath.EndsWith(".framework"))
            {//添加框架搜索路径
                string addStr = "$PROJECT_DIR" + Path.GetDirectoryName(filePath.Replace(xcodePath, ""));
                Hashtable arg = new Hashtable();
                Hashtable add = new Hashtable();
                arg.Add("+", add);
                arg.Add("=", new Hashtable());
                arg.Add("-", new Hashtable());
                var array = new ArrayList();
                array.Add(addStr);
                add.Add("FRAMEWORK_SEARCH_PATHS", array);
                SetBuildProperties(proj, arg);
            }
            else if (filePath.EndsWith(".h"))
            {//添加头文件搜索路径
                string addStr = "$PROJECT_DIR" + Path.GetDirectoryName(filePath.Replace(xcodePath, ""));
                Hashtable arg = new Hashtable();
                Hashtable add = new Hashtable();
                arg.Add("+", add);
                arg.Add("=", new Hashtable());
                arg.Add("-", new Hashtable());
                var array = new ArrayList();
                array.Add(addStr);
                add.Add("HEADER_SEARCH_PATHS", array);
                SetBuildProperties(proj, arg);
            }
            else if (filePath.EndsWith(".a"))
            {//添加静态库搜索路径
                string addStr = "$PROJECT_DIR" + Path.GetDirectoryName(filePath.Replace(xcodePath, ""));
                Hashtable arg = new Hashtable();
                Hashtable add = new Hashtable();
                arg.Add("+", add);
                arg.Add("=", new Hashtable());
                arg.Add("-", new Hashtable());
                var array = new ArrayList();
                array.Add(addStr);
                add.Add("LIBRARY_SEARCH_PATHS", array);
                SetBuildProperties(proj, arg);
            }
        }
        private static void SetFilesCompileFlag(PBXProject proj, Hashtable arg)
        {
            string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
            foreach (DictionaryEntry i in arg)
            {
                string fileProjPath = i.Key.ToString();
                string fguid = proj.FindFileGuidByProjectPath(fileProjPath);
                ArrayList des = i.Value as ArrayList;
                List<string> list = new List<string>();
                foreach (var flag in des)
                {
                    list.Add(flag.ToString());
                }
                proj.SetCompileFlagsForFile(target, fguid, list);
            }
        }
        private static bool NeedCopy(string file)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileEx = Path.GetExtension(file);
            if (fileName.StartsWith(".") || file.EndsWith(".gitkeep") || file.EndsWith(".DS_Store"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
		/// 添加动态库
		/// </summary>
		/// <param name="frameworks"></param>
        public void AddFileToEmbedFrameworksTO(string frameworkName)
        {
            const string defaultLocationInProj = "Plugins/HeroUSDKForIOS/HeroUsdk/";
            string framework = Path.Combine(defaultLocationInProj, frameworkName);
            string fileGuid = project.AddFile(framework, "Frameworks/" + framework, PBXSourceTree.Sdk);
            PBXProjectExtensions.AddFileToEmbedFrameworks(project, m_TargetGuid, fileGuid);

            project.SetBuildProperty(m_TargetGuid, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");
        }
        

        /// <summary>
        /// Writes the modifications to the project file, entitlements file and 
        /// the Info.plist file. Any external changes to these files after
        /// the ProjectCapabilityManager instance has been created and before
        /// the call to WriteToFile() will be overwritten.
        /// </summary>
        public void WriteToFile()
        {
            File.WriteAllText(m_PBXProjectPath, project.WriteToString());
            if (m_Entitlements != null)
                m_Entitlements.WriteToFile(PBXPath.Combine(m_BuildPath, m_EntitlementFilePath));
            if (m_InfoPlist != null)
                m_InfoPlist.WriteToFile(PBXPath.Combine(m_BuildPath, "Info.plist"));
        }

        /// <summary>
        /// Adds iCloud capability to project
        /// </summary>
        /// <param name="enableKeyValueStorage">Enables key-value storage option if set to true</param>
        /// <param name="enableiCloudDocument">Enables iCloud document option if set to true</param>
        /// <param name="customContainers">A list of custom containers to add</param>
        public void AddiCloud(bool enableKeyValueStorage, bool enableiCloudDocument, 
                              string[] customContainers)
        {
            AddiCloud(enableKeyValueStorage, enableiCloudDocument, true, true, 
                      customContainers);
        }

        /// <summary>
        /// Adds iCloud capability to project
        /// </summary>
        /// <param name="enableKeyValueStorage">Enables key-value storage option if set to true</param>
        /// <param name="enableiCloudDocument">Enables iCloud document option if set to true</param>
        /// <param name="enablecloudKit">Enables cloudKit option if set to true</param>
        /// <param name="addDefaultContainers">Default containers are added if this option is set to true</param>
        /// <param name="customContainers">A list of custom containers to add</param>
        public void AddiCloud(bool enableKeyValueStorage, bool enableiCloudDocument, bool enablecloudKit, bool addDefaultContainers, string[] customContainers)
        {
            var ent = GetOrCreateEntitlementDoc();
            var val = (ent.root[ICloudEntitlements.ContainerIdKey] = new PlistElementArray()) as PlistElementArray;

            // Cloud document storage and CloudKit require specifying services.
            PlistElementArray ser = null;
            if (enableiCloudDocument || enablecloudKit)
                ser = (ent.root[ICloudEntitlements.ServicesKey] = new PlistElementArray()) as PlistElementArray;

            if (enableiCloudDocument)
            {
                val.values.Add(new PlistElementString(ICloudEntitlements.ContainerIdValue));
                ser.values.Add(new PlistElementString(ICloudEntitlements.ServicesDocValue));
                var ubiquity = (ent.root[ICloudEntitlements.UbiquityContainerIdKey] = new PlistElementArray()) as PlistElementArray;

                if (addDefaultContainers)
                    ubiquity.values.Add(new PlistElementString(ICloudEntitlements.UbiquityContainerIdValue));

                if (customContainers != null && customContainers.Length > 0)
                {
                    // For cloud document, custom containers go in the ubiquity values.
                    for (var i = 0; i < customContainers.Length; i++)
                        ubiquity.values.Add(new PlistElementString(customContainers[i]));
                }
            }

            if (enablecloudKit)
            {
                if (addDefaultContainers && !enableiCloudDocument)
                    val.values.Add(new PlistElementString(ICloudEntitlements.ContainerIdValue));

                if (customContainers != null && customContainers.Length > 0)
                {
                    // For CloudKit, custom containers also go in the container id values.
                    for (var i = 0; i < customContainers.Length; i++)
                        val.values.Add(new PlistElementString(customContainers[i]));
                }

                ser.values.Add(new PlistElementString(ICloudEntitlements.ServicesKitValue));
            }

            if (enableKeyValueStorage)
                ent.root[ICloudEntitlements.KeyValueStoreKey] = new PlistElementString(ICloudEntitlements.KeyValueStoreValue);

            project.AddCapability(m_TargetGuid, PBXCapabilityType.iCloud, m_EntitlementFilePath, enablecloudKit);
        }

        /// <summary>
        /// Add Push (or remote) Notifications capability to the project
        /// </summary>
        /// <param name="development">Sets the development option if set to true</param>
        public void AddPushNotifications(bool development)
        {
            GetOrCreateEntitlementDoc().root[PushNotificationEntitlements.Key] = new PlistElementString(development ? PushNotificationEntitlements.DevelopmentValue : PushNotificationEntitlements.ProductionValue);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.PushNotifications, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds Game Center capability to the project
        /// </summary>
        public void AddGameCenter()
        {
            var arr = (GetOrCreateInfoDoc().root[GameCenterInfo.Key] ?? (GetOrCreateInfoDoc().root[GameCenterInfo.Key] = new PlistElementArray())) as PlistElementArray;
            arr.values.Add(new PlistElementString(GameCenterInfo.Value));
            project.AddCapability(m_TargetGuid, PBXCapabilityType.GameCenter);
        }

        /// <summary>
        /// Adds wallet capability to the project.
        /// </summary>
        /// <param name="passSubset">Controls the allowed pass types. If null or
        /// empty, then all team pass types are allowed. Otherwise, only the 
        /// specified subset of pass types is allowed</param>
        public void AddWallet(string[] passSubset)
        {
            var arr = (GetOrCreateEntitlementDoc().root[WalletEntitlements.Key] = new PlistElementArray()) as PlistElementArray;
            if ((passSubset == null || passSubset.Length == 0) && arr != null)
            {
                arr.values.Add(new PlistElementString(WalletEntitlements.BaseValue + WalletEntitlements.BaseValue));
            }
            else
            {
                for (var i = 0; i < passSubset.Length; i++)
                {
                    if (arr != null)
                        arr.values.Add(new PlistElementString(WalletEntitlements.BaseValue + passSubset[i]));
                }
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.Wallet, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds Siri capability to project.
        /// </summary>
        public void AddSiri()
        {
            GetOrCreateEntitlementDoc().root[SiriEntitlements.Key] = new PlistElementBoolean(true);

            project.AddCapability(m_TargetGuid, PBXCapabilityType.Siri, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds Apple Pay capability to the project.
        /// </summary>
        /// <param name="merchants">The list of merchant IDs to configure</param>
        public void AddApplePay(string[] merchants)
        {
            var arr = (GetOrCreateEntitlementDoc().root[ApplePayEntitlements.Key] = new PlistElementArray()) as PlistElementArray;
            for (var i = 0; i < merchants.Length; i++)
            {
                arr.values.Add(new PlistElementString(merchants[i]));
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.ApplePay, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds In App Purchase capability to the project.
        /// </summary>
        public void AddInAppPurchase()
        {
            project.AddCapability(m_TargetGuid, PBXCapabilityType.InAppPurchase);
        }
        public void AddSignInWithApple(string[] domains)
        {
            var arr = (GetOrCreateEntitlementDoc().root[SingInWithApplentitlements.Key] = new PlistElementArray()) as PlistElementArray;
            for (var i = 0; i < domains.Length; i++)
            {
                arr.values.Add(new PlistElementString(domains[i]));
            }
            project.AddCapability(m_TargetGuid, PBXCapabilityType.SignInWithApple, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds Maps capability to the project.
        /// </summary>
        /// <param name="options">The routing options to configure.</param>
        public void AddMaps(MapsOptions options)
        {
            var bundleArr = (GetOrCreateInfoDoc().root[MapsInfo.BundleKey] ?? (GetOrCreateInfoDoc().root[MapsInfo.BundleKey] = new PlistElementArray())) as PlistElementArray;
            bundleArr.values.Add(new PlistElementDict());
            PlistElementDict bundleDic = GetOrCreateUniqueDictElementInArray(bundleArr);
            bundleDic[MapsInfo.BundleNameKey] = new PlistElementString(MapsInfo.BundleNameValue);
            var bundleTypeArr = (bundleDic[MapsInfo.BundleTypeKey] ?? (bundleDic[MapsInfo.BundleTypeKey]  = new PlistElementArray())) as PlistElementArray;
            GetOrCreateStringElementInArray(bundleTypeArr, MapsInfo.BundleTypeValue);

            var optionArr = (GetOrCreateInfoDoc().root[MapsInfo.ModeKey] ??
                            (GetOrCreateInfoDoc().root[MapsInfo.ModeKey] = new PlistElementArray())) as PlistElementArray;
            if ((options & MapsOptions.Airplane) == MapsOptions.Airplane)
            {
                GetOrCreateStringElementInArray(optionArr, MapsInfo.ModePlaneValue);
            }
            if ((options & MapsOptions.Bike) == MapsOptions.Bike)
            {
                GetOrCreateStringElementInArray(optionArr, MapsInfo.ModeBikeValue);
            }
            if ((options & MapsOptions.Bus) == MapsOptions.Bus)
            {
                GetOrCreateStringElementInArray(optionArr, MapsInfo.ModeBusValue);
            }
            if ((options & MapsOptions.Car) == MapsOptions.Car)
            {
                GetOrCreateStringElementInArray(optionArr, MapsInfo.ModeCarValue);
            }
            if ((options & MapsOptions.Ferry) == MapsOptions.Ferry)
            {
                GetOrCreateStringElementInArray(optionArr, MapsInfo.ModeFerryValue);
            }
            if ((options & MapsOptions.Other) == MapsOptions.Other)
            {
                GetOrCreateStringElementInArray(optionArr, MapsInfo.ModeOtherValue);
            }
            if ((options & MapsOptions.Pedestrian) == MapsOptions.Pedestrian)
            {
                GetOrCreateStringElementInArray(optionArr, MapsInfo.ModePedestrianValue);
            }
            if ((options & MapsOptions.RideSharing) == MapsOptions.RideSharing)
            {
                GetOrCreateStringElementInArray(optionArr, MapsInfo.ModeRideShareValue);
            }
            if ((options & MapsOptions.StreetCar) == MapsOptions.StreetCar)
            {
                GetOrCreateStringElementInArray(optionArr, MapsInfo.ModeStreetCarValue);
            }
            if ((options & MapsOptions.Subway) == MapsOptions.Subway)
            {
                GetOrCreateStringElementInArray(optionArr, MapsInfo.ModeSubwayValue);
            }
            if ((options & MapsOptions.Taxi) == MapsOptions.Taxi)
            {
                GetOrCreateStringElementInArray(optionArr, MapsInfo.ModeTaxiValue);
            }
            if ((options & MapsOptions.Train) == MapsOptions.Train)
            {
                GetOrCreateStringElementInArray(optionArr, MapsInfo.ModeTrainValue);
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.Maps);
        }

        /// <summary>
        /// Adds Personal VPN capability to the project.
        /// </summary>
        public void AddPersonalVPN()
        {
            var arr = (GetOrCreateEntitlementDoc().root[VPNEntitlements.Key] = new PlistElementArray()) as PlistElementArray;
            arr.values.Add(new PlistElementString(VPNEntitlements.Value));

            project.AddCapability(m_TargetGuid, PBXCapabilityType.PersonalVPN, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds Background capability to the project.
        /// </summary>
        /// <param name="options">The list of background modes to configure.</param>
        public void AddBackgroundModes(BackgroundModesOptions options)
        {
            var optionArr = (GetOrCreateInfoDoc().root[BackgroundInfo.Key] ??
                            (GetOrCreateInfoDoc().root[BackgroundInfo.Key] = new PlistElementArray())) as PlistElementArray;

            if ((options & BackgroundModesOptions.ActsAsABluetoothLEAccessory) == BackgroundModesOptions.ActsAsABluetoothLEAccessory)
            {
                GetOrCreateStringElementInArray(optionArr, BackgroundInfo.ModeActsBluetoothValue);
            }
            if ((options & BackgroundModesOptions.AudioAirplayPiP) == BackgroundModesOptions.AudioAirplayPiP)
            {
                GetOrCreateStringElementInArray(optionArr, BackgroundInfo.ModeAudioValue);
            }
            if ((options & BackgroundModesOptions.BackgroundFetch) == BackgroundModesOptions.BackgroundFetch)
            {
                GetOrCreateStringElementInArray(optionArr, BackgroundInfo.ModeFetchValue);
            }
            if ((options & BackgroundModesOptions.ExternalAccessoryCommunication) == BackgroundModesOptions.ExternalAccessoryCommunication)
            {
                GetOrCreateStringElementInArray(optionArr, BackgroundInfo.ModeExtAccessoryValue);
            }
            if ((options & BackgroundModesOptions.LocationUpdates) == BackgroundModesOptions.LocationUpdates)
            {
                GetOrCreateStringElementInArray(optionArr, BackgroundInfo.ModeLocationValue);
            }
            if ((options & BackgroundModesOptions.NewsstandDownloads) == BackgroundModesOptions.NewsstandDownloads)
            {
                GetOrCreateStringElementInArray(optionArr, BackgroundInfo.ModeNewsstandValue);
            }
            if ((options & BackgroundModesOptions.RemoteNotifications) == BackgroundModesOptions.RemoteNotifications)
            {
                GetOrCreateStringElementInArray(optionArr, BackgroundInfo.ModePushValue);
            }
            if ((options & BackgroundModesOptions.VoiceOverIP) == BackgroundModesOptions.VoiceOverIP)
            {
                GetOrCreateStringElementInArray(optionArr, BackgroundInfo.ModeVOIPValue);
            }
            project.AddCapability(m_TargetGuid, PBXCapabilityType.BackgroundModes);
        }

        /// <summary>
        /// Adds Keychain Sharing capability to the project.
        /// </summary>
        /// <param name="accessGroups">The list of keychain access groups to configure.</param>
        public void AddKeychainSharing(string[] accessGroups)
        {
            var arr = (GetOrCreateEntitlementDoc().root[KeyChainEntitlements.Key] = new PlistElementArray()) as PlistElementArray;
            if (accessGroups != null)
            {
                for (var i = 0; i < accessGroups.Length; i++)
                {
                    arr.values.Add(new PlistElementString(accessGroups[i]));
                }
            }
            else
            {
                arr.values.Add(new PlistElementString(KeyChainEntitlements.DefaultValue));
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.KeychainSharing, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds Inter App Audio capability to the project.
        /// </summary>
        public void AddInterAppAudio()
        {
            GetOrCreateEntitlementDoc().root[AudioEntitlements.Key] = new PlistElementBoolean(true);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.InterAppAudio, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds Associated Domains capability to the project.
        /// </summary>
        /// <param name="domains">The list of domains to configure.</param>
        public void AddAssociatedDomains(string[] domains)
        {
            var arr = (GetOrCreateEntitlementDoc().root[AssociatedDomainsEntitlements.Key] = new PlistElementArray()) as PlistElementArray;
            for (var i = 0; i < domains.Length; i++)
            {
                arr.values.Add(new PlistElementString(domains[i]));
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.AssociatedDomains, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds App Groups capability to the project.
        /// </summary>
        /// <param name="groups">The list of app groups to configure.</param>
        public void AddAppGroups(string[] groups)
        {
            var arr = (GetOrCreateEntitlementDoc().root[AppGroupsEntitlements.Key] = new PlistElementArray()) as PlistElementArray;
            for (var i = 0; i < groups.Length; i++)
            {
                arr.values.Add(new PlistElementString(groups[i]));
            }

            project.AddCapability(m_TargetGuid, PBXCapabilityType.AppGroups, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds HomeKit capability to the project.
        /// </summary>
        public void AddHomeKit()
        {
            GetOrCreateEntitlementDoc().root[HomeKitEntitlements.Key] = new PlistElementBoolean(true);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.HomeKit, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds Data Protection capability to the project.
        /// </summary>
        public void AddDataProtection()
        {
            GetOrCreateEntitlementDoc().root[DataProtectionEntitlements.Key] = new PlistElementString(DataProtectionEntitlements.Value);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.DataProtection, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds HealthKit capability to the project.
        /// </summary>
        public void AddHealthKit()
        {
            var capabilityArr = (GetOrCreateInfoDoc().root[HealthInfo.Key] ??
                                (GetOrCreateInfoDoc().root[HealthInfo.Key] = new PlistElementArray())) as PlistElementArray;
            GetOrCreateStringElementInArray(capabilityArr, HealthInfo.Value);
            GetOrCreateEntitlementDoc().root[HealthKitEntitlements.Key] = new PlistElementBoolean(true);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.HealthKit, m_EntitlementFilePath);
        }

        /// <summary>
        /// Adds Wireless Accessory Configuration capability to the project.
        /// </summary>
        public void AddWirelessAccessoryConfiguration()
        {
            GetOrCreateEntitlementDoc().root[WirelessAccessoryConfigurationEntitlements.Key] = new PlistElementBoolean(true);
            project.AddCapability(m_TargetGuid, PBXCapabilityType.WirelessAccessoryConfiguration, m_EntitlementFilePath);
        }

        private PlistDocument GetOrCreateEntitlementDoc()
        {
            if (m_Entitlements == null)
            {
                m_Entitlements = new PlistDocument();
                string[] entitlementsFiles = Directory.GetFiles(m_BuildPath, m_EntitlementFilePath);
                if (entitlementsFiles.Length > 0)
                {
                    m_Entitlements.ReadFromFile(entitlementsFiles[0]);
                }
                else
                {
                    m_Entitlements.Create();
                }
            }

            return m_Entitlements;
        }

        private PlistDocument GetOrCreateInfoDoc()
        {
            if (m_InfoPlist == null)
            {
                m_InfoPlist = new PlistDocument();
                string[] infoFiles = Directory.GetFiles(m_BuildPath + "/", "Info.plist");
                if (infoFiles.Length > 0)
                {
                    m_InfoPlist.ReadFromFile(infoFiles[0]);
                }
                else
                {
                    m_InfoPlist.Create();
                }
            }

            return m_InfoPlist;
        }

        private PlistElementString GetOrCreateStringElementInArray(PlistElementArray root, string value)
        {
            PlistElementString r = null;
            var c = root.values.Count;
            var exist = false;
            for (var i = 0; i < c; i++)
            {
                if (root.values[i] is PlistElementString && (root.values[i] as PlistElementString).value == value)
                {
                    r = root.values[i] as PlistElementString;
                    exist = true;
                }
            }
            if (!exist)
            {
                r = new PlistElementString(value);
                root.values.Add(r);
            }
            return r;
        }

        private PlistElementDict GetOrCreateUniqueDictElementInArray(PlistElementArray root)
        {
            PlistElementDict r;
            if (root.values.Count == 0)
            {
                r = root.values[0] as PlistElementDict;
            }
            else
            {
                r = new PlistElementDict();
                root.values.Add(r);
            }
            return r;
        }
    }

    // The list of options available for Background Mode.
    [Flags]
    [Serializable]
    public enum BackgroundModesOptions
    {
        None                           = 0,
        AudioAirplayPiP                = 1<<0,
        LocationUpdates                = 1<<1,
        VoiceOverIP                    = 1<<2,
        NewsstandDownloads             = 1<<3,
        ExternalAccessoryCommunication = 1<<4,
        UsesBluetoothLEAccessory       = 1<<5,
        ActsAsABluetoothLEAccessory    = 1<<6,
        BackgroundFetch                = 1<<7,
        RemoteNotifications            = 1<<8
    }

    // The list of options available for Maps.
    [Serializable]
    [Flags]
    public enum MapsOptions
    {
        None          = 0,
        Airplane      = 1<<0,
        Bike          = 1<<1,
        Bus           = 1<<2,
        Car           = 1<<3,
        Ferry         = 1<<4,
        Pedestrian    = 1<<5,
        RideSharing   = 1<<6,
        StreetCar     = 1<<7,
        Subway        = 1<<8,
        Taxi          = 1<<9,
        Train         = 1<<10,
        Other         = 1<<11
    }

    /* Follows the large quantity of string used as key and value all over the place in the info.plist or entitlements file. */
    internal class GameCenterInfo
    {
        internal static readonly string Key = "UIRequiredDeviceCapabilities";
        internal static readonly string Value = "gamekit";
    }
    internal class ApplicationQueriesSchemesInfo
    {
        internal static readonly string Key = "LSApplicationQueriesSchemes";
    }
    internal class UrlTypeInfo
    {
        internal static readonly string URLTYPE = "CFBundleURLTypes";

        internal static readonly string TYPEROLE = "CFBundleTypeRole";
        internal static readonly string URLNAME = "CFBundleURLName";
        internal static readonly string URLSCHEMES = "CFBundleURLSchemes";
    }
    /// <summary>
	/// 权限配置
	/// </summary>
    internal class XcodePermissionsInfo
    {
        internal static readonly string CameraUsage = "NSCameraUsageDescription";
        internal static readonly string ContactsUsage = "NSContactsUsageDescription";
    }

    internal class MapsInfo
    {
        internal static readonly string BundleKey = "CFBundleDocumentTypes";
        internal static readonly string BundleNameKey = "CFBundleTypeName";
        internal static readonly string BundleNameValue = "MKDirectionsRequest";
        internal static readonly string BundleTypeKey = "LSItemContentTypes";
        internal static readonly string BundleTypeValue = "com.apple.maps.directionsrequest";
        internal static readonly string ModeKey = "MKDirectionsApplicationSupportedModes";
        internal static readonly string ModePlaneValue = "MKDirectionsModePlane";
        internal static readonly string ModeBikeValue = "MKDirectionsModeBike";
        internal static readonly string ModeBusValue = "MKDirectionsModeBus";
        internal static readonly string ModeCarValue = "MKDirectionsModeCar";
        internal static readonly string ModeFerryValue = "MKDirectionsModeFerry";
        internal static readonly string ModeOtherValue = "MKDirectionsModeOther";
        internal static readonly string ModePedestrianValue = "MKDirectionsModePedestrian";
        internal static readonly string ModeRideShareValue = "MKDirectionsModeRideShare";
        internal static readonly string ModeStreetCarValue = "MKDirectionsModeStreetCar";
        internal static readonly string ModeSubwayValue = "MKDirectionsModeSubway";
        internal static readonly string ModeTaxiValue = "MKDirectionsModeTaxi";
        internal static readonly string ModeTrainValue = "MKDirectionsModeTrain";
    }

    internal class BackgroundInfo
    {
        internal static readonly string Key = "UIBackgroundModes";
        internal static readonly string ModeAudioValue = "audio";
        internal static readonly string ModeBluetoothValue = "bluetooth-central";
        internal static readonly string ModeActsBluetoothValue = "bluetooth-peripheral";
        internal static readonly string ModeExtAccessoryValue = "external-accessory";
        internal static readonly string ModeFetchValue = "fetch";
        internal static readonly string ModeLocationValue = "location";
        internal static readonly string ModeNewsstandValue = "newsstand-content";
        internal static readonly string ModePushValue = "remote-notification";
        internal static readonly string ModeVOIPValue = "voip";
    }

    internal class HealthInfo
    {
        internal static readonly string Key = "UIRequiredDeviceCapabilities";
        internal static readonly string Value = "healthkit";
    }

    internal class ICloudEntitlements
    {
        internal static readonly string ContainerIdKey = "com.apple.developer.icloud-container-identifiers";
        internal static readonly string ContainerIdValue = "iCloud.$(CFBundleIdentifier)";
        internal static readonly string UbiquityContainerIdKey = "com.apple.developer.ubiquity-container-identifiers";
        internal static readonly string UbiquityContainerIdValue = "iCloud.$(CFBundleIdentifier)";
        internal static readonly string ServicesKey = "com.apple.developer.icloud-services";
        internal static readonly string ServicesDocValue = "CloudDocuments";
        internal static readonly string ServicesKitValue = "CloudKit";
        internal static readonly string KeyValueStoreKey = "com.apple.developer.ubiquity-kvstore-identifier";
        internal static readonly string KeyValueStoreValue = "$(TeamIdentifierPrefix)$(CFBundleIdentifier)";
    }

    internal class PushNotificationEntitlements
    {
        internal static readonly string Key = "aps-environment";
        internal static readonly string DevelopmentValue = "development";
        internal static readonly string ProductionValue = "production";
    }

    internal class WalletEntitlements
    {
        internal static readonly string Key = "com.apple.developer.pass-type-identifiers";
        internal static readonly string BaseValue = "$(TeamIdentifierPrefix)";
        internal static readonly string DefaultValue = "*";
    }

    internal class SiriEntitlements
    {
        internal static readonly string Key = "com.apple.developer.siri";
    }

    internal class ApplePayEntitlements
    {
        internal static readonly string Key = "com.apple.developer.in-app-payments";
    }

    internal class VPNEntitlements
    {
        internal static readonly string Key = "com.apple.developer.networking.vpn.api";
        internal static readonly string Value = "allow-vpn";
    }

    internal class KeyChainEntitlements
    {
        internal static readonly string Key = "keychain-access-groups";
        internal static readonly string DefaultValue = "$(AppIdentifierPrefix)$(CFBundleIdentifier)";
    }

    internal class SingInWithApplentitlements
    {
        internal static readonly string Key = "com.apple.developer.applesignin";
        internal static readonly string DefaultValue = "Default";
    }

    internal class AudioEntitlements
    {
        internal static readonly string Key = "inter-app-audio";
    }

    internal class AssociatedDomainsEntitlements
    {
        // value is an array of string of domains
        internal static readonly string Key = "com.apple.developer.associated-domains";
    }

    internal class AppGroupsEntitlements
    {
        // value is an array of string of groups
        internal static readonly string Key = "com.apple.security.application-groups";
    }

    internal class HomeKitEntitlements
    {
        // value is bool true.
        internal static readonly string Key = "com.apple.developer.homekit";
    }

    internal class DataProtectionEntitlements
    {
        internal static readonly string Key = "com.apple.developer.default-data-protection";
        internal static readonly string Value = "NSFileProtectionComplete";
    }

    internal class HealthKitEntitlements
    {
        // value is bool true.
        internal static readonly string Key = "com.apple.developer.healthkit";
    }

    internal class WirelessAccessoryConfigurationEntitlements
    {
        // value is bool true.
        internal static readonly string Key = "com.apple.external-accessory.wireless-configuration";
    }
}
