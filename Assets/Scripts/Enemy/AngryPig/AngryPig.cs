using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryPig : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        // 初始化patrol和chase状态
        states.Add(EnemyState.Patrol, new AngryPigPatrolState());
        states.Add(EnemyState.Chase, new AngryPigChaseState());
    }
}
