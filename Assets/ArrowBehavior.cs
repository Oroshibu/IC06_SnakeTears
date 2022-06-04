using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector2.right * Mathf.Sign(transform.lossyScale.x) * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Game_Manager.i.Death();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Stone"))
        {
            Destroy(gameObject);
        }
    }
}
