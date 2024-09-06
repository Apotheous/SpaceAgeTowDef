using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : EnemyState
{

    private Vector3 _targetPos;
    private Vector3 _direction;
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.moveSpeed = 2f;
        _targetPos = GetRandomPointInCircle();
        enemy.ChangeAnimationState(enemy.animatoinClass.ENEMY_WALK_FRONT);
        Debug.Log("++++++-----------EnterState");
    }

    private Vector3 GetRandomPointInCircle()
    {
        return enemy.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * enemy.RandomMovementrange;
        
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("++++++-----------ExitState");
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Debug.Log("++++++-----------FrameUpdate");
        
        if (enemy.moveSpeed > 0)
        {
            enemy.MoveEnemyTowardsTarget(enemy.gameObject, enemy.moveSpeed);
        }

        
        if ((enemy.transform.position - _targetPos).sqrMagnitude < 0.01f)
        {
            _targetPos = GetRandomPointInCircle();
        }

        
        if (enemy.moveSpeed > 0)
        {
            enemy.ChangeAnimationState(enemy.animatoinClass.ENEMY_WALK_FRONT);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Debug.Log("++++++-----------PhysicUpdate");
    }
}
