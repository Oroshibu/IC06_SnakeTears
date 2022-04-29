using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class Camera_Manager : MonoBehaviour
{
    public Camera cameraObj;
    public Volume rayVolumeObj;
   
    //Singletion Pattern
    private static Camera_Manager _i;

    public static Camera_Manager i
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

    public void Shake(float duration = .5f, float strength = .3f, int vibrato = 20, DG.Tweening.Ease ease = Ease.OutExpo)
    {
        cameraObj.DOShakePosition(duration, strength, vibrato).SetEase(ease);
    }

    public void RayCameraEffect(float newWeight, float duration)
    {
        DOTween.To(() => rayVolumeObj.weight, x => rayVolumeObj.weight = x, newWeight, duration).SetEase(Ease.OutQuart);
    }
}
