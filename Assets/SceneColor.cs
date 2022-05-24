using UnityEngine;
using UnityEngine.Tilemaps;

//[ExecuteInEditMode]
public class SceneColor : MonoBehaviour
{
    public enum RendererTypeEnum
    {
        SpriteRenderer,
        Tilemap,
        Camera,
        ParticleSystem,
        Shader
    }

    public RendererTypeEnum RendererType = RendererTypeEnum.SpriteRenderer;

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
            case RendererTypeEnum.ParticleSystem:
                var mainInterface = GetComponent<ParticleSystem>().main;
                mainInterface.startColor = Mood_Manager.i.sceneColor;
                break;
            case RendererTypeEnum.Shader:
                GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Mood_Manager.i.sceneColor);
                break;
        }
        
    }
}
