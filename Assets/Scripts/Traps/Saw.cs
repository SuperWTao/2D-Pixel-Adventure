using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float speed; // 齿轮移动的速度
    public Vector3[] direction;
    private int currentDirection = 0;

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if(direction.Length == 0)
            return;
        Vector3 targetDirection = direction[currentDirection];
        Vector3 movementDirection = targetDirection - transform.position;
        float distance = movementDirection.magnitude;
        
        // 如果距离小于速度乘以每帧时间，则认为已到达该途径点
        if (distance < speed*Time.deltaTime)
        {
            currentDirection = (currentDirection + 1) % direction.Length;
        }
        else
        {
            transform.position += movementDirection.normalized * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerController player = collider.gameObject.GetComponent<PlayerController>();
            if (player!=null)
            {
                player.Dead(transform);
            }
        }
    }
}
