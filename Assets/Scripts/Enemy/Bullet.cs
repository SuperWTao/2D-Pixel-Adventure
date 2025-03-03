using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    public GameObject bulletPieces;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // 播放动画，销毁对象
        // 播放粒子动画
        GameObject particles = Instantiate(bulletPieces, transform.position, Quaternion.identity);
        ParticleSystem particleSystem = particles.GetComponent<ParticleSystem>();
        particleSystem.Play();
        if (collision.gameObject.CompareTag("Player"))
        {
            // Debug.Log("命中");
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Dead(transform);
        }
        Destroy(gameObject);
        
    }
    
    /// <summary>
    /// 场景加载完成之后，销毁上一个场景中还在飞行的子弹
    /// </summary>
    private void OnAfterSceneLoadedEvent()
    {
        Destroy(gameObject);
    }
}
