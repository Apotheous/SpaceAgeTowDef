using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UIElements;


public class TryModelController : TurretModel, IDamageable, IClickable
{
    public Transform target;
    public float MaxHealth { get ; set ; }
    public float CurrentHealth { get ; set ; }

    [HideInInspector]
    private TowerBuildManager towerBuildManager;
    [HideInInspector]
    private Transform mybuilder;

    public FireType selectedFireType;
    public string enemyGroupTag;

    public UnityEvent gunShot;


    GameObject currentTurret;

    [Header("Unity Stuff")]
    public UnityEngine.UI.Image healthBar;

    public bool UiState;


    //public List<GameObject> enemies;

    GameObject nearestEnemy;

    


    void Start()
    {
 
        TurretModelStart();
        
        MaxHealth = Health;
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        Information();
        UpdateTarget();
        if (weaponClass.OnTarget)
        {
            HandleFiring(weaponClass.FireRate, Firing);
        }
        else
        {
            if (laserClass.UseLaser)
            {
                if (laserClass.lineRenderer.enabled)
                {
                    laserClass.lineRenderer.enabled = false;
                    laserClass.impactEffect.Stop();
                    laserClass.impactLight.enabled = false;
                }
            }
        }

        if (target != null && weaponClass.OnVision)
        {
            AimAtTargetX();

            AimAtTargetY();
        }
    }

