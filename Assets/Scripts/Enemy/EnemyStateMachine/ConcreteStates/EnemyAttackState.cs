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

    private void BulletFiring()
    {
        Debug.Log("Bullet Firing = ");

    }
    private void BombExplosive()
    {
        Debug.Log("Bomb Exp = ");
        enemy.myWeapon.ammoPrefab.SetActive(true);
        Collider[] colliders = Physics.OverlapSphere(transform.position, myWeapon.explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == enemyGroupTag)
            {
                DamageExplosive(collider.transform);
            }
        }
    }  

    private void DamageExplosive(Transform enemy)
    {
        TryModelController e = enemy.GetComponent<TryModelController>();

        if (e != null)
        {
            e.Damage(myWeapon.damage);
            myWeapon.damage = 0;
        }
    }

    private void LaserFiring()
    {
        Debug.Log("Laser Firing = ");

    }
    public void EnemyFireType(EnemyGunnerType enmyGnrType)
    {
        switch (enmyGnrType)
        {
            case EnemyGunnerType.Bullet:
                Debug.Log("Enemy Firing = " + EnemyGunnerType.Bullet);
                BulletFiring();
                break;
            case EnemyGunnerType.Laser:
                Debug.Log("Enemy Firing = " + EnemyGunnerType.Laser);
                LaserFiring();
                break;
            case EnemyGunnerType.Bomb:
                Debug.Log("Enemy Firing = " + EnemyGunnerType.Bomb);
                BombExplosive();
                Die();
                break;  
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, myWeapon.explosionRadius);
    }
}
