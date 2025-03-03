using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Box : MonoBehaviour
{
    private Animator anim;
    public BoxType type;
    public GameObject[] fruits;
    public GameObject breakParticle;

    private int hitCount;
    private int breakCount;
    private int minFruit;
    private int maxFruit;
    private float bounceForce;

    private void Start()
    {
        anim = GetComponent<Animator>();
        bounceForce = 5;
        if (type == BoxType.Box1)
        {
            breakCount = 2;
            minFruit = 1;
            maxFruit = 2;
        }
        else if (type == BoxType.Box2)
        {
            breakCount = 5;
            minFruit = 2;
            maxFruit = 3;
        }
        else
        {
            breakCount = 7;
            minFruit = 2;
            maxFruit = 4;
        }
    }

    private void Update()
    {
        if (hitCount >= breakCount)
        {
            BoxBreak();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hitCount++;
            CameraShake.Instance.impulseSource.GenerateImpulse();
            ContactPoint2D contact = collision.contacts[0];
            Vector2 normal = contact.normal;
            if (Mathf.Abs(normal.y) > 0.1f)
            {
                anim.SetTrigger("Hit");
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                Vector2 bounceDirection = normal.normalized;
                Vector2 bounceVector = -bounceDirection * bounceForce;
                rb.AddForce(bounceVector, ForceMode2D.Impulse);

                SpawnFruit();
            }
        }
    }

    private void SpawnFruit()
    {
        int fruitCount = Random.Range(minFruit, maxFruit);

        for (int i = 0; i < fruitCount; i++)
        {
            int fruitIndex = Random.Range(0, fruits.Length);
            GameObject fruit = Instantiate(fruits[fruitIndex], transform.position, Quaternion.identity);

            Rigidbody2D fruitrb = fruit.GetComponent<Rigidbody2D>();
            fruitrb.bodyType = RigidbodyType2D.Dynamic;

            if (fruitrb!=null)
            {
                float fruitForce = 2f;
                Vector2 forceDirection = new Vector2(Random.Range(-1f,1f), Random.Range(0f,1f)).normalized;
                fruitrb.AddForce(fruitForce * forceDirection, ForceMode2D.Impulse);
            }

        }
    }

    private void BoxBreak()
    {
        Destroy(gameObject);
        GameObject partical = Instantiate(breakParticle, transform.position, Quaternion.identity);
        partical.GetComponent<ParticleSystem>().Play();
    }
}
