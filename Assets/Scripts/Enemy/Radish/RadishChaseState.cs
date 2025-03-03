using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadishChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
    }

    public override void LogicUpdate()
    {
        if (!currentEnemy.physicsCheck.isGround|| (currentEnemy.physicsCheck.isLeftWall && currentEnemy.faceDirection.x < 0) || (currentEnemy.physicsCheck.isRightWall && currentEnemy.faceDirection.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("Run", false);
        }

        if (!currentEnemy.wait)
        {
            currentEnemy.anim.SetBool("Run", true);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }
}
