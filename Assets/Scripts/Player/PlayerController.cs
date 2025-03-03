using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    
    private Animator anim;
    
    private CapsuleCollider2D coll;
    private Vector2 originalOffset;
    private Vector2 originalSize;

    
    [Header("人物移动控制")]
    public PlayerInputControl inputControl;
    [HideInInspector]public Vector2 inputDirection;
    private PhysicsCheck physicsCheck;
    public float speed;
    public float jumpForce;
    public float wallJumpForce;
    public int maxJumpCount = 2; //跳跃次数
    private int currentJumpCount;
    public GameObject walkParticle;
    
    [Header("状态")]
    private bool isDead;

    private bool wallJump;

    private void Awake()
    {
        inputControl = new PlayerInputControl();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        originalOffset = coll.offset;
        originalSize = coll.size;
        inputControl.GamePlay.Jump.started += Jump;
        isDead = false;
    }

    private void Start()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        inputControl.Enable();
        EventHandler.RestartGameEvent += OnRestartGameEvent;
        EventHandler.SceneLoadedEvent += OnSceneLoadedEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        inputControl.Disable();
        EventHandler.RestartGameEvent += OnRestartGameEvent;
        EventHandler.SceneLoadedEvent -= OnSceneLoadedEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }
    
    private void Update()
    {
        inputDirection = inputControl.GamePlay.Move.ReadValue<Vector2>();
        anim.SetFloat("VercitalVelocity", rb.velocity.y);
        anim.SetFloat("HorizontalVelocity", Mathf.Abs(rb.velocity.x));
        anim.SetBool("IsGround", physicsCheck.isGround);
        // coll.sharedMaterial = physicsCheck.onWall ? wall : nomal;
        anim.SetBool("onWall", physicsCheck.onWall);
        anim.SetBool("Dead", isDead);


        
    }

    private void FixedUpdate()
    {
        Move();
        if(wallJump && rb.velocity.y < 0)
        {
            wallJump = false;
        }
        
        if (rb.velocity.x != 0 && physicsCheck.isGround)
            walkParticle.GetComponent<ParticleSystem>().Play();
    }

    /// <summary>
    /// 人物移动
    /// </summary>
    public void Move()
    {
        // 人物移动
        if (!wallJump)
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        if (physicsCheck.onWall)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y/2);
        
        // 人物翻转(使用transform.localScale进行翻转，也可以使用Sprite Renderer的flip属性进行翻转)
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if(inputDirection.x < 0)
            faceDir = -1;

        transform.localScale = new Vector3(faceDir, 1, 1);

        if (physicsCheck.onWall)
        {
            coll.size = new Vector2(originalSize.x - 0.4f, originalSize.y);
        }
        else
        {
            coll.size = originalSize;
        }
    }
    
    /// <summary>
    /// 空格跳跃
    /// </summary>
    /// <param name="obj"></param>
    private void Jump(InputAction.CallbackContext obj)
    {
        if (physicsCheck.isGround)
        {
            // 如果在地面上，重置跳跃次数
            // ani.SetBool("IsGround", true);
            currentJumpCount = 1;
            anim.SetTrigger("Jump");
            rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
        }
        else if(physicsCheck.onWall)
        {
            rb.AddForce(new Vector2(-inputDirection.x , 2f)*wallJumpForce, ForceMode2D.Impulse);
            wallJump = true;
        }
        else if (currentJumpCount < maxJumpCount)
        {
            // 如果没有达到最大跳跃次数，允许进行第二次跳跃
            anim.SetTrigger("DoubleJump");
            currentJumpCount++;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
        }
    }
    
    public void Bounce(float force)
    {
        rb.AddForce(transform.up * force, ForceMode2D.Impulse);
    }

    /// <summary>
    /// 人物死亡
    /// </summary>
    public void Dead(Transform attacker)
    {
        isDead = true;
        // 人物被击退
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 1).normalized;
        rb.AddForce(dir * 15f, ForceMode2D.Impulse);
        // 旋转一个角度
        rb.MoveRotation(rb.rotation + 30f);
        // 取消碰撞体
        coll.enabled = false;
        CameraShake.Instance.impulseSource.GenerateImpulse();
        inputControl.GamePlay.Disable();
        EventHandler.CallPlayerDeadEvent();
        inputControl.GamePlay.Disable();
        
        Debug.Log("player dead");
        
    }

    public void SetFalseAfterAnimation()
    {
        gameObject.SetActive(false);
    }
    
    private void OnRestartGameEvent()
    {
        gameObject.SetActive(true);
        coll.enabled = true;
        rb.rotation = 0;
        inputControl.GamePlay.Enable();
        isDead = false;
        transform.position = GameObject.FindGameObjectWithTag("Start").transform.position;
    }
    
    private void OnSceneLoadedEvent()
    {
        // 场景加载时禁止player的移动控制
        inputControl.GamePlay.Disable();
    }
    
    private void OnAfterSceneLoadedEvent()
    {
        // 加载完成后恢复player的移动控制
        inputControl.GamePlay.Enable();
    }
}
