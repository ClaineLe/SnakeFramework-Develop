using UnityEngine;

namespace com.snake.framework
{
    namespace runtime
    {
        using tool;
        public class BootDriver
        {
            [RuntimeInitializeOnLoadMethod]
            static public void StartUp()
            {
                Singleton<AppFacade>.CreateInstance();
            }
        }
    }
}