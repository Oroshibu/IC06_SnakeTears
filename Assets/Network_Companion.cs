using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network_Companion : MonoBehaviour
{
    public Network_Interface.Direction direction;

    //[HideInInspector] 
    public GameObject networkObject;

    [HideInInspector]
    public event Action<GameObject> OnNetworkUpdate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        networkObject = collision.gameObject;
        if (OnNetworkUpdate != null)
        {
            OnNetworkUpdate(networkObject);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        networkObject = null;
        if (OnNetworkUpdate != null)
        {
            OnNetworkUpdate(null);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        networkObject = collision.gameObject;
        if (OnNetworkUpdate != null)
        {
            OnNetworkUpdate(networkObject);
        }
    }

}
