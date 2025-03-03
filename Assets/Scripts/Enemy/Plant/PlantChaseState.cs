using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.anim.SetBool("Run", true);
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
    }

    public override void LogicUpdate()
    {
        if (currentEnemy.lostCounter <= 0)
        {
            currentEnemy.SwitchState(EnemyState.Patrol);
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
