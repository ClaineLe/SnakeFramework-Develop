using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode.Custom;

public class XCodeApi
{

    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        
        if (buildTarget == BuildTarget.iOS)
        {
            // 只处理IOS工程， pathToBuildProject会传入导出的ios工程的根目录
            // 创建工程设置对象
            var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            //初始化
            var capManager = new ProjectCapabilityManager(projectPath, "hero.entitlements", PBXProject.GetUnityTargetName(),pathToBuiltProject);

            ////添加推送
            //capManager.AddPushNotifications(true);
            ////添加内购
            //capManager.AddInAppPurchase();
            ////添加钥匙串
            ////capManager.AddKeychainSharing(new string[] { "com.DefaultCompany.NewUnityProject6" });
            ////添加link
            ////capManager.AddAssociatedDomains(new string[] { "applinks:other.yingxiong.com" });
            ////添加苹果登录
            ////capManager.AddSignInWithApple(new string[] { "Default" });

            capManager.WriteToFile();
        }
    }
}
