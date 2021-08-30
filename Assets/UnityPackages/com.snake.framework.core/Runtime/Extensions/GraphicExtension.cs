using System;
using UnityEngine;
using UnityEngine.UI;

static public class GraphicExtensions
{
    static public void SetGrey(this Graphic targetGraphic, bool gray = true, bool isInChild = false)
    {
        if (gray)
        {
            targetGraphic.material = Resources.Load<Material>("UIGreyMaterial");
            if (isInChild) 
            {
                Image[] _imgs = targetGraphic.gameObject.GetComponentsInChildren<Image>();
                Text[] _texts = targetGraphic.gameObject.GetComponentsInChildren<Text>();
                for (int i = 0; i < _imgs.Length; i++)
                {
                    if (_imgs[i] != targetGraphic)
                        _imgs[i].SetGrey(gray, isInChild);
                }

                for (int i = 0; i < _texts.Length; i++)
                {
                    if (_texts[i] != targetGraphic)
                        _texts[i].SetGrey(gray, isInChild);
                }
            }
        }
        else
        {

            //targetGraphic.material = null;
            targetGraphic.material = Resources.Load<Material>("UIDefaultMaterial");
            if (isInChild)
            {
                Image[] _imgs = targetGraphic.gameObject.GetComponentsInChildren<Image>();
                Text[] _texts = targetGraphic.gameObject.GetComponentsInChildren<Text>();
                for (int i = 0; i < _imgs.Length; i++)
                {
                    if (_imgs[i] != targetGraphic)
                        _imgs[i].SetGrey(gray, isInChild);
                }

                for (int i = 0; i < _texts.Length; i++)
                {
                    if (_texts[i] != targetGraphic)
                        _texts[i].SetGrey(gray, isInChild);
                }
            }
        }
    }
}
