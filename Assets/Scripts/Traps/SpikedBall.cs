using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedBall : MonoBehaviour
{
    public float swingSpeed;
    public float minAngle;
    public float maxAngle;
    
    public float currentAngle;
    
    private Rigidbody2D rb;
    private HingeJoint2D hingeJoint;
    private JointMotor2D motor;
    
    private bool isSwingingForward = true;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hingeJoint = GetComponent<HingeJoint2D>();
    
        JointAngleLimits2D limits = new JointAngleLimits2D()
        {
            min = minAngle,
            max = maxAngle
        };
        hingeJoint.limits = limits;
    
        motor = new JointMotor2D
        {
            motorSpeed = swingSpeed,
            maxMotorTorque = 10000f
        };
        hingeJoint.motor = motor;
        hingeJoint.useMotor = true;
    
    }
    
    private void FixedUpdate()
    {
        currentAngle = hingeJoint.jointAngle;
    
        if (isSwingingForward && currentAngle >= minAngle)
        {
            isSwingingForward = false;
            motor.motorSpeed = -motor.motorSpeed;
            hingeJoint.motor = motor;
        }
        else if(!isSwingingForward && currentAngle <= maxAngle)
        {
            isSwingingForward = true;
            motor.motorSpeed = -motor.motorSpeed;
            hingeJoint.motor = motor;
            
        }
        
    }
}
