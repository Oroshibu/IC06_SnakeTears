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
                stoneComp.ResetFallingVelocity();
                StartCoroutine(BreakCoroutine());
            }
        }
    }

    IEnumerator BreakCoroutine()
    {
        var instance = Instantiate(ps, transform.position, transform.rotation);
        Destroy(instance.gameObject, 5);
        
        Camera_Manager.i.Shake(.5f, .15f);
        
        yield return new WaitForEndOfFrame();
        tilemap.EraseTileAt(transform.position);
    }
}
