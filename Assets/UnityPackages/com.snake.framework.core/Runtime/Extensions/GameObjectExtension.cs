using System;
using UnityEngine;

static public class GameObjectExtensions
{
    static public GameObject Clone(this GameObject target, Transform parent)
    {
        GameObject go = GameObject.Instantiate(target, parent);
        go.name = go.name.Replace("(Clone)", string.Empty);
        return go;
    }

    static public void SetLayerByName(this GameObject targetGameObject, string layerName, bool isIncludeChildren = false)
    {
        int layer = LayerMask.NameToLayer(layerName);

        if (isIncludeChildren)
        {
            SetLayerRecursive(targetGameObject.transform, layer);
        }
        else
        {
            targetGameObject.layer = layer;
        }
    }

    public static void SetLayer(this GameObject targetGameObject, int layer, bool isIncludeChildren = true)
    {
        if (isIncludeChildren)
        {
            SetLayerRecursive(targetGameObject.transform, layer);
        }
        else
        {
            targetGameObject.layer = layer;
        }
    }

    /// <summary>
    /// 递归设置层级 //
    /// </summary>
    private static void SetLayerRecursive(Transform targetTransform, int layer)
    {
        targetTransform.gameObject.layer = layer;

        // 子节点递归遍历 //
        for (int i = 0; i < targetTransform.childCount; i++)
        {
            SetLayerRecursive(targetTransform.GetChild(i), layer);
        }
    }

    public static void SetActiveExtra(this GameObject targetGameObject, bool isActive)
    {
        if (targetGameObject.activeSelf != isActive)
        {
            targetGameObject.SetActive(isActive);
        }
    }

    /// <summary>
    /// 获取组件但没有的时候添加进去
    /// </summary>
    /// <param name="targetGameObject"></param>
    /// <param name="componentType"></param>
    /// <returns></returns>
    public static Component GetComponentOrAdd(this GameObject targetGameObject, Type componentType)
    {
        Component tComponent = targetGameObject.GetComponent(componentType);
        if (tComponent == null)
            tComponent = targetGameObject.AddComponent(componentType);
        return tComponent;
    }

    public static void SetGameObjectActive(this GameObject pTargetGameObject, bool pIsActive)
    {
        if (pTargetGameObject.activeSelf != pIsActive)
        {
            pTargetGameObject.SetActive(pIsActive);
        }
    }




}
