using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StoneComponent : MonoBehaviour
{
    [SerializeField] LayerMask maskGround;

    [Header("Physics")]
    public float pushDelay = .25f;
    public float pushXStep = 1;    
    public float snapYStep = .5f;
    public Vector2 offset;
    public Vector2 boxColliderWidths;

    [Header("Network")]
    public Network_Interface network;

    bool isFalling = false;
    bool isPushed = false;
    bool isPushedMoving = false;
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
                StartPush(pushedDirection.x);
                var pushedDirectionEnum = pushedDirection.x > 0 ? Network_Interface.Direction.Right : Network_Interface.Direction.Left;
                foreach(var stone in network.GetExtendedStoneNetwork(pushedDirectionEnum))
                {
                    stone.StartPush(pushedDirection.x);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerVelocity = collision.gameObject.GetComponent<Player_Controller>().direction.normalized;
            if (isPushed && playerVelocity.x == 0)
            {
                StopPush();
                var pushedDirectionEnum = pushedDirection.x > 0 ? Network_Interface.Direction.Right : Network_Interface.Direction.Left;
                foreach (var stone in network.GetExtendedStoneNetwork(pushedDirectionEnum))
                {
                    stone.StopPush();
                }
            } else if (!isPushed && playerVelocity.x != 0)
            {
                OnCollisionEnter2D(collision);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopPush();
            var pushedDirectionEnum = pushedDirection.x > 0 ? Network_Interface.Direction.Right : Network_Interface.Direction.Left;
            foreach (var stone in network.GetExtendedStoneNetwork(pushedDirectionEnum))
            {
                stone.StopPush();
            }
        }
    }

    public void StartPush(float pushDirectionX)
    {
        isPushed = true;
        if (pushCoroutineRef != null)
        {
            StopCoroutine(pushCoroutineRef);
        }
        pushCoroutineRef = StartCoroutine(pushCoroutine(pushDirectionX));

        foreach (var stone in network.GetExtendedStoneNetwork(Network_Interface.Direction.Up))
        {
            stone.StartPush(pushDirectionX);
        }
    }

    public void StopPush()
    {
        if (isPushed)
        {
            isPushed = false;
            if (pushCoroutineRef != null)
            {
                StopCoroutine(pushCoroutineRef);
            }
        }

        foreach (var stone in network.GetExtendedStoneNetwork(Network_Interface.Direction.Up))
        {
            stone.StopPush();
        }
    }

    private bool isGridSnapped()
    {
        if ((transform.position.x - offset.x)%1 == 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    IEnumerator pushCoroutine(float pushDirectionX)
    {
        while (isPushed && !isFalling)
        {
            yield return new WaitForSeconds(pushDelay);
            var pushedDirectionEnum = pushDirectionX > 0 ? Network_Interface.Direction.Right : Network_Interface.Direction.Left;
            if (!network.IsWallInNetworkDirection(pushedDirectionEnum) || !isGridSnapped())
            {
                float endValue;
                if (pushDirectionX >= 0)
                {
                    endValue = Mathf.Floor(transform.position.x - offset.x + pushDirectionX * pushXStep) + offset.x;
                } else
                {
                    endValue = Mathf.Ceil(transform.position.x - offset.x + pushDirectionX * pushXStep) + offset.x;
                }
                //yield return transform.DOMoveX(Mathf.Round(transform.position.x - offset.x + pushDirectionX * pushXStep) + offset.x, .4f).WaitForCompletion();
                //isPushedMoving = true;
                //yield return transform.DOMoveX(endValue, .4f).OnComplete(() => isPushedMoving = false).WaitForCompletion();
                yield return Move(endValue, .4f);
            }

            //Stone can push new stone on reset
            foreach (var stone in network.GetExtendedStoneNetwork(pushedDirectionEnum))
            {
                stone.StartPush(pushedDirection.x);
            }
        }
    }

    public IEnumerator Move(float endValue, float duration = .4f)
    {
        isPushedMoving = true;
        yield return transform.DOMoveX(endValue, duration).OnComplete(() => isPushedMoving = false).WaitForCompletion();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();

        //COMMENTED TEMPORARY
        //network.networkLeft.OnNetworkUpdate += OnEncounterObjectWhileMoving;
        //network.networkRight.OnNetworkUpdate += OnEncounterObjectWhileMoving;
    }

    //private void OnEnable()
    //{
    //    SnapToClosestX();
    //}

    public void SnapToClosestX()
    {
        var endValue = Mathf.Round(transform.position.x - offset.x) + offset.x;
        transform.position = new Vector2(endValue, transform.position.y);
    }

    private void OnEncounterObjectWhileMoving(GameObject gameObject)
    {
        if (isPushedMoving)
        {
            if (gameObject != null)
            {
                if (gameObject.CompareTag("Stone"))
                {
                    gameObject.GetComponent<StoneComponent>().StartPush(pushedDirection.x);
                }
            }
        }
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
            GetComponent<BoxCollider2D>().size = new Vector2(boxColliderWidths.y, 1f);
        } else
        {
            rb.isKinematic = false;
            GetComponent<BoxCollider2D>().size = new Vector2(boxColliderWidths.x, 1f);
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

    public bool IsFalling()
    {
        return isFalling;
    }

    public void ResetFallingVelocity()
    {
        rb.velocity = new Vector2(0, -0.05f);
    }
}
