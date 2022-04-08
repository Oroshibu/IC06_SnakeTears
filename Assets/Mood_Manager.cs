using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        } else
        {
            Destroy(gameObject);
        }
    }

    public Color sceneColor;

}
