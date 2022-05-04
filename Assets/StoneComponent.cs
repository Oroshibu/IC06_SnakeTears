using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StoneComponent : MonoBehaviour
{
    [SerializeField] LayerMask maskGround;

    public float pushDelay = .25f;
    public float pushXStep = 1;    
    public float snapYStep = .5f;
    public Vector2 offset;


    bool isFalling = false;
    bool isPushed = false;
    Vector2 pushedDirection;

    Coroutine pushCoroutineRef;
    Rigidbody2D rb;
    BoxCollider2D bc;



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var vec = (Vector2)collision.transform.position - (Vector2)transform.position;
            if (Vector2.Angle(Vector2.up, vec) > 50)
            {
                pushedDirection = collision.gameObject.GetComponent<Player_Controller>().direction.normalized;
                StartPush();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopPush();
        }
    }

    void StartPush()
    {
        isPushed = true;
        if (pushCoroutineRef != null)
        {
            StopCoroutine(pushCoroutineRef);
        }
        pushCoroutineRef = StartCoroutine(pushCoroutine());
    }

    void StopPush()
    {
        if (isPushed)
        {
            isPushed = false;
            if (pushCoroutineRef != null)
            {
                StopCoroutine(pushCoroutineRef);
            }
        }
    }

    IEnumerator pushCoroutine()
    {
        while (isPushed && !isFalling)
        {
            yield return new WaitForSeconds(pushDelay);
            //yield return transform.DOMoveX(Mathf.Round((transform.position.x + pushedDirection.x * offset)/offset)*offset, .5f).WaitForCompletion();
            yield return transform.DOMoveX(Mathf.Round(transform.position.x - offset.x + pushedDirection.x * pushXStep) + offset.x, .5f).WaitForCompletion();

        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < -.01f)
        {
            isFalling = true;
            StopPush();
        } else
        {
            isFalling = false;
        }

        //if (!isFalling)
        if (IsGrounded() && !isFalling)
        {
            rb.isKinematic = true;
            transform.position = new Vector2(transform.position.x, Mathf.Round((transform.position.y - offset.y)/snapYStep)*snapYStep + offset.y);
        } else
        {
            rb.isKinematic = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(bc.bounds.center - new Vector3(0, bc.bounds.size.y / 2 + +.25f), new Vector2(bc.bounds.size.x * .75f, .05f), 0f, Vector2.down, .2f, maskGround);
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(bc.bounds.center - new Vector3(0, bc.bounds.size.y / 2 + .25f), new Vector2(bc.bounds.size.x * .75f, .05f));
        }
    }
}
