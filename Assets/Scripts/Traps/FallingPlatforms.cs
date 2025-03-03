using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    private Rigidbody2D rb;
    private TargetJoint2D target;
    private BoxCollider2D collider;

    public float fallDelay;
    public float fallSpeed;

    private bool isFalling;

    private Collider2D border;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GetComponent<TargetJoint2D>();
        collider = GetComponent<BoxCollider2D>();

        // rb.bodyType = RigidbodyType2D.Kinematic;
        
        GameObject obj = GameObject.FindGameObjectWithTag("Border");
        border = obj.GetComponent<Collider2D>();
        
    }

    private void Update()
    {
        // 超出边界范围销毁对象
        if (isFalling)
        {
            if (!border.bounds.Contains(transform.position))
            {
                Destroy(gameObject);
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            // 获取碰撞点
            // ContactPoint2D contact = collision.GetContact(0);
    
            if (collision.transform.position.y > transform.position.y)
            {
                isFalling = true;
                Invoke("Fall", fallDelay);
            }
        }
        else if (collision.gameObject.CompareTag("Ground") && isFalling)
        {
            Destroy(gameObject);
        }
    }
    
    private void Fall()
    {
        target.enabled = false;
        collider.isTrigger = false;
    }
}
