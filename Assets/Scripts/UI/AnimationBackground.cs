using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBackground : MonoBehaviour
{
    Material material;
    private Vector2 movement;

    public Vector2 speed;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        movement += speed * Time.deltaTime;
        material.mainTextureOffset = movement;
    }
}
