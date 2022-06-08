using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrushableEntity : MonoBehaviour
{
    [SerializeField] GameObject deathParticles;
    private bool crushed = false;
    //get crushed when stone collide on top
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            //get collision direction
            Vector2 direction = collision.contacts[0].normal;
            //if collisions direction up
            if (direction.y < - .75f)
            {
                //crush
                //Debug.Log("crush " + direction.y);
                Crush();
            }
        }
    }
    
    public void Crush()
    {
        if (!crushed)
        {
            crushed = true;
            StartCoroutine(CrushCoroutine());
        }
    }

    IEnumerator CrushCoroutine()
    {
        if (TryGetComponent<PatrolAI>(out PatrolAI ai))
        {
            ai.enabled = false;
        }
        if (TryGetComponent<CircleCollider2D>(out CircleCollider2D cc))
        {
            cc.radius = 0.25f;
        }

        GetComponent<Animator>().speed = 0;

        transform.DOMoveY(transform.position.y - 0.25f, 0.15f);
        transform.DOScaleX(transform.localScale.x * 2f, .1f);
        yield return transform.DOScaleY(transform.localScale.y / 2, .15f).SetEase(Ease.OutBack).WaitForCompletion();
        transform.DOScaleX(transform.localScale.x / 1.75f, .15f);
        yield return new WaitForSeconds(.4f);
        var particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(particles, 1f);
        Destroy(transform.parent.gameObject);
    }
}
