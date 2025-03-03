using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public Animator anim;
    [HideInInspector]public PhysicsCheck physicsCheck;

    private Collider2D coll;

    [Header("基本参数")] 
    public float nomalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public bool isDead;
    public GameObject walkParticles;
    
    [HideInInspector]public Vector3 faceDirection; // 将enemy的localscale取反

    [Header("计时器")] 
    public float waitTime;

    public float waitTimeCounter;
    public bool wait;
    
    public float lostTime;
    public float lostCounter;
    
    protected BaseState currentState;

    protected Dictionary<EnemyState, BaseState> states = new Dictionary<EnemyState, BaseState>();
    
    [Header("检测Player的参数")]
    public Vector2 centerOffset;

    public Vector2 checkSize;
    public float checkDistence;
    public LayerMask attackLayer;
    
    [Header("踩到enemy的参数")]
    public float upOffset;
    
    
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<Collider2D>();
        
        currentSpeed = nomalSpeed;
        waitTimeCounter = waitTime;
    }

    protected virtual void OnEnable()
    {
        // 将enemy开始的状态设置为patrol
        currentState = states[EnemyState.Patrol];
        currentState.OnEnter(this);
    }

    private void Update()
    {
        // 将enemy的localscale取反,和player一样，1是面朝右，-1是面朝左
        faceDirection = new Vector3(-transform.localScale.x, 0, 0);

        currentState.LogicUpdate();
        TimeCounter();
        
    }

    private void FixedUpdate()
    {
        if(!wait && !isDead)
            Move();
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }

    /// <summary>
    /// enemy死亡
    /// </summary>
    public void Dead()
    {
        gameObject.layer = 2;
        coll.enabled = false;
        // 旋转
        rb.MoveRotation(rb.rotation + 30f);
        CameraShake.Instance.impulseSource.GenerateImpulse();
        anim.SetTrigger("Hit");
        isDead = true;
    }
    
    /// <summary>
    /// 死亡动画结束之后触发
    /// </summary>
    public void DestoryAfterAnimation()
    {
        if(isDead)
            Destroy(gameObject);
    }

    /// <summary>
    /// enemy的移动， 不同的enemy可能要重写这个方法
    /// </summary>
    public virtual void Move()
    {
        if (rb.velocity.x != 0)
        {
            walkParticles.transform.position = (Vector2)transform.position + physicsCheck.bottomOffset * -transform.localScale.x;
            walkParticles.GetComponent<ParticleSystem>().Play();
        }
        rb.velocity = new Vector2(faceDirection.x * currentSpeed * Time.deltaTime, rb.velocity.y);
    }

    /// <summary>
    /// 计数器
    /// </summary>
    public virtual void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDirection.x, 1, 1);
            }
        }

        if (!FoundPlayer())
        {
            lostCounter -= Time.deltaTime;
        }
        else
        {
            lostCounter = lostTime;
        }
    }
    
    /// <summary>
    /// 检测player
    /// </summary>
    /// <returns></returns>
    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDirection, checkDistence,
            attackLayer);
    }
    
    /// <summary>
    /// 转换enemy的状态
    /// </summary>
    /// <param name="state"></param>
    public void SwitchState(EnemyState state)
    {
        var newState = state switch
        {
            EnemyState.Patrol => states[EnemyState.Patrol],
            EnemyState.Chase => states[EnemyState.Chase],
            _ => null
        };
        // 切换状态先从上一个状态退出
        currentState.OnExit();
        // 进入新状态
        currentState = newState;
        currentState.OnEnter(this);
    }

    /// <summary>
    /// 可视化检测player的距离
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset+ new Vector3(checkDistence * -transform.localScale.x, 0), 0.2f);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            // 获取碰撞点
            ContactPoint2D contact = collision.contacts[0];
            if (contact.point.y > transform.position.y + upOffset)
            {
                Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
                playerRB.AddForce(transform.up * 10f, ForceMode2D.Impulse);
                // 敌人死亡
                Dead();
            }
            else
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.Dead(transform);
                }
            }
        }
    }
}
