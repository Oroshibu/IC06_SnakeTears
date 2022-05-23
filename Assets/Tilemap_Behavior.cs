using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tilemap_Behavior : MonoBehaviour
{
    public Tilemap tilemap;
    [SerializeField] List<RuleTile> tiles;

    private void Start()
    {
        ImportTileMap(GameObject.Find("IntGrid").GetComponent<Tilemap>());

        Camera_Manager.i.FocusOnGrid(tilemap);
    }
    
    private void ImportTileMap(Tilemap intGrid)
    {
        var bounds = intGrid.cellBounds;

        transform.parent.position = intGrid.transform.position;

        foreach (var i in bounds.allPositionsWithin)
        {
            if (intGrid.HasTile(i))
            {
                tilemap.SetTile(i, tiles.Find(x => x.name == intGrid.GetTile(i).name));
            }
        }

        Destroy(intGrid.gameObject);
    }

    public void EraseTileAt(Vector3 position)
    {
       var pos = tilemap.WorldToCell(position);
       tilemap.SetTile(pos, null);
    }
}
