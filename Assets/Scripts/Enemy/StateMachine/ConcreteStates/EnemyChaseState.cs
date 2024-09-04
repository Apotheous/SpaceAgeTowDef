using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{

    private Transform _playerTransform;
    private float _MovementSpeed = 1.75f;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        //_playerTransform = GameObject.FindWithTag("Player").transform;
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();

        //Debug.Log("hello from the chase state");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        //Vector3 moveDirection = (_playerTransform.position - enemy.transform.position).normalized;
        //enemy.MoveEnemy(2f);//moveDirection * _MovementSpeed

        //if (enemy.IsWithinStrikeingDistance)
        //{
        //    enemy.StateMachine.ChangeState(enemy.AttackState);
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
