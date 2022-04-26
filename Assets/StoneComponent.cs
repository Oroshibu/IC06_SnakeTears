using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StoneComponent : MonoBehaviour
{
    public float pushDelay = .5f;
    public float offset = .5f;
    public float yOffset = .5f;


    bool isFalling = false;
    bool isPushed = false;
    Vector2 pushedDirection;

    Coroutine pushCoroutineRef;
    Rigidbody2D rb;



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var vec = (Vector2)collision.transform.position - (Vector2)transform.position; ;
            if (Vector2.Angle(Vector2.up, vec) > 50)
            {
                pushedDirection = collision.gameObject.GetComponent<Player_Controller>().direction.normalized;
                print(pushedDirection);
                isPushed = true;
                if (pushCoroutineRef != null)
                {
                    StopCoroutine(pushCoroutineRef);
                }
                pushCoroutineRef = StartCoroutine(pushCoroutine());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isPushed)
            {
                isPushed = false;
                StopCoroutine(pushCoroutineRef);
            }
        }
    }

    IEnumerator pushCoroutine()
    {
        while (isPushed)
        {
            yield return new WaitForSeconds(pushDelay);
            //yield return transform.DOMoveX(Mathf.Round((transform.position.x + pushedDirection.x * offset)/offset)*offset, .5f).WaitForCompletion();
            yield return transform.DOMoveX((transform.position.x + pushedDirection.x * offset), .5f).WaitForCompletion();

        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isFalling)
        {
            rb.isKinematic = true;
            transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y - yOffset) + yOffset);
        } else
        {
            rb.isKinematic = false;
        }
    }
}
