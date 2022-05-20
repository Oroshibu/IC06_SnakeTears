using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tilemap_Behavior : MonoBehaviour
{
    public Tilemap tilemap;

    public void EraseTileAt(Vector3 position)
    {
       var pos = tilemap.WorldToCell(position);
       tilemap.SetTile(pos, null);
    }
}
