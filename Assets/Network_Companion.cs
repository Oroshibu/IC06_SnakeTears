using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network_Companion : MonoBehaviour
{
    public Network_Interface.Direction direction;

    //[HideInInspector] 
    public GameObject networkObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        networkObject = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        networkObject = null;
    }
}
