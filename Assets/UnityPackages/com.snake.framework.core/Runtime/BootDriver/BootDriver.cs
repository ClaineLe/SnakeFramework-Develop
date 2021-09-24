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
                    Debuger.ErrorFormat("没有在Resources目录下找到BootDriverSetting.asset，请右键创建一个");
                    return;
                }
                if (bootDriverSetting.Active == false)
                    return;

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
                    throw new System.Exception("没有找到应用门户的自定义实现类型(IAppFacadeCostom)。");

                object appFacadeCostomObj = System.Activator.CreateInstance(type);
                if (appFacadeCostomObj == null)
                    throw new System.Exception("没有找到应用门户的自定义实现对象(IAppFacadeCostom)。");
                this.mAppFacadeCostom = appFacadeCostomObj as IAppFacadeCostom;
            }
        }
    }
}