using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Transition_Manager : MonoBehaviour
{
    //Singletion Pattern
    private static Transition_Manager _i;

    public Canvas canvas;
    public Image maskImage;
    public Image transiFlatImage;

    private int maxScale = 7500;
    private Tween transiTween;

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
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator TransitionIn(float duration = 1, float exitTimes = .5f)
    {
        maskImage.rectTransform.anchoredPosition = Vector2.zero;
        transiFlatImage.rectTransform.anchoredPosition = Vector2.zero;

        maskImage.rectTransform.sizeDelta = Vector2.zero;
        yield return TransitionAnim(maxScale, duration, Ease.InQuart, exitTimes: exitTimes);
    }

    public IEnumerator TransitionIn(Vector2 worldPos, float duration = 1, float exitTimes = .5f)
    {
        Vector2 anchoredPosition = canvas.WorldToCanvasPosition(worldPos, Camera_Manager.i.cameraObj);

        maskImage.rectTransform.anchoredPosition = anchoredPosition;
        transiFlatImage.rectTransform.anchoredPosition = -anchoredPosition;

        maskImage.rectTransform.sizeDelta = Vector2.zero;
        yield return TransitionAnim(maxScale, duration, Ease.InQuart, exitTimes: exitTimes);
    }

    public IEnumerator TransitionOut(float duration = 2, float exitTimes = .5f)
    {
        maskImage.rectTransform.anchoredPosition = Vector2.zero;
        transiFlatImage.rectTransform.anchoredPosition = Vector2.zero;

        maskImage.rectTransform.sizeDelta = Vector2.one * maxScale;
        yield return TransitionAnim(0, duration, exitTimes: exitTimes);
    }

    public IEnumerator TransitionOut(Vector2 worldPos, float duration = 2, float exitTimes = .5f)
    {
        Vector2 anchoredPosition = canvas.WorldToCanvasPosition(worldPos, Camera_Manager.i.cameraObj);

        maskImage.rectTransform.anchoredPosition = anchoredPosition;
        transiFlatImage.rectTransform.anchoredPosition = -anchoredPosition;

        maskImage.rectTransform.sizeDelta = Vector2.one * maxScale;
        yield return TransitionAnim(0, duration, exitTimes: exitTimes);
    }

    private IEnumerator TransitionAnim(float size = 7500, float duration = 2, Ease ease = Ease.Linear, float exitTimes = .5f)
    {
        yield return new WaitForSeconds(exitTimes);
        if (transiTween!= null && transiTween.IsPlaying())
        {
            transiTween.Kill();
            transiTween = null;
        }
        transiTween = DOTween.To(() => maskImage.rectTransform.sizeDelta, x => maskImage.rectTransform.sizeDelta = x, Vector2.one * size, duration).SetEase(ease).OnKill(() => transiTween = null);
        yield return transiTween.WaitForCompletion();
        yield return new WaitForSeconds(exitTimes);
    }
}
