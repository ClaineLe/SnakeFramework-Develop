using UnityEngine;

namespace com.snake.framework
{
    namespace runtime
    {
        public class BootDriver : MonoBehaviour
        {
            public const string RUNTIME_ASSEMBLY = "Assembly-CSharp";
            public const string CUSTOM_NAMESPACE = "com.snake.framework.custom.runtime";
            public string CustomAppFacadeClassName = "AppFacadeCostom";
            public IAppFacadeCostom mAppFacadeCostom { get; private set; }
            private void Awake()
            {
                mAppFacadeCostom = _CreateCostomAppFacade();
            }

            private void Start()
            {
                Singleton<AppFacade>.GetInstance().StartUp(mAppFacadeCostom);
            }

            private IAppFacadeCostom _CreateCostomAppFacade() 
            {
                System.Type type = default;
                System.Reflection.Assembly[] s_Assemblies = Utility.Assembly.GetAssemblies();
                foreach (System.Reflection.Assembly assembly in s_Assemblies)
                {
                    if (assembly.GetName().Name.Equals(RUNTIME_ASSEMBLY))
                    {
                        type = assembly.GetType(CUSTOM_NAMESPACE + "." + CustomAppFacadeClassName);
                        break;
                    }
                }

                if (type == null)
                    throw new System.Exception("û���ҵ�Ӧ���Ż����Զ���ʵ������(IAppFacadeCostom)��");

                object appFacadeCostomObj = System.Activator.CreateInstance(type);
                if (appFacadeCostomObj == null)
                    throw new System.Exception("û���ҵ�Ӧ���Ż����Զ���ʵ�ֶ���(IAppFacadeCostom)��");
                return appFacadeCostomObj as IAppFacadeCostom;
            }
        }
    }
}