using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock_Behavior : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
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

    private IEnumerator BreakCoroutine()
    {
        ps.Play();
        Camera_Manager.i.Shake(.5f,.15f);
        tilemap.EraseTileAt(transform.position);
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
