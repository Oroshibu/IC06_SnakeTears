using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    //Singletion Pattern
    private static Game_Manager _i;

    public static Game_Manager i
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

    private void OnEnable()
    {
        Transition_Manager.i.TransitionIn();
    }


    public void Win()
    {
        //THIS LEVEL WON
        Debug.Log("Win");

        //LAUNCH TRANSITION TO NEXT LEVEL
    }
}
