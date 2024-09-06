using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{

    private Transform _playerTransform;
    private float _MovementSpeed = 1.75f;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        _playerTransform = enemy.target;
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
        base.FrameUpdate();
        // Hedefe doðru hareket et
        Vector3 direction = (target.position - enemy.transform.position).normalized;
        enemy.MoveEnemyTowardsTarget(enemy.target.gameObject, enemy.moveSpeed);

        // Eðer düþman hedefe yeterince yaklaþtýysa saldýrý durumuna geç
        if (Vector3.Distance(enemy.transform.position, target.position) < enemy.myWeapon.attackRange)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }

    }

    public override void PhysicsUpdate()
    {
        Debug.Log("+-+-+-1 = Physicsupdate");
        base.PhysicsUpdate();
    }
}
