using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using com.snake.framework.runtime;

namespace com.snake.framework
{
    namespace editor
    {
        public class SnakeBuildBundleOptions
        {
            public BundleBuildParameters mParameters { get; private set; }
            public string mAssetBundleOutputPath { get; private set; }
            public string mExtOutPutPath { get; private set; }
            public List<string> mExtSourceBuneleNameList { get; private set; }
            public SnakeBuildBundleOptions(string assetBundleOutputPath, BuildTarget buildTarget)
            {
                switch (buildTarget)
                {
                    case BuildTarget.Android:
                        {
                            mParameters = new BundleBuildParameters(buildTarget, BuildTargetGroup.Android, assetBundleOutputPath);
                            break;
                        }
                    case BuildTarget.iOS:
                        {
                            mParameters = new BundleBuildParameters(buildTarget, BuildTargetGroup.iOS, assetBundleOutputPath);
                            break;
                        }
                    default:
                        {
                            SnakeDebuger.Error("目前只支持Android和iOS两个平台的资源打包");
                            return;
                        }
                }
                this.mAssetBundleOutputPath = assetBundleOutputPath;
                this.mExtSourceBuneleNameList = new List<string>();
            }

            public bool IsProcessExtBundle()
            {
                if (string.IsNullOrEmpty(mExtOutPutPath) == true)
                    return false;

                if (mExtSourceBuneleNameList == null)
                    return false;

                if (mExtSourceBuneleNameList.Count == 0)
                    return false;

                return true;
            }

            public void SetExtBundleNamePath(string extOutPutPath, string extBundleNameFilePath)
            {
                this.mExtOutPutPath = extOutPutPath;

                if (string.IsNullOrEmpty(extBundleNameFilePath))
                {
                    SnakeDebuger.Error("没有找到拓张Bundle列表文件. path:" + extBundleNameFilePath);
                    return;
                }
                string[] bundleNems = System.IO.File.ReadAllLines(extBundleNameFilePath);
                foreach (var bundleName in bundleNems)
                {
                    if (string.IsNullOrEmpty(bundleName) == true)
                        continue;
                    this.mExtSourceBuneleNameList.Add(bundleName);
                }
            }

        }
    }
}