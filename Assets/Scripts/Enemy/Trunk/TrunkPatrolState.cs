using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        // Debug.Log("Trunk Patrol State");
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.nomalSpeed;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(EnemyState.Chase);
        }

        if (!currentEnemy.physicsCheck.isGround ||
            (currentEnemy.physicsCheck.isLeftWall && currentEnemy.faceDirection.x < 0) ||
            (currentEnemy.physicsCheck.isRightWall && currentEnemy.faceDirection.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.anim.SetBool("Walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("Walk", true);
        }
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }
}
