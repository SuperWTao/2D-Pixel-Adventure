using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    
    protected override void Awake()
    {
        base.Awake();
        states.Add(EnemyState.Patrol, new PlantPatrolState());
        states.Add(EnemyState.Chase, new PlantChaseState());
    }

    
    public override void Move()
    {
        rb.velocity = Vector2.zero;
    }

    public override void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                // transform.localScale = new Vector3(faceDirection.x, 1, 1);
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

    public void Launch()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, rb.position + new Vector2(1.4f * faceDirection.x, 1.45f), Quaternion.identity);
        bulletObject.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Launch(faceDirection, bulletSpeed);
    }
}
