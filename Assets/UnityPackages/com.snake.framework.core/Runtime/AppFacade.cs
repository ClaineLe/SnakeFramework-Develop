using com.halo.framework.common;
using UnityEngine;

namespace com.snake.framework
{
    namespace runtime
    {
        using System.Reflection;
        using tool;
        public class AppFacade : Singleton<AppFacade>, ISingleton
        {
            public override void Initialize()
            {
                base.Initialize();
                foreach (Assembly assembly in Utility.Assembly.GetAssemblies())
                {
                    if (assembly.FullName.StartsWith("Assembly-CSharp") == true)
                    {
                        foreach (var a in assembly.GetTypes())
                        {
                            Debug.Log(a);
                        }
                    }
                }
            }
        }
    }
}
