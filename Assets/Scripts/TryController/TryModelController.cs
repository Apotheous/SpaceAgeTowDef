using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TryModelController : TurretModel
{
    public static TryModelController Instance { get; private set; }
    [HideInInspector]
    private TowerBuildManager towerBuildManager;
    [HideInInspector]
    private Transform mybuilder;

    public FireType selectedFireType;
    public string enemyGroupTag;

    public UnityEvent gunShot;
    public Transform target;

    GameObject currentTurret;

    public bool UiState;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        TurretModelStart();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        Information();

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
            weaponClass.Dist = Vector3.Distance(transform.position, target.position);

            weaponClass.OnVision = weaponClass.Dist < weaponClass.RangeOfVision;
            weaponClass.OnTarget = false;
            RaycastHit hit;
            if (Physics.Raycast(weaponClass.Barrels[0].position, weaponClass.Barrels[0].forward, out hit))
            {

                if (hit.transform != null && hit.transform.tag == enemyGroupTag)
                {
                    weaponClass.OnTarget = true;
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyGroupTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
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
    void OnMouseDown()
    {
        if (UiState == true)
        {
            mybuilder.GetComponent<TowerBuildManager>().DestroyCurrentTurret();
            currentTurret = Instantiate(gameObject, TowerBuildManager.builderTransform);
            mybuilder.GetComponent<TowerBuildManager>().currentTurret = currentTurret;
            mybuilder.GetComponent<TowerBuildManager>().planeClose();
            currentTurret.GetComponent<TurretController>().uiState = false;
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

    public void Damage(float damage)
    {

    }

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
                Debug.LogError(" Desteklenmeyen namlu sayýsý: " + barrelCount);
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
        //targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        //targetEnemy.Slow(slowAmount);

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
    }
    public void FireBulletFromPool(int barrelIndex)
    {

        if (bulletClass.Bullets.Count > 0)
        {
            GameObject bullet = bulletClass.Bullets[0];
            bullet.transform.SetParent(null);
            bulletClass.Bullets.RemoveAt(0);
            bullet.SetActive(true);
            gunShot.Invoke();
            // Mermiyi namlunun pozisyonuna ve rotasyonuna ayarla
            bullet.transform.position = weaponClass.Barrels[barrelIndex].position;
            bullet.transform.rotation = weaponClass.Barrels[barrelIndex].rotation;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero; // Eski hýzýný sýfýrla
            rb.angularVelocity = Vector3.zero; // Eski dönme hýzýný sýfýrla
            rb.AddForce(weaponClass.Barrels[barrelIndex].forward * weaponClass.ShotForce);

        }
        else
        {
            // Eðer obje havuzunda mermi yoksa yeni bir mermi oluþtur
            GameObject newBullet = Instantiate(bulletClass.BulletPrefab, weaponClass.Barrels[barrelIndex].position, weaponClass.Barrels[barrelIndex].rotation);
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            rb.AddForce(weaponClass.Barrels[barrelIndex].forward * weaponClass.ShotForce);
            gunShot.Invoke();

        }
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        bullet.SetActive(false);
        bullet.transform.SetParent(weaponClass.Barrels[0]);
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localRotation = Quaternion.identity;

        bulletClass.Bullets.Add(bullet);
    }

    #endregion

    #region Trackingtarget
    private void AimAtTargetX()
    {
        // Hedefin taretle olan fark vektörünü hesapla
        Vector3 directionToTarget = target.position - rotationClass.AngleX.position;

        // Y bileþenini sýfýrla, sadece yatay düzlemde çalýþ
        directionToTarget.y = 0;

        // Eðer directionToTarget sýfýr vektörü deðilse
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            // Hedefe yönelmek için gereken rotasyonu hesapla
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // X ekseninde düzgün bir þekilde dönmesi için interpolasyon (lerp) kullan
            rotationClass.AngleX.rotation = Quaternion.Slerp(rotationClass.AngleX.rotation, targetRotation, Time.deltaTime * rotationClass.RotationSpeed);
        }
    }

    private void AimAtTargetY()
    {

        // Hedefin taretle olan fark vektörünü hesapla
        Vector3 directionToTarget = target.position - rotationClass.AngleY.position;

        // Eðer directionToTarget sýfýr vektörü deðilse
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            // Hedefe yönelmek için gereken rotasyonu hesapla
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Y ekseninde düzgün bir þekilde dönmesi için interpolasyon (lerp) kullan
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

    #endregion
}
