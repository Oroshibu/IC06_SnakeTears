using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StoneComponent : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    
    [SerializeField] LayerMask maskGround;
    public SpriteRenderer sprite;

    [SerializeField] private ParticleSystem ps;


    [Header("Physics")]
    public float pushDelay = .25f;
    public float pushXStep = 1;    
    public float snapYStep = .5f;
    public Vector2 offset;
    public Vector2 boxColliderWidths;

    [Header("Tree")]
    [SerializeField] LayerMask maskTree;

    [HideInInspector] public bool canBePushed = false;
    [HideInInspector] public bool isFalling = false;
    [HideInInspector] public bool isGrounded = true;
    private bool isOnTilesGround;
    private bool snapped;
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
            //if (isFalling) return;
            var vec = (Vector2)collision.transform.position - (Vector2)transform.position;
            
            //yDist 0 -> 1
            var yDist = (collision.transform.position.y - transform.position.y);
            if (Vector2.Angle(Vector2.up, vec) > 50 && yDist > -.05f && yDist < bc.size.y +.05f)
            {
                Player_Controller player = collision.gameObject.GetComponent<Player_Controller>();

                if (player.isGrounded)
                {
                    pushedDirection = player.direction.normalized;
                    if (!canBePushed) return;
                    player.SetIsPushing(true);
                    StartPush(pushedDirection.x);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player_Controller player = collision.gameObject.GetComponent<Player_Controller>();
            var playerVelocity = player.direction.normalized;
            
            if (playerVelocity.x == 0)
            {
                player.SetIsPushing(false);
            }
            
            if (isPushed && playerVelocity.x == 0)
            {
                StopPush();
                Game_Manager.i.player.DOStopMovePlayer();                
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
            Player_Controller player = collision.gameObject.GetComponent<Player_Controller>();
            StopPush();
            player.SetIsPushing(false);
            Game_Manager.i.player.DOStopMovePlayer();
            var pushedDirectionEnum = pushedDirection.x > 0 ? Direction.Right : Direction.Left;
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
    }

    IEnumerator pushCoroutine(float pushDirectionX)
    {
        yield return new WaitForSeconds(pushDelay);
        yield return TryPushBlock(pushDirectionX);
        StopPush();
    }

    public IEnumerator TryPushBlock(float pushDirectionX)
    {
        //if line pushed
        if (TryPushLine(pushDirectionX))
        {
            var topObject = GetObject(Direction.Up);
            if (topObject == null || !topObject.CompareTag("Ground"))
            {
                Game_Manager.i.player.DOMovePlayer(pushDirectionX * pushXStep, .4f);
            }
            yield return new WaitForSeconds(.4f);
        }

        //var direction = pushDirectionX > 0 ? Direction.Right : Direction.Left;        
        //if (CanPushLine(direction))
        //{
        //    RecursivePushLine(pushDirectionX);
            
        //    Game_Manager.i.player.DOMovePlayer(pushDirectionX * pushXStep, .4f);
        //    yield return new WaitForSeconds(.41f);
        //}
    }

    private bool TryPushLine(float pushDirectionX)
    {
        var direction = pushDirectionX > 0 ? Direction.Right : Direction.Left;
        if (CanPushLine(direction))
        {
            //push line
            RecursivePushLine(pushDirectionX);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RecursivePushLine(float pushDirectionX)
    {
        if (isPushedMoving) return;

        MoveThisStone(pushDirectionX);

        var direction = pushDirectionX > 0 ? Direction.Right : Direction.Left;
        GameObject nextObject = GetObject(direction);

        if (nextObject != null && nextObject.CompareTag("Stone"))
        {
            StoneComponent nextStone = nextObject.GetComponent<StoneComponent>();
            nextStone.RecursivePushLine(pushDirectionX);
        }

        GameObject upObject = GetObject(Direction.Up);

        if (upObject != null && upObject.CompareTag("Stone"))
        {
            StoneComponent upStone = upObject.GetComponent<StoneComponent>();
            upStone.TryPushLine(pushDirectionX);
        }
    }

    private void MoveThisStone(float pushDirectionX)
    {
        float endValue;
        if (pushDirectionX >= 0)
        {
            endValue = Mathf.Floor(transform.position.x - offset.x + pushDirectionX * pushXStep) + offset.x;
        }
        else
        {
            endValue = Mathf.Ceil(transform.position.x - offset.x + pushDirectionX * pushXStep) + offset.x;
        }
        if (isOnTilesGround) ps.Play();
        Move(endValue, .4f);
    }

    private bool CanPushLine(Direction direction)
    {
        GameObject nextObject = GetObject(direction);
        if (nextObject == null){
            return true;
        } else if (nextObject.CompareTag("Stone"))
        {
            StoneComponent stoneComp = nextObject.GetComponent<StoneComponent>();
            if (!stoneComp.canBePushed)
            {
                return false;
            } else
            {
                return stoneComp.CanPushLine(direction);
            }
        } else //Ground & Enemy
        {
            return false;
        }
    }

    private GameObject GetObject(Direction direction)
    {
        Vector2 projOffset;
        Vector2 boxSize;
        
        if (direction == Direction.Up)
        {
            projOffset = Vector2.up;
            boxSize = new Vector2(.8f, .3f);
        } else if (direction == Direction.Left)
        {
            projOffset = Vector2.left;
            boxSize = new Vector2(.3f, .95f);
        } else if (direction == Direction.Right)
        {
            projOffset = Vector2.right;
            boxSize = new Vector2(.3f, .95f);
        }
        else
        {
            projOffset = Vector2.down;
            boxSize = new Vector2(.8f, .3f);
        }
        
        var m_HitDetect = Physics2D.BoxCast(
            bc.bounds.center + (Vector3)projOffset * .75f,
            boxSize,
            0f,
            projOffset,
            .5f,
            maskTree
        );

        if (m_HitDetect)
        {
            //Output the name of the Collider your Box hit
            //Debug.Log("Hit : " + m_HitDetect.collider.name);

            return m_HitDetect.collider.gameObject;
        } else
        {
            return null;
        }
    }

    private void Move(float endValue, float duration = .4f)
    {
        isPushedMoving = true;
        transform.DOMoveX(endValue, duration).OnComplete(() => isPushedMoving = false);
    }

    //public IEnumerator Move(float endValue, float duration = .4f)
    //{
    //    isPushedMoving = true;
    //    yield return transform.DOMoveX(endValue, duration).OnComplete(() => isPushedMoving = false).WaitForCompletion();
    //}

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        ps.transform.parent = transform;
        ps.transform.localPosition = Vector3.zero;
    }

    public void SnapToClosestX()
    {
        var endValue = Mathf.Round(transform.position.x - offset.x) + offset.x;
        transform.position = new Vector2(endValue, transform.position.y);
    }

    public void SnapToClosestX(float duration)
    {
        if (snapped) return;
        snapped = true;
        var endValue = Mathf.Round(transform.position.x - offset.x) + offset.x;
        Move(endValue, duration);
        //transform.position = new Vector2(endValue, transform.position.y);
    }

    private void OnDisable()
    {
        snapped = false;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < -.01f)
        {
            isFalling = true;
        } else
        {
            isFalling = false;
        }

        //if (!isFalling)
        isGrounded = IsGrounded();
        if (isGrounded && !isFalling)
        {
            if (!canBePushed && isOnTilesGround)
            {
                ps.Play();
            }
            canBePushed = true;
            rb.isKinematic = true;
            transform.position = new Vector2(transform.position.x, Mathf.Round((transform.position.y - offset.y)/snapYStep)*snapYStep + offset.y);
            GetComponent<BoxCollider2D>().size = new Vector2(boxColliderWidths.y, 1f);
        } else
        {
            canBePushed = false;
            rb.isKinematic = false;
            GetComponent<BoxCollider2D>().size = new Vector2(boxColliderWidths.x, 1f);
        }
    }

    private bool IsGrounded()
    {
        var cast = Physics2D.BoxCast(bc.bounds.center - new Vector3(0, bc.bounds.size.y / 2 + +.25f), new Vector2(bc.bounds.size.x * .75f, .05f), 0f, Vector2.down, .2f, maskGround);
        if (!cast) return false;
        isOnTilesGround = cast.collider.CompareTag("Ground");
        return true;
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(bc.bounds.center - new Vector3(0, bc.bounds.size.y / 2 + .25f), new Vector2(bc.bounds.size.x * .75f, .05f));

            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(bc.bounds.center + Vector3.right * .75f, new Vector2(.3f, .95f));
            Gizmos.DrawCube(bc.bounds.center + Vector3.left * .75f, new Vector2(.3f, .95f));
            Gizmos.DrawCube(bc.bounds.center + Vector3.up * .75f, new Vector2(.8f, .3f));
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
