using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{

    private Transform _towerTransform;
    private float _MovementSpeed = 1.75f;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        _towerTransform = enemy.target;
        Debug.Log("+-+-+-1 = EnemyChaseState" );
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        Debug.Log("+-+-+-1 = AnimatinTrigger");
    }

    public override void EnterState()
    {
        Debug.Log("+-+-+-1= EnterState");
        base.EnterState();
    }

    public override void ExitState()
    {
        Debug.Log("+-+-+-1 = ExitState" );
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        Debug.Log("+-+-+-1 = FrameUpdate");
        // base.FrameUpdate();
        // Hedefe doðru hareket et
        if (enemy.target == null)
        {
            Debug.LogWarning("No target in Chase State, switching to IdleState.");
            StateMachine.ChangeState(enemy.IdleState);
            return;
        }
       
        enemy.MoveEnemyTowardsTarget(enemy.target.gameObject, enemy.moveSpeed);
    }

    public override void PhysicsUpdate()
    {
        Debug.Log("+-+-+-1 = Physicsupdate");
        base.PhysicsUpdate();
    }
}
