using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCockatrice_Behavior : MonoBehaviour
{
    [SerializeField] Transform raySprite;
    [SerializeField] LayerMask sightMask;

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.lossyScale.x, 30f, sightMask);

        if (hit.collider != null)
        {
            //get hit distance
            float distance = Vector2.Distance(transform.position, hit.point);

            //raySprite.transform.localScale = new Vector3(distance + transform.localPosition.x + .25f, raySprite.transform.localScale.y, raySprite.transform.localScale.z);
            raySprite.GetComponent<SpriteRenderer>().size = new Vector2((distance + transform.localPosition.x + .25f)/.3f, 1);
            raySprite.localPosition = new Vector3((distance - transform.localPosition.x + .25f) / 2, raySprite.localPosition.y, raySprite.localPosition.z);

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                StoneEnemy(hit.transform.gameObject);
            } else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                StonePlayer(hit.transform.gameObject);
            }
        }
    }

    private void StoneEnemy(GameObject go)
    {
        //if (Mathf.Sign(this.transform.parent.localScale.x) != Mathf.Sign(go.transform.parent.localScale.x))
        if (Mathf.Sign(this.transform.parent.lossyScale.x) != Mathf.Sign(go.transform.lossyScale.x))
        {
            go.gameObject.GetComponentInParent<Stoneable_Behavior>().Stone();
        }
    }

    private void StonePlayer(GameObject go)
    {
        //if (Mathf.Sign(this.transform.parent.localScale.x) != Mathf.Sign(go.transform.parent.localScale.x))
        if (Mathf.Sign(this.transform.parent.lossyScale.x) != Mathf.Sign(go.transform.lossyScale.x))
        {
            if (Mathf.Abs(transform.position.y - go.transform.position.y) < .25)
            {
                Game_Manager.i.StonePlayer();
            }

        }
    }

    void OnDrawGizmosSelected()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.lossyScale.x * 30f);
    }
}
