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
    private float bulletSpeed = 10f;

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
        enemy.targetDir = (enemy.target.position - enemy.transform.position).normalized;
        // Hedef ile pozisyon aras�ndaki fark� hesaplay�n, Y eksenini s�f�rlay�n
        Vector3 targetDir = enemy.target.position - enemy.transform.position;

        // Y eksenini s�f�rlayarak sadece XZ d�zleminde hesaplama yap�yoruz
        targetDir.y = 0;

        // Y�n vekt�r�n� normalize et (birim vekt�r yap)
        targetDir = targetDir.normalized;

        // D��man� hedefe do�ru d�nd�r (sadece yatay eksende)
        enemy.transform.rotation = Quaternion.LookRotation(targetDir);

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

        _timer += Time.deltaTime;
        if (_timer > _timeBetweenShots)
        {
            _timer = 0;

            // Havuzdan mermi al
            GameObject bullet = PoolStorage.Instance.GetFromPool("bullet");
            bullet.transform.position = enemy.myWeapon.myBarrelT.position;

            // Hedefe y�nlendir
            Vector3 targetDir = (enemy.target.position - enemy.myWeapon.myBarrelT.position).normalized;
            bullet.transform.forward = targetDir;

            // Kuvvet uygula
            bullet.GetComponent<Rigidbody>().AddForce(targetDir * bulletSpeed, ForceMode.Impulse);

            // Belirli bir s�re sonra mermiyi havuza iade et
            PoolStorage.Instance.StartReturnBulletCoroutine(bullet, 3f);
        }

        if (Vector2.Distance(enemy.target.position, enemy.transform.position) > _distancetoCountExit)
        {
            _exitTimer += Time.deltaTime;
            if (_exitTimer > 0)
            {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
        }
        else
        {
            _exitTimer = 0;
        }

    }
    private IEnumerator ReturnBulletToPoolAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolStorage.Instance.ReturnToPool(bullet);
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
