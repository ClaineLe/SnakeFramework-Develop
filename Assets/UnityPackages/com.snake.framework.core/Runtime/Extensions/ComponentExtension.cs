using UnityEngine;
public static class ComponentExtensions
{
    /// <summary>
    /// 获取子物体控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pTarget"></param>
    /// <param name="pPath"></param>
    /// <returns></returns>
    public static T GetChildComponent<T>(this Component pTarget, string pPath) where T : Component
    {
        return pTarget.GetChildComponent(pPath, typeof(T)) as T;
    }

    /// <summary>
    /// 获取子物体控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pTarget"></param>
    /// <param name="pPath"></param>
    /// <param name="pComponentType"></param>
    /// <returns></returns>
    public static Component GetChildComponent(this Component pTarget, string pPath, System.Type pComponentType)
    {
        Transform child = pTarget.transform.Find(pPath);
        if (child == null)
        {
            Debug.LogError("找不到子物理.parent:" + pTarget + ", child is null");
            return null;
        }
        return child.GetComponent(pComponentType);
    }
}
