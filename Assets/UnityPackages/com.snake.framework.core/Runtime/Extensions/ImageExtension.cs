using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ImageExtensions
{
    /// <summary>
    /// 根据Sprite源Size进行平铺适配
    /// </summary>
    /// <param name="pAspectMode"></param>
    static public void AspectRatio(this Image img, AspectRatioFitter.AspectMode pAspectMode = AspectRatioFitter.AspectMode.EnvelopeParent)
    {
        if (img.sprite == null)
            return;

        AspectRatioFitter aspectRatioFitter = img.GetComponent<AspectRatioFitter>();
        if (aspectRatioFitter == null)
            aspectRatioFitter = img.gameObject.AddComponent<AspectRatioFitter>();

        aspectRatioFitter.aspectMode = pAspectMode;
        aspectRatioFitter.aspectRatio = img.sprite.rect.width / img.sprite.rect.height;
    }
}
