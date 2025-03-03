using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    private Animator anim;
    
    public string sceneFrom;
    public string sceneToGo;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerController player = collider.GetComponent<PlayerController>();
        if (player != null)
        {
            // Debug.Log("victory");
            anim.SetBool("out", true);
            
            TransitionManager.Instance.Transition(sceneFrom, sceneToGo);
        }
    }

    public void SetAnimationAfterOut()
    {
        anim.SetBool("Idle", true);
    }
    
    
}
