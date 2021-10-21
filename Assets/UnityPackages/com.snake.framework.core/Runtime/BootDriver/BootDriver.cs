using UnityEngine.SceneManagement;

namespace com.snake.framework
{
    namespace runtime
    {
        internal class BootDriver
        {
            public ISnakeFrameworkExt mSnakeFrameworkExt { get; private set; }

            [UnityEngine.RuntimeInitializeOnLoadMethod]
            static public void BootUp()
            {
                BootDriverSetting bootDriverSetting = UnityEngine.Resources.Load<BootDriverSetting>(typeof(BootDriverSetting).Name);
                if (bootDriverSetting == null)
                {
                    SnakeDebuger.ErrorFormat("û����ResourcesĿ¼���ҵ�BootDriverSetting.asset�����Ҽ�����һ��");
                    return;
                }

                if (bootDriverSetting.Active == false)
                {
                    SnakeDebuger.ErrorFormat("���δ���bootDriverSetting.Active == false");
                    return;
                }

                if (string.IsNullOrEmpty(bootDriverSetting.BootUpTagName) == true)
                {
                    SnakeDebuger.ErrorFormat("�������Tag����Ϊ�ա�bootDriverSetting.BootUpTagName��" + bootDriverSetting.BootUpTagName);
                    return;
                }

                try
                {
                    if (UnityEngine.GameObject.FindWithTag(bootDriverSetting.BootUpTagName) == null)
                    {
                        SnakeDebuger.ErrorFormat("û���ҵ�Tag���Ϊ��{0} ��GameObject���󣬲�����Ϸ��������", bootDriverSetting.BootUpTagName);
                        return;
                    }
                }
                catch (UnityEngine.UnityException unityEx)
                {
                    SnakeDebuger.Error("û���ҵ�����Tag:" + bootDriverSetting.BootUpTagName + ".(" + unityEx.Message + ")");
                    return;
                }


                BootDriver bootDriver = new BootDriver(bootDriverSetting);
                SnakeFramework.Instance.StartUp(bootDriver.mSnakeFrameworkExt);
            }
            private BootDriver(BootDriverSetting bootDriverSetting)
            {
                System.Type type = default;
                System.Reflection.Assembly[] s_Assemblies = Utility.Assembly.GetAssemblies();
                foreach (System.Reflection.Assembly assembly in s_Assemblies)
                {
                    if (assembly.GetName().Name.Equals(bootDriverSetting.RuntimeAssemblyName))
                    {
                        type = assembly.GetType(bootDriverSetting.FrameworkExtTypeFullName);
                        break;
                    }
                }

                if (type == null)
                    throw new System.Exception("û���ҵ�Ӧ���Ż����Զ���ʵ������(IAppFacadeCostom)��");

                object appFacadeCostomObj = System.Activator.CreateInstance(type);
                if (appFacadeCostomObj == null)
                    throw new System.Exception("û���ҵ�Ӧ���Ż����Զ���ʵ�ֶ���(IAppFacadeCostom)��");
                this.mSnakeFrameworkExt = appFacadeCostomObj as ISnakeFrameworkExt;
            }
        }
    }
}