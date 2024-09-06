using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private Transform _playerTransform;

    private float _timer;
    private float _timeBetweenShots = 2f;

    private float _exitTimer;
    private float _timeTillExit;
    private float _distancetoCountExit = 3f;
    private float bulletSpeed;

    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {

    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Debug.Log("GPT Yapýyor Bu sporu");

        //enemy.MoveEnemy(0);
        //if (_timer >_timeBetweenShots)
        //{
        //    _timer = 0;
        //    Vector3 dir = (_playerTransform.position - enemy.transform.position).normalized;
        //    Rigidbody bullet = GameObject.Instantiate(enemy.bulletPrefab, enemy.transform.position, Quaternion.identity);
        //   bullet.velocity = dir * bulletSpeed;
        //}
        //if (Vector2.Distance(_playerTransform.position, enemy.transform.position) > _distancetoCountExit) 
        //{
        //    _exitTimer += Time.deltaTime;
        //    if (_exitTimer > 0)
        //    {
        //        enemy.StateMachine.ChangeState(enemy.ChaseState);
        //    }
        //}
        //else
        //{
        //    _exitTimer = 0;
        //}
        //_timer += Time.deltaTime;

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
