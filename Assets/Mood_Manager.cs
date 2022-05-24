using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LDtkUnity;

//[ExecuteInEditMode]
public class Mood_Manager : MonoBehaviour
{
    //Singletion Pattern

    private static Mood_Manager _i;

    public static Mood_Manager i
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
            GetLevelColor();
        } else
        {
            Destroy(gameObject);
        }
    }

    public Color sceneColor;

    private void Start()
    {
        GetLevelColor();
    }

    private void GetLevelColor()
    {
        var levelComponent = FindObjectOfType<LDtkComponentLevel>();
        if (levelComponent != null)
        {
            sceneColor = FindObjectOfType<LDtkComponentLevel>().BgColor;
        }
    }

}
