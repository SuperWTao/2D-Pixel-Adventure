using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Debug.Log("FireHit");
        PlayerController player = collider.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Dead(transform);
        }
    }
}
