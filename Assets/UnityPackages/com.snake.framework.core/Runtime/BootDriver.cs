using UnityEngine;

namespace com.snake.framework
{
    namespace runtime
    {
        public class BootDriver
        {
            [RuntimeInitializeOnLoadMethod]
            static public void StartUp()
            {
                Debug.Log("StartUp");
            }
        }
    }
}