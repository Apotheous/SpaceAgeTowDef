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
        Debug.Log("AttackStateEnemy");
        enemy.ChangeAnimationState(enemy.animatoinClass.ENEMY_SHOOT_AUTO);
        EnemyFireType(enemy.myWeapon.enemy_Gnnr_Type);
    }

    public override void ExitState()
    {
        base.ExitState();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Debug.Log("AttackStateEnemy");
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

    private void BulletFiring()
    {

    }
    private void BombExplosive()
    {
        Debug.Log("Spider bomb = ");
        enemy.myWeapon.ammoPrefab.SetActive(true);
        Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, enemy.myWeapon.explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == enemy.enemyGroupTag && collider.GetComponent<TryModelController>())
            {
                DamageExplosive(collider.transform);
                Debug.Log("Spider bomb = " + collider.name);
            }
        }
    }  

    private void DamageExplosive(Transform enemyObj)
    {
        TryModelController e = enemyObj.GetComponent<TryModelController>();

        if (e != null)
        {
            e.Damage(enemy.myWeapon.damage);
            enemy.Die();
            enemy.myWeapon.damage = 0;
        }
    }

    private void LaserFiring()
    {

    }
    public void EnemyFireType(EnemyGunnerType enmyGnrType)
    {
        switch (enmyGnrType)
        {
            case EnemyGunnerType.Bullet:

                BulletFiring();
                break;
            case EnemyGunnerType.Laser:

                LaserFiring();
                break;
            case EnemyGunnerType.Bomb:

                BombExplosive();
                
                break;  
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, myWeapon.explosionRadius);
    }
}
