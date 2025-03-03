using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skull : Enemy
{
    public Vector2 direction;

    protected override void Awake()
    {
        base.Awake();
        states.Add(EnemyState.Patrol, new SkullPatrolState());
        states.Add(EnemyState.Chase, new SkullChaseState());
        direction = GetRandomDirection();
    }

    public override void Move()
    {
        // transform.position += (Vector3)direction*currentSpeed*Time.deltaTime;
        rb.velocity = direction * currentSpeed * Time.deltaTime;
    }
    
    Vector2 GetRandomDirection()
    {
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        return randomDirection.normalized;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetTrigger("HitWall");
            Vector2 normal = collision.contacts[0].normal;
            direction = Vector2.Reflect(direction, normal).normalized; // 反射后的方向是入射方向相对于法线的镜像
            // 添加随机偏移
            direction += GetRandomDirection() * 0.1f; // 0.1f是随机偏移的强度
            direction.Normalize();
            SwitchState(currentState == states[EnemyState.Patrol] ? EnemyState.Chase : EnemyState.Patrol);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (currentState == states[EnemyState.Patrol])
            {
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
