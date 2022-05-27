using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock_Behavior : MonoBehaviour
{
    [SerializeField] GameObject ps;
    private Tilemap_Behavior tilemap;

    private void Start()
    {
        tilemap = GetComponentInParent<Tilemap_Behavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            var stoneComp = collision.gameObject.GetComponent<StoneComponent>();
            if (stoneComp.IsFalling())
            {

                GameObject adjacentTile;

                if (((transform.position.x - .25f) % 1) == 0){
                    stoneComp.ResetFallingVelocity();
                    Break();
                    
                    adjacentTile = tilemap.GetTileObjectAt(transform.position + Vector3.right * .5f);
                } else
                {
                    stoneComp.ResetFallingVelocity();
                    Break();
                    
                    adjacentTile = tilemap.GetTileObjectAt(transform.position - Vector3.right * .5f);
                }

                if (adjacentTile != null)
                {
                    adjacentTile.TryGetComponent(out BreakableBlock_Behavior adjacentBlock);

                    if (adjacentBlock != null)
                    {
                        stoneComp.ResetFallingVelocity();
                        adjacentBlock.Break();
                    }
                }

            }
        }
    }
    
    public void Break()
    {
        StartCoroutine(BreakCoroutine());
    }

    IEnumerator BreakCoroutine()
    {
        var instance = Instantiate(ps, transform.position, transform.rotation);
        Destroy(instance.gameObject, 5);

        Camera_Manager.i.Shake(.5f, .15f);

        yield return new WaitForEndOfFrame();
        float posX = transform.position.x;
        posX = Mathf.FloorToInt(posX / 2) * 2;
        Vector3 recombPos = new Vector3(posX, transform.position.y, transform.position.z);

        tilemap.EraseTileAt(transform.position);
    }
}
