using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryPigChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        // Debug.Log("Chase");
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("Run", true);
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostCounter <= 0)
        {
            currentEnemy.SwitchState(EnemyState.Patrol);
        }
        
        if (!currentEnemy.physicsCheck.isGround|| (currentEnemy.physicsCheck.isLeftWall && currentEnemy.faceDirection.x < 0) || (currentEnemy.physicsCheck.isRightWall && currentEnemy.faceDirection.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDirection.x, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("Run", false);
    }
}
