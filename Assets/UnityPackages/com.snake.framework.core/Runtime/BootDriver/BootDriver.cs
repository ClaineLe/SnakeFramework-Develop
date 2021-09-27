using UnityEngine.SceneManagement;

namespace com.snake.framework
{
    namespace runtime
    {
        public class BootDriver
        {
            public IAppFacadeCostom mAppFacadeCostom { get; private set; }

            [UnityEngine.RuntimeInitializeOnLoadMethod]
            static public void BootUp()
            {
                BootDriverSetting bootDriverSetting = UnityEngine.Resources.Load<BootDriverSetting>(typeof(BootDriverSetting).Name);
                if (bootDriverSetting == null)
                {
                    Debuger.ErrorFormat("û����ResourcesĿ¼���ҵ�BootDriverSetting.asset�����Ҽ�����һ��");
                    return;
                }

                if (bootDriverSetting.Active == false)
                {
                    Debuger.ErrorFormat("���δ���bootDriverSetting.Active == false");
                    return;
                }

                if (string.IsNullOrEmpty(bootDriverSetting.BootUpTagName) == true)
                {
                    Debuger.ErrorFormat("�������Tag����Ϊ�ա�bootDriverSetting.BootUpTagName��" + bootDriverSetting.BootUpTagName);
                    return;
                }

                try
                {
                    if (UnityEngine.GameObject.FindWithTag(bootDriverSetting.BootUpTagName) == null)
                    {
                        Debuger.ErrorFormat("û���ҵ�Tag���Ϊ��{0} ��GameObject���󣬲�����Ϸ��������", bootDriverSetting.BootUpTagName);
                        return;
                    }
                }
                catch (UnityEngine.UnityException unityEx)
                {
                    Debuger.Error("û���ҵ�����Tag:" + bootDriverSetting.BootUpTagName + ".(" + unityEx.Message + ")");
                    return;
                }


                BootDriver bootDriver = new BootDriver(bootDriverSetting);
                Singleton<AppFacade>.GetInstance().StartUp(bootDriver.mAppFacadeCostom);
            }
            private BootDriver(BootDriverSetting bootDriverSetting)
            {
                System.Type type = default;
                System.Reflection.Assembly[] s_Assemblies = Utility.Assembly.GetAssemblies();
                foreach (System.Reflection.Assembly assembly in s_Assemblies)
                {
                    if (assembly.GetName().Name.Equals(bootDriverSetting.RuntimeAssemblyName))
                    {
                        type = assembly.GetType(bootDriverSetting.CustomAppFacadeTypeFullName);
                        break;
                    }
                }

                if (type == null)
                    throw new System.Exception("û���ҵ�Ӧ���Ż����Զ���ʵ������(IAppFacadeCostom)��");

                object appFacadeCostomObj = System.Activator.CreateInstance(type);
                if (appFacadeCostomObj == null)
                    throw new System.Exception("û���ҵ�Ӧ���Ż����Զ���ʵ�ֶ���(IAppFacadeCostom)��");
                this.mAppFacadeCostom = appFacadeCostomObj as IAppFacadeCostom;
            }
        }
    }
}