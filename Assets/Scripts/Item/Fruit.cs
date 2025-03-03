using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public FruitType fruitType;
    public GameObject collectedEffect;
    public int score;
    
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D coll;

    private void Start()
    {
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // coll = GetComponent<CircleCollider2D>();
        coll = transform.parent.gameObject.GetComponent<CircleCollider2D>();
        spriteRenderer = transform.parent.gameObject.GetComponent<SpriteRenderer>();
    }
    
    private void OnEnable()
    {
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }

    private void OnAfterSceneLoadedEvent()
    {
        // Destroy(transform.parent.gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            UIManager.Instance.totalScore += 10;
            UIManager.Instance.UpdateTotalScore();
            spriteRenderer.enabled = false;
            coll.enabled = false;
            Destroy(transform.parent.gameObject);
            GameObject effect = Instantiate(collectedEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.3f);
        }
    }
}
