using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Transition_Manager : MonoBehaviour
{
    //Singletion Pattern
    private static Transition_Manager _i;

    public Image maskImage;

    public static Transition_Manager i
    {
        get
        {
            return _i;
        }
    }

    private void Awake()
    {
        if (_i == null)
        {
            _i = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionIn()
    {
        maskImage.rectTransform.sizeDelta = Vector2.zero;
        DOTween.To(() => maskImage.rectTransform.sizeDelta, x => maskImage.rectTransform.sizeDelta = x, Vector2.one * 5000, 1).SetEase(Ease.InExpo);
    }
}
