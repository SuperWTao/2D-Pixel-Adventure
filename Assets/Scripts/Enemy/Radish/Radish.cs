using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radish : Enemy
{
    public GameObject leafsParticles;
    public GameObject flyingParticles;
    public GameObject[] targetPoints;
    private int currentPoint;
    private int currentHit;
    
    
    protected override void Awake()
    {
        base.Awake();
        states.Add(EnemyState.Patrol,new RadishPatrolState());
        states.Add(EnemyState.Chase,new RadishChaseState());
        currentPoint = 0;
        flyingParticles.transform.position = transform.position;
    }

    public override void Move()
    {
        if (physicsCheck.isGround && currentState != states[EnemyState.Chase])
        {
            anim.SetBool("Run", true);
            SwitchState(EnemyState.Chase);
        }
        if (currentState == states[EnemyState.Patrol])
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoints[currentPoint].transform.position, currentSpeed * Time.deltaTime);
            flyingParticles.GetComponent<ParticleSystem>().Play();
            if (Mathf.Abs(transform.position.x - targetPoints[currentPoint].transform.position.x) <= 0.01f)
            {
                currentPoint = (currentPoint + 1) % targetPoints.Length;
                transform.localScale = new Vector3(Mathf.Sign(transform.position.x- targetPoints[currentPoint].transform.position.x), 1, 1);
            }
        }
        else if(currentState == states[EnemyState.Chase])
        {
            if (rb.velocity.x != 0)
            {
                walkParticles.transform.position = (Vector2)transform.position + physicsCheck.bottomOffset * -transform.localScale.x;
                walkParticles.GetComponent<ParticleSystem>().Play();
            }
            rb.velocity = new Vector2(faceDirection.x * currentSpeed * Time.deltaTime, rb.velocity.y);
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 获取碰撞点
            ContactPoint2D contact = collision.contacts[0];
            Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
            playerRB.AddForce(transform.up * 10f, ForceMode2D.Impulse);
            if (contact.point.y > transform.position.y + upOffset)
            {
                if (currentHit == 0)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    currentHit++;
                    anim.SetTrigger("Hit");
                    anim.SetBool("Walk", true);
                    // 播放叶子掉落particle
                    GameObject leaf = Instantiate(leafsParticles, (Vector2)transform.position + new Vector2(0,1.7f), Quaternion.identity);
                    leaf.GetComponent<ParticleSystem>().Play();
                }
                else
                {
                    // 敌人死亡
                    Dead();
                }
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
