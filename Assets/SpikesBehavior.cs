using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Game_Manager.i.Death();
        }
    }
}
