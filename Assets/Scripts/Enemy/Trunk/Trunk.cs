using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : Enemy
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    
    protected override void Awake()
    {
        base.Awake();
        states.Add(EnemyState.Patrol, new TrunkPatrolState());
        states.Add(EnemyState.Chase, new TrunkChaseState());
    }

    public override void Move()
    {
        if (rb.velocity.x!=0)
        {
            walkParticles.transform.position = (Vector2)transform.position + physicsCheck.bottomOffset * -transform.localScale.x;
            walkParticles.GetComponent<ParticleSystem>().Play();
        }
        
        if (FoundPlayer())
        {
            rb.velocity = Vector2.zero;
        }
        rb.velocity = new Vector2(faceDirection.x * currentSpeed * Time.deltaTime, rb.velocity.y);
    }

    public void Launch()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, rb.position + new Vector2(1.85f * faceDirection.x, 0.81f), Quaternion.identity);
        bulletObject.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Launch(faceDirection, bulletSpeed);
    }
}
