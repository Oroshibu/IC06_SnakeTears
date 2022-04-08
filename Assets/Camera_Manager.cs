using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
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
}
