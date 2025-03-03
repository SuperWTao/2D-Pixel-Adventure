using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlePatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
    }

    public override void LogicUpdate()
    {
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetTrigger("Out");
    }
}
