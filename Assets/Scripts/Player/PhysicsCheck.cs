using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private PlayerController player;
    private Rigidbody2D rb;
    
    [Header("检测参数")]
    public float checkRaduis; // 用于检测的范围，从player底部的锚点开始

    public Vector2 bottomOffset;
    public Vector2 topOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public LayerMask groundLayer; // 图层

    [Header("状态")]
    public bool isGround;
    public bool isTop;
    public bool isLeftWall;
    public bool isRightWall;
    public bool onWall;

    public bool isPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (isPlayer)
        {
            player = GetComponent<PlayerController>();
        }
        
    }

    private void Update()
    {
        Check();
    }

    public void Check()
    {
        // 检测地面
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset*transform.localScale.x, checkRaduis, groundLayer);
        isTop = Physics2D.OverlapCircle((Vector2)transform.position + topOffset, checkRaduis, groundLayer);
        // 检测墙体
        isLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayer);
        isRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);

        if (isPlayer)
            onWall = ((isLeftWall && player.inputDirection.x < 0f) || (isRightWall && player.inputDirection.x > 0f)) &&
                     rb.velocity.y < 0f;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}
