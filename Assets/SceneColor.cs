using UnityEngine;
using UnityEngine.Tilemaps;

public class SceneColor : MonoBehaviour
{
    public enum RendererTypeEnum
    {
        SpriteRenderer,
        Tilemap,
        Camera
    }

    public RendererTypeEnum RendererType = RendererTypeEnum.SpriteRenderer;

    [ExecuteInEditMode]
    void Start()
    {
        switch (RendererType)
        {
            case RendererTypeEnum.SpriteRenderer:
                GetComponent<SpriteRenderer>().color = Mood_Manager.i.sceneColor;
                break;
            case RendererTypeEnum.Tilemap:
                GetComponent<Tilemap>().color = Mood_Manager.i.sceneColor;
                break;
            case RendererTypeEnum.Camera:
                GetComponent<Camera>().backgroundColor = Mood_Manager.i.sceneColor;
                break;
        }
        
    }
}
