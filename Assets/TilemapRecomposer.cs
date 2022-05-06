using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class TilemapRecomposer : MonoBehaviour
{
    void Start()
    {
        var collider = GetComponent<TilemapCollider2D>();
        collider.usedByComposite = false;
        collider.usedByComposite = true;
    }
}
