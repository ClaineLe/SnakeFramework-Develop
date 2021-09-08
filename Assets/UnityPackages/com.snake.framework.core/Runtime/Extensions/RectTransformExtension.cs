using UnityEngine;
namespace com.snake.framework
{
    static public class RectTransformExtension
    {
        /// <summary>
        /// 设置默认RectTransform数值
        /// </summary>
        /// <param name="rectTransform"></param>
        static public void Identity(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;

            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.sizeDelta = Vector2.zero;

            rectTransform.localScale = Vector3.one;
        }

        static public void SetAnchoredPosition(this RectTransform rectTransform, Vector2 anchoredPos)
        {
            rectTransform.anchoredPosition = anchoredPos;
        }

        static public void SetAnchoredPosition(this RectTransform rectTransform, float x, float y)
        {
            rectTransform.anchoredPosition = new Vector2(x, y);
        }

        static public void SetAnchoredPositionX(this RectTransform rectTransform, float x)
        {
            rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
        }

        static public void SetAnchoredPositionY(this RectTransform rectTransform, float y)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
        }

    }
}