using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float bounce;
    
    private Animator anim;
    private Collider2D coll;

    private void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            anim.SetTrigger("Hit");
            PlayerController player = collider.gameObject.GetComponent<PlayerController>();
            player.Bounce(bounce);
        }
    }

    public void DestoryAfterAnimation()
    {
        Destroy(gameObject);
    }
}
