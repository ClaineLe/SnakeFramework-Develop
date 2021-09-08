using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace com.snake.framework
{
    public static class TransformExtensions
    {
        public static bool IsNull(this Component comp)
        {
            return comp == null || comp.Equals(null);
        }

        public static void SetActive(this Transform transform, bool active)
        {
            //transform.DOScale
            transform.gameObject.SetActive(active);
        }

        public static void HideChild(this Transform transform)
        {
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject obj = transform.GetChild(i).gameObject;
                obj.SetActive(false);
            }
        }
        //坐标
        static public void SetLocalPosition(this Transform transform, Vector3 localPos)
        {
            transform.localPosition = localPos;
        }

        static public void SetLocalPosition(this Transform transform, float x, float y, float z)
        {
            transform.localPosition = new Vector3(x, y, z);
        }

        static public void SetLocalPositionX(this Transform transform, float x)
        {
            Vector3 tmpPosition = transform.localPosition;
            tmpPosition.x = x;
            transform.localPosition = tmpPosition;
        }

        static public void SetLocalPositionY(this Transform transform, float y)
        {
            Vector3 tmpPosition = transform.localPosition;
            tmpPosition.y = y;
            transform.localPosition = tmpPosition;

        }
        static public void SetLocalPositionZ(this Transform transform, float z)
        {
            Vector3 tmpPosition = transform.localPosition;
            tmpPosition.z = z;
            transform.localPosition = tmpPosition;
        }
        static public void SetPosition(this Transform transform, Vector3 localPos)
        {
            transform.position = localPos;
        }

        static public void SetPosition(this Transform transform, float x, float y, float z)
        {
            transform.position = new Vector3(x, y, z);
        }

        static public void SetpositionX(this Transform transform, float x)
        {
            Vector3 tmpPosition = transform.position;
            tmpPosition.x = x;
            transform.position = tmpPosition;
        }

        static public void SetPositionY(this Transform transform, float y)
        {
            Vector3 tmpPosition = transform.position;
            tmpPosition.y = y;
            transform.position = tmpPosition;
        }
        static public void SetPositionZ(this Transform transform, float z)
        {
            Vector3 tmpPosition = transform.position;
            tmpPosition.z = z;
            transform.position = tmpPosition;
        }
        //大小
        static public void SetLocalScale(this Transform transform, Vector3 localSc)
        {
            transform.localScale = localSc;
        }
        static public void SetLocalScale(this Transform transform, float x, float y, float z)
        {
            transform.localScale = new Vector3(x, y, z);
        }
        static public void SetLocalScaleX(this Transform transform, float x)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = x;
            transform.localScale = localScale;
        }
        static public void SetLocalScaleY(this Transform transform, float y)
        {
            Vector3 localScale = transform.localScale;
            localScale.y = y;
            transform.localScale = localScale;
        }
        static public void SetLocalScaleZ(this Transform transform, float z)
        {
            Vector3 localScale = transform.localScale;
            localScale.z = z;
            transform.localScale = localScale;
        }
        //旋转
        static public void SetEulerAngles(this Transform transform, Vector3 eulerAngles)
        {
            transform.eulerAngles = eulerAngles;
        }
        static public void SetEulerAngles(this Transform transform, float x, float y, float z)
        {
            transform.eulerAngles = new Vector3(x, y, z);
        }
        static public void SetEulerAnglesX(this Transform transform, float x)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.x = x;
            transform.eulerAngles = eulerAngles;
        }
        static public void SetEulerAnglesY(this Transform transform, float y)
        {

            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y = y;
            transform.eulerAngles = eulerAngles;
        }
        static public void SetEulerAnglesZ(this Transform transform, float z)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = z;
            transform.eulerAngles = eulerAngles;
        }
        static public void SetLocalEulerAngles(this Transform transform, Vector3 eulerAngles)
        {
            transform.localEulerAngles = eulerAngles;
        }
        static public void SetLocalEulerAngles(this Transform transform, float x, float y, float z)
        {
            transform.localEulerAngles = new Vector3(x, y, z);
        }
        static public void SetLocalEulerAnglesX(this Transform transform, float x)
        {
            Vector3 eulerAngles = transform.localEulerAngles;
            eulerAngles.x = x;
            transform.localEulerAngles = eulerAngles;
        }
        static public void SetLocalEulerAnglesY(this Transform transform, float y)
        {

            Vector3 eulerAngles = transform.localEulerAngles;
            eulerAngles.y = y;
            transform.localEulerAngles = eulerAngles;
        }
        static public void SetLocalEulerAnglesZ(this Transform transform, float z)
        {
            Vector3 eulerAngles = transform.localEulerAngles;
            eulerAngles.z = z;
            transform.localEulerAngles = eulerAngles;
        }
    }
}