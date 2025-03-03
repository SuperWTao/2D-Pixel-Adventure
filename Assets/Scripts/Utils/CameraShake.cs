using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    public CinemachineImpulseSource impulseSource;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
}
