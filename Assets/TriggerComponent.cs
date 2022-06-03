using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerComponent : MonoBehaviour
{
    [HideInInspector] public delegate void Triggered(Collider2D collision);
    [HideInInspector] public Triggered onTrigger;

    [SerializeField] bool onStay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTrigger(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (onStay) onTrigger(collision);
    }
}
