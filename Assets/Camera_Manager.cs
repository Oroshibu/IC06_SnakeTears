using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class Camera_Manager : MonoBehaviour
{
    public Camera cameraObj;
    public Volume rayVolumeObj;
    public Volume deathVolumeObj;

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
        if (newWeight > 0)
        {
            rayVolumeObj.gameObject.SetActive(true);
            DOTween.To(() => rayVolumeObj.weight, x => rayVolumeObj.weight = x, newWeight, duration).SetEase(Ease.OutQuart);
        } else
        {
            DOTween.To(() => rayVolumeObj.weight, x => rayVolumeObj.weight = x, newWeight, duration).SetEase(Ease.OutQuart).OnComplete(() => rayVolumeObj.gameObject.SetActive(false));
        }
    }

    public void DeathCameraEffect(float newWeight, float duration)
    {
        if (newWeight > 0)
        {
            deathVolumeObj.gameObject.SetActive(true);
            DOTween.To(() => deathVolumeObj.weight, x => deathVolumeObj.weight = x, newWeight, duration).SetEase(Ease.InQuart);
        }
        else
        {
            DOTween.To(() => deathVolumeObj.weight, x => deathVolumeObj.weight = x, newWeight, duration).SetEase(Ease.InQuart).OnComplete(() => deathVolumeObj.gameObject.SetActive(false));
        }
    }

    public void FocusOnGrid(Tilemap tilemap)
    {
        var bounds = tilemap.cellBounds;

        Vector3Int actualBoundsMin = Vector3Int.back;
        Vector3Int actualBoundsMax = Vector3Int.back;
        foreach (var i in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(i))
            {
                if (actualBoundsMin == Vector3Int.back)
                {
                    actualBoundsMin = i;
                }
                actualBoundsMax = i;
            }
        }
        BoundsInt actualBounds = new BoundsInt(actualBoundsMin, actualBoundsMax - actualBoundsMin + Vector3Int.one);

        Vector2Int emptyBoundMin = (Vector2Int)actualBounds.max;
        Vector2Int emptyBoundMax = (Vector2Int)actualBounds.min;

        foreach (var i in actualBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(i))
            {
                if (i.x <= emptyBoundMin.x){
                    emptyBoundMin.x = i.x;
                }
                else if (i.x > emptyBoundMax.x)
                {
                    emptyBoundMax.x = i.x;
                }
                
                if (i.y <= emptyBoundMin.y)
                {
                    emptyBoundMin.y = i.y;
                }
                else if (i.y > emptyBoundMax.y)
                {
                    emptyBoundMax.y = i.y;
                }
            }
        }

        //Vector2 cornerTopLeftWorld = tilemap.CellToWorld((Vector3Int)emptyBoundMin - Vector3Int.one * 2);
        //Vector2 cornerBottomRightWorld = tilemap.CellToWorld((Vector3Int)emptyBoundMax + Vector3Int.one * 3);
        cornerTopLeftWorld = tilemap.CellToWorld((Vector3Int)emptyBoundMin - Vector3Int.one * 2);
        cornerBottomRightWorld = tilemap.CellToWorld((Vector3Int)emptyBoundMax + Vector3Int.one * 3);

        FocusCam(cornerTopLeftWorld, cornerBottomRightWorld);

        Game_Manager.i.CameraFocused();
    }

    private void FocusCam(Vector2 topLeft, Vector2 bottomRight)
    {
        var size = bottomRight - topLeft;
        var center = (topLeft + bottomRight) / 2;

        cameraObj.transform.position = new Vector3(center.x, center.y, -10f);

        cameraObj.orthographicSize = ((size.x > size.y * cameraObj.aspect) ? (float)size.x / (float)cameraObj.pixelWidth * cameraObj.pixelHeight : size.y) / 2;
    }

    Vector2 cornerTopLeftWorld;
    Vector2 cornerBottomRightWorld;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube((cornerTopLeftWorld + cornerBottomRightWorld) / 2, cornerBottomRightWorld - cornerTopLeftWorld);
    }
}

