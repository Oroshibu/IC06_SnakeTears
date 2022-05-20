using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels_Manager : MonoBehaviour
{
    private static Levels_Manager _i;

    public static Levels_Manager i { 
        get 
        {
            if (_i != null)
            {
                return _i;
            } else
            {
                return _i = new GameObject("Levels_Manager").AddComponent<Levels_Manager>();
            }            
        } 
    }

    private int actualLevelID = 0;

    private void Awake()
    {
        if (_i == null && _i != this)
        {
            _i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        actualLevelID = 0;
    }

}
