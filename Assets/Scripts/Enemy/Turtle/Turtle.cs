using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enemy
{
    public float intervalTime;
    public float intervalTimeCounter;
    protected override void Awake()
    {
        base.Awake();
        states.Add(EnemyState.Patrol, new TurtlePatrolState());
        states.Add(EnemyState.Chase, new TurtleChaseState());
        intervalTimeCounter = intervalTime;
    }

    public override void Move()
    {
        rb.velocity = Vector2.zero;
    }

    public override void TimeCounter()
    {
        intervalTimeCounter -= Time.deltaTime;
        if (intervalTimeCounter <= 0)
        {
            if (currentState == states[EnemyState.Patrol])
            {
                SwitchState(EnemyState.Chase);
            }
            else
            {
                SwitchState(EnemyState.Patrol);
            }
            intervalTimeCounter = intervalTime;
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 获取碰撞点
            ContactPoint2D contact = collision.contacts[0];
            if (contact.point.y > transform.position.y + upOffset && currentState == states[EnemyState.Patrol])
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