    private void Information()
    {
        if (target != null)
        {
            Collider targetCollider = target.GetComponent<Collider>();

            Vector3 targetPosition = targetCollider.bounds.center; // Collider merkezini hedef olarak kullan�r
            weaponClass.Dist = Vector3.Distance(transform.position, targetPosition);


            //weaponClass.Dist = Vector3.Distance(transform.position, target.position);
            
            weaponClass.OnVision = weaponClass.Dist < weaponClass.RangeOfVision;
            weaponClass.OnTarget = false;
            RaycastHit hit;
            Vector3 rayOrigin = weaponClass.Barrels[0].position;
            Vector3 rayDirection = weaponClass.Barrels[0].forward * weaponClass.RangeOfVision;
            if (Physics.Raycast(weaponClass.Barrels[0].position, weaponClass.Barrels[0].forward, out hit, weaponClass.RangeOfVision))
            {
                Debug.DrawRay(rayOrigin, rayDirection, Color.blue);
                if (hit.transform != null && hit.transform.tag == enemyGroupTag)
                {
                    Debug.DrawRay(rayOrigin, rayDirection, Color.green);
                    if (weaponClass.Dist < weaponClass.FiringRange)
                    {
                        Debug.DrawRay(rayOrigin, rayDirection, Color.red);
                        weaponClass.OnTarget = true;
                    }
                }
            }
        }
    }
    public void HandleFiring(float fireRate, System.Action firingAction)
    {
        weaponClass.Timer += Time.deltaTime;

        if (weaponClass.Timer >= fireRate)
        {
            firingAction.Invoke();
            weaponClass.Timer = 0f;
        }
    }
    //garbage && garbage collecter
    void UpdateTarget()
    {
        
        //enemies = GameObject.FindGameObjectsWithTag(enemyGroupTag);
        float shortestDistance = Mathf.Infinity;
        nearestEnemy = null;
        foreach (GameObject enemy in EnemyMainBase.instance.myUnitList)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                rotationClass.NotDeepT = distanceToEnemy;
                //average value notDeep 30f
                if (rotationClass.NotDeepT > rotationClass.NotDeep)
                {
                    nearestEnemy = enemy;
                    target = enemy.transform;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= weaponClass.RangeOfVision)
        {

            target = nearestEnemy.transform;
            //targetEnemy = nearestEnemy.GetComponent<EnemyUnit>();
        }
        else
        {
            target = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weaponClass.RangeOfVision);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<TowerMan>() != null)
        {
            collider.gameObject.GetComponent<TowerMan>().currentTurret = gameObject;
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer("UiObjects"))
        {
            Debug.Log("+++++" + collider.name);
        }
    }
    #region IDamageable
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        healthBar.fillAmount = CurrentHealth / MaxHealth;
        if (CurrentHealth <= 0)
        {
            Debug.Log("Turret+-+- Damage");
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Turret+-+- die");
        //StateMachine.ChangeState(DieState);
        Destroy(this.gameObject, 2.5f);
    }

    #endregion


    #region Fire Funcs
    void Firing()
    {
        if (!UiState) Fire(weaponClass.Barrels.Count, selectedFireType);
    }

    public void Fire(int barrelCount, FireType fireType)
    {

        switch (barrelCount)
        {
            case 1:
                FireOneBarrel(fireType);
                break;
            case 2:
                FireTwoBarrels(fireType);
                break;
            case 4:
                FireFourBarrels(fireType);
                break;
            default:
                Debug.LogError("Desteklenmeyen namlu say�s�: " + barrelCount);
                break;
        }
    }

    public void FireOneBarrel(FireType fireType)
    {
        switch (fireType)
        {
            case FireType.Bullet:
                FireBulletFromPool(0);
                break;
            case FireType.Laser:
                FireLaser(1);
                break;
        }
    }

    public void FireTwoBarrels(FireType fireType)
    {
        switch (fireType)
        {
            case FireType.Bullet:
                if (weaponClass.BarrelTimer == 0)
                {
                    FireBulletFromPool(0);
                }
                weaponClass.BarrelTimer += weaponClass.BarrelTimerRate;
                if (weaponClass.BarrelTimer >= weaponClass.BarrelTimerLine)
                {
                    FireBulletFromPool(1);
                    weaponClass.BarrelTimer = 0;
                }
                break;
            case FireType.Laser:
                FireLaser(2);
                break;
        }
    }

    public void FireFourBarrels(FireType fireType)
    {
        switch (fireType)
        {
            case FireType.Bullet:
                if (weaponClass.BarrelTimer == 0)
                {
                    FireBulletFromPool(0);
                    FireBulletFromPool(1);
                }
                weaponClass.BarrelTimer += weaponClass.BarrelTimerRate;
                if (weaponClass.BarrelTimer >= weaponClass.BarrelTimerLine)
                {
                    FireBulletFromPool(2);
                    FireBulletFromPool(3);
                    weaponClass.BarrelTimer = 0;
                }
                break;
            case FireType.Laser:
                FireLaser(4);
                break;
        }
    }

    private void FireLaser(int numberOfBarrels)
    {
        for (int i = 0; i < numberOfBarrels; i++)
        {
            Laser();
        }
    }
    #endregion

    #region BulletPool
    void Laser()
    {
        if (!laserClass.lineRenderer.enabled)
        {
            laserClass.lineRenderer.enabled = true;
            laserClass.impactEffect.Play();
            laserClass.impactLight.enabled = true;
        }

        laserClass.lineRenderer.SetPosition(0, weaponClass.Barrels[0].position);
        laserClass.lineRenderer.SetPosition(1, target.position);

        Vector3 dir = weaponClass.Barrels[0].position - target.position;

        laserClass.impactEffect.transform.position = target.position + dir.normalized;

        laserClass.impactEffect.transform.rotation = Quaternion.LookRotation(dir);

        if (target==null)
        {
            StopLaser();
        }
    }

    void StopLaser()
    {
        if (laserClass.lineRenderer.enabled)
        {
            laserClass.lineRenderer.enabled = false;
            laserClass.impactEffect.Stop();
            laserClass.impactLight.enabled = false;

            // ImpactEffect'in pozisyonunu ve rotasyonunu ba�lang�� haline getirmek i�in
            laserClass.impactEffect.transform.position = Vector3.zero;
            laserClass.impactEffect.transform.rotation = Quaternion.identity;
        }
    }
    public void FireBulletFromPool(int barrelIndex)
    {
        bulletClass.myBullet = bulletClass.pool.GetFromPool();

        gunShot.Invoke();

        bulletClass.myBulletRb = bulletClass.myBullet.GetComponent<Rigidbody>();

        bulletClass.myBulletRb.AddForce(weaponClass.Barrels[barrelIndex].forward * weaponClass.ShotForce);
        
        bulletClass.myBullet.SetActive(true);
        bulletClass.pool.StartReturnBulletCoroutine(bulletClass.myBullet, 5f);
    }

    #endregion

    #region Trackingtarget
    private void AimAtTargetX()
    {
        Vector3 directionToTarget = target.position - rotationClass.AngleX.position;

        directionToTarget.y = 0;

        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            rotationClass.AngleX.rotation = Quaternion.Slerp(rotationClass.AngleX.rotation, targetRotation, Time.deltaTime * rotationClass.RotationSpeed);
        }
    }

    private void AimAtTargetY()
    {
        Vector3 directionToTarget = target.position - rotationClass.AngleY.position;

        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            rotationClass.AngleY.rotation = Quaternion.Slerp(rotationClass.AngleY.rotation, targetRotation, Time.deltaTime * rotationClass.RotationSpeed);
        }
    }
    #endregion

    #region StartFoncs
    private void TurretModelStart()
    {
        mybuilder = TowerBuildManager.builderTransform;

        if (laserClass.UseLaser)
        {
            if (laserClass.lineRenderer.enabled)
            {
                laserClass.lineRenderer.enabled = false;
                laserClass.impactEffect.Stop();
                laserClass.impactLight.enabled = false;
            }
        }
    }


    public void ICilkable()
    {
        if (UiState == true)
        {
            mybuilder.GetComponent<TowerBuildManager>().DestroyCurrentTurret();
            currentTurret = Instantiate(gameObject, TowerBuildManager.builderTransform);
            mybuilder.GetComponent<TowerBuildManager>().currentTurret = currentTurret;
            mybuilder.GetComponent<TowerBuildManager>().planeClose();
            currentTurret.GetComponent<TryModelController>().UiState = false;
        }
    }

    #endregion
}
