using UnityEngine;

namespace com.snake.framework
{
    namespace runtime
    {
        public abstract class SnakeBasicSetting<T> : UnityEngine.ScriptableObject
            where T : UnityEngine.Object
        {
            static public T Get()
            {
                T setting = Resources.Load<T>(typeof(T).Name);
                if (setting == null)
                {
                    SnakeDebuger.ErrorFormat("没有在Resources目录下找到{0}.asset，请右键创建一个", typeof(T).Name);
                    return null;
                }
                return setting;
            }
        }
    }
}