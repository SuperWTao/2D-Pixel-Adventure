using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RockHead : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private int direction;
    
    public RockHeadState rockHeadState;
    public float speed;
    
    public bool wait;
    public float waitTime;
    public float waitTimeCounter;

    public LayerMask playerLayer;
    
    public float checkRadius;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public Vector2 topOffset;
    public Vector2 bottomOffset;
    private bool isLeft;
    private bool isRight;
    private bool isTop;
    private bool isBottom;
    

    private GameObject player;

    private void Start()
    {
        direction = 1;
        waitTimeCounter = waitTime;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        RigidbodyConstraints2D constraints = RigidbodyConstraints2D.FreezeRotation;
        if (rockHeadState == RockHeadState.horizontal)
        {
            anim.SetFloat("Direction", 0);
            constraints |= RigidbodyConstraints2D.FreezePositionY;
        }

        else
        {
            anim.SetFloat("Direction", 1);
            constraints |= RigidbodyConstraints2D.FreezePositionX;
        }
        rb.constraints = constraints;
    }

    private void Update()
    {
        
        // 更新动画状态
        anim.SetBool("Move", !wait);
        // 计时器逻辑
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime; // 重置计时器
                direction = -direction; // 切换方向
            }
        }

        if (player != null)
        {
            if (rockHeadState == RockHeadState.horizontal && ((player.GetComponent<PhysicsCheck>().isLeftWall && isLeft) ||
                                                              (player.GetComponent<PhysicsCheck>().isRightWall && isRight)))
            {
                player.GetComponent<PlayerController>().Dead(transform);
            }
            if (rockHeadState == RockHeadState.vertical && (player.GetComponent<PhysicsCheck>().isTop && isTop || player.GetComponent<PhysicsCheck>().isGround && isBottom))
            {
                player.GetComponent<PlayerController>().Dead(transform);
            }
        }
        
            
    }

    private void FixedUpdate()
    {
        if (!wait)
        {
            Check();
            Move();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetTrigger("Hit");
            wait = true;
        }
        
    }
    
    public void Move()
    {
        if (rockHeadState == RockHeadState.horizontal)
        {
            rb.velocity = new Vector2(speed*direction*Time.deltaTime, rb.velocity.y);
        }
        else if (rockHeadState == RockHeadState.vertical)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed*direction);
        }
    }
    
    public void Check()
    {
        isLeft = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, playerLayer);
        isRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRadius, playerLayer);
        isBottom = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRadius, playerLayer);
        isTop = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, checkRadius, playerLayer);
    }
    //
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRadius);
    //     Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRadius);
    //     Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);
    //     Gizmos.DrawWireSphere((Vector2)transform.position + topOffset, checkRadius);
    // }
}
