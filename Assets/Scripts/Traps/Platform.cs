using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Animator anim;
    public PlatformType platformType;
    public float speed;
    public Vector3 start;
    public Vector3 end;
    private Vector3 targetPosition;
    public bool on;
    private bool move;
    
    private List<Rigidbody2D> rbList = new List<Rigidbody2D>();
    private Vector3 lastPos;
    private Transform _transform;

    private void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = start;
        lastPos = transform.position;
        _transform = transform;
        if (platformType == PlatformType.Grey)
        {
            targetPosition = end;
            on = true;
        }
    }

    private void Update()
    {
        anim.SetBool("On", on);
        MovePlatform();
    }

    private void LateUpdate()
    {
        if (rbList.Count > 0)
        {
            for (int i = 0; i < rbList.Count; i++)
            {
                Rigidbody2D rb = rbList[i];
                Vector3 velocity = _transform.position - lastPos;
                rb.transform.Translate(velocity, _transform);
            }
        }
        lastPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // collision.transform.parent = transform;
            rbList.Add(collision.gameObject.GetComponent<Rigidbody2D>());
            if (platformType == PlatformType.Brown)
            {
                move = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // collision.transform.parent = null;
            rbList.Remove(collision.gameObject.GetComponent<Rigidbody2D>());
            if (platformType == PlatformType.Brown)
            {
                move = false;
            }
        }
    }

    public void MovePlatform()
    {
        if (platformType == PlatformType.Brown)
        {
            targetPosition = move ? end:start;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            on = Vector3.Distance(transform.position, targetPosition) >= 0.01f;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                if (Vector3.Distance(transform.position, start) < 0.01f)
                    targetPosition = end;
                if (Vector3.Distance(transform.position, end) < 0.01f)
                    targetPosition = start;
            }
        }
    }
}
