using UnityEngine;
namespace com.snake.framework
{
    static public class ObjectExtensions
    {
        static bool IsNull(this Object obj)
        {
            return obj == null || obj.Equals(null);
        }

        public static Object Clone(this Object target, Transform parent)
        {
            Object gameobject = GameObject.Instantiate(target, parent);
            gameobject.name = gameobject.name.Replace("(Clone)", string.Empty);
            return gameobject;
        }

    }
}