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

    [HideInInspector]
    private TowerBuildManager towerBuildManager;
    [HideInInspector]
    private Transform mybuilder;

    public string enemyGroupTag;

    public UnityEvent gunShot;


    GameObject currentTurret;

    [Header("Unity Stuff")]
    public UnityEngine.UI.Image healthBar;

    public bool UiState;

    GameObject nearestEnemy;
    void Start()
    {
        TurretModelStart();
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

        if (target != null )
        {
            AimAtTargetX();

            AimAtTargetY();
        }
    }
    private void Information()
    {
        if (target != null)
        {
            // Hedefin Collider bileşenini al
            Collider targetCollider = target.GetComponent<Collider>();

            // Hedef pozisyonunu belirle
            Vector3 targetPosition = targetCollider.bounds.center;

            // Taret ile hedef arasındaki mesafeyi hesapla
            weaponClass.Dist = Vector3.Distance(transform.position, targetPosition);

            // Başlangıçta hedefte olmadığını varsay
            weaponClass.OnTarget = false;

            // Raycast için başlangıç pozisyonu ve yönü belirle
            RaycastHit hit;
            Vector3 rayOrigin = weaponClass.Barrels[0].position;
            Vector3 rayDirection = weaponClass.Barrels[0].forward * weaponClass.FiringRange;

            // Mavi ışını her zaman çiz
            Debug.DrawRay(rayOrigin, rayDirection, Color.blue);

            // Raycast kontrolü
            if (Physics.Raycast(rayOrigin, weaponClass.Barrels[0].forward, out hit, weaponClass.FiringRange))
            {
                // Eğer ışın bir düşmana çarptıysa
                if (hit.transform != null && hit.transform.tag == enemyGroupTag)
                {
                    // Hedef ateş menzili içerisindeyse kırmızı ışını çiz ve hedefe kilitlen
                    Debug.DrawRay(rayOrigin, rayDirection, Color.red);
                    weaponClass.OnTarget = true;
                }
                else
                {
                    // Hedef ateş menzilinde değilse yeşil ışını çiz
                    Debug.DrawRay(rayOrigin, rayDirection, Color.green);
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
  
    void UpdateTarget()
    {
        
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

        if (nearestEnemy != null && shortestDistance <= weaponClass.FiringRange)
        {

            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weaponClass.FiringRange);
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
        currentHealth -= damageAmount;
        healthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject, 2.5f);
    }

    #endregion


    #region Fire Funcs
    void Firing()
    {
        if (!UiState) Fire(weaponClass.Barrels.Count, GunnerType);
    }

    public void Fire(int barrelCount, GunnerType fireType)
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

    public void FireOneBarrel(GunnerType fireType)
    {
        switch (fireType)
        {
            case GunnerType.GUNNER:
                FireBulletFromPool(0);
                break;
            case GunnerType.LASER:
                FireLaser(1);
                break;
        }
    }

    public void FireTwoBarrels(GunnerType fireType)
    {
        switch (fireType)
        {
            case GunnerType.GUNNER:
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
            case GunnerType.LASER:
                FireLaser(2);
                break;
        }
    }

    public void FireFourBarrels(GunnerType fireType)
    {
        switch (fireType)
        {
            case GunnerType.GUNNER:
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
            case GunnerType.LASER:
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
    public void FireBulletFromPool(int barrelIndex)
    {
        bulletClass.myBullet = bulletClass.pool.GetFromPool();
        bulletClass.myBullet.transform.position = weaponClass.Barrels[barrelIndex].position;
        gunShot.Invoke();

        bulletClass.myBulletRb = bulletClass.myBullet.GetComponent<Rigidbody>();

        bulletClass.myBulletRb.AddForce(weaponClass.Barrels[barrelIndex].forward * weaponClass.ShotForce);

        bulletClass.myBullet.SetActive(true);
        bulletClass.pool.StartReturnBulletCoroutine(bulletClass.myBullet, 5f);
    }
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


    #endregion

    #region Trackingtarget
    private void AimAtTargetX()
    {
        Collider targetCollider = target.GetComponent<Collider>();
        if (targetCollider == null) return;
        Vector3 closestPoint = targetCollider.ClosestPoint(rotationClass.AngleX.position);
        Vector3 directionToTarget = closestPoint - rotationClass.AngleX.position;
        directionToTarget.y = 0;

        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            rotationClass.AngleX.rotation = Quaternion.Slerp(
                rotationClass.AngleX.rotation,
                targetRotation,
                Time.deltaTime * rotationClass.RotationSpeed
            );
        }
    }

    private void AimAtTargetY()
    {
        Collider targetCollider = target.GetComponent<Collider>();
        if (targetCollider == null) return;

        Vector3 closestPoint = targetCollider.ClosestPoint(rotationClass.AngleY.position);
        Vector3 directionToTarget = closestPoint - rotationClass.AngleY.position;

        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            rotationClass.AngleY.rotation = Quaternion.Slerp(
                rotationClass.AngleY.rotation,
                targetRotation,
                Time.deltaTime * rotationClass.RotationSpeed
            );
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
