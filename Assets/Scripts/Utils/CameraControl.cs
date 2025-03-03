using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }
    
    // private void Start()
    //
    // {
    //     GetNewCameraBounds();
    // }

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
        GetNewCameraBounds();
    }


    private void GetNewCameraBounds()
    {
        
        var obj = GameObject.FindGameObjectWithTag("Border");
        if (obj == null)
            return;
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
    }
}
