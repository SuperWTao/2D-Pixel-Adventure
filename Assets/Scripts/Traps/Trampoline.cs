using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private Animator anim;
    public float bounceStrengh = 10f;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("Jump");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bounceStrengh), ForceMode2D.Impulse);
        }
    }
    
}
