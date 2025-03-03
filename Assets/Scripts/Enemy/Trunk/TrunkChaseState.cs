using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        Debug.Log("Trunk Chase");
        currentEnemy = enemy;
        currentEnemy.anim.SetBool("Run", true);
        // currentEnemy.rb.velocity = Vector2.zero;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
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
