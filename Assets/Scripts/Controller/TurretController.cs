using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class TurretController : MonoBehaviour
{
    public static TurretController Instance { get; private set; }

    [HideInInspector]
    public TurretModel turretModel;
    [HideInInspector]
    private TowerBuildManager towerBuildManager;
    [HideInInspector]
    private Transform mybuilder;

    public FireType selectedFireType;
    public string enemyGroupTag;

    GameObject currentTurret;

    public bool uiState;

    public Transform target;

    [System.Serializable]
    public class WeaponClass
    {
        public List<Transform> barrels = new List<Transform>();
        public int shotForce;

        public float visionRange;
        public float firingRange;
        public float dist;

        public bool onVision;
        public bool onTarget;

        public float Timer;
        public float fireRate;


        public float barrelTimer;
        public float barrelTimerRate;
        public float barrelTimerLine;

    }

    [System.Serializable]
    public class BulletClass
    {
        public List<GameObject> bullets = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> bulletPool = new List<GameObject>();//--????
        public GameObject bulletPrefab;
    }

    [System.Serializable]
    public class RotationClass
    {

        public Transform angleX;    // X ekseninde d�nen par�a
        public Transform angleY;    // Y ekseninde d�nen par�a
        public float rotationSpeed = 5f; // D�nme h�z�

        public float notDeep;
        public float notDeepT;
        public float BarrelHeightAllowance;
    }


    [System.Serializable]
    public class LaserClass
    {
        [Header("Use Laser")]
        public bool useLaser = false;

        public int damageOverTime = 30;
        public float slowAmount = .5f;

        public LineRenderer lineRenderer;
        public ParticleSystem impactEffect;
        public Light impactLight;

    }

    public WeaponClass weaponClass;
    public BulletClass bulletClass;
    public RotationClass rotationClass;
    public LaserClass laserClass;

    public UnityEvent gunShot;



    private void Awake() // al�nd� ==================================
    {
        Instance = this;
    }

    void Start()
    {
        TurretModelStart();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    } // al�nd� ==================================

    private void Update()
    {
        Information();

        if (weaponClass.onTarget)
        {
            HandleFiring(weaponClass.fireRate, Firing);
        }
        else
        {
            if (laserClass.useLaser)
            {
                if (laserClass.lineRenderer.enabled)
                {
                    laserClass.lineRenderer.enabled = false;
                    laserClass.impactEffect.Stop();
                    laserClass.impactLight.enabled = false;
                }
            }
        }

        if (target != null && weaponClass.onVision)
        {
            AimAtTargetX();

            AimAtTargetY();
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

    private void Information()
    {
        if (target != null)
        {
            weaponClass.dist = Vector3.Distance(transform.position, target.position);

            weaponClass.onVision = weaponClass.dist < weaponClass.visionRange;
            weaponClass.onTarget = false;
            RaycastHit hit;
            if (Physics.Raycast(weaponClass.barrels[0].position, weaponClass.barrels[0].forward, out hit))
            {
                
                if (hit.transform != null && hit.transform.tag == enemyGroupTag)
                {
                    weaponClass.onTarget = true;
                }
            }
        }
        #region OldInformation
        /*
        private void Information()
        {

            if (target != null)
            {
                weaponClass.dist = Vector3.Distance(transform.position, target.position);
                if (weaponClass.dist < weaponClass.visionRange)
                {
                    weaponClass.onVision = true;
                }
                else
                {
                    weaponClass.onVision = false;
                }
                if (weaponClass.dist < weaponClass.firingRange)
                {
                    weaponClass.onTarget = true;
                }
                else
                {
                    weaponClass.onTarget = false;
                }
            }

        }
        */
        #endregion
    }//----------
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
                rotationClass.notDeepT = distanceToEnemy;
                //average value notDeep 30f
                if (rotationClass.notDeepT > rotationClass.notDeep)
                {
                    nearestEnemy = enemy;
                    target = enemy.transform;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= weaponClass.visionRange)
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
        if (uiState == true)
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
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, weaponClass.visionRange);
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
    #region StartFoncs
    private void TurretModelStart()
    {

   

        mybuilder = TowerBuildManager.builderTransform;

        if (laserClass.useLaser)
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



    #region Trackingtarget
    private void AimAtTargetX()
    {
        // Hedefin taretle olan fark vekt�r�n� hesapla
        Vector3 directionToTarget = target.position - rotationClass.angleX.position;

        // Y bile�enini s�f�rla, sadece yatay d�zlemde �al��
        directionToTarget.y = 0;

        // E�er directionToTarget s�f�r vekt�r� de�ilse
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            // Hedefe y�nelmek i�in gereken rotasyonu hesapla
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // X ekseninde d�zg�n bir �ekilde d�nmesi i�in interpolasyon (lerp) kullan
            rotationClass.angleX.rotation = Quaternion.Slerp(rotationClass.angleX.rotation, targetRotation, Time.deltaTime * rotationClass.rotationSpeed);
        }
    }

    private void AimAtTargetY()
    {
        //// Taretin g�vdesi ile namlusu aras�ndaki y�kseklik fark�n� tan�mla
        //float fark = angleY.position.y - BarrelHeightAllowance;

        //// Hedefin taretle olan fark vekt�r�n� hesapla
        //Vector3 directionToTarget = target.position - angleY.position;

        //// E�er directionToTarget s�f�r vekt�r� de�ilse
        //if (directionToTarget.sqrMagnitude > 0.001f)
        //{
        //    // Y�kseklik fark�n� hesaba kat
        //    directionToTarget.y -= fark;

        //    // Hedefe y�nelmek i�in gereken rotasyonu hesapla
        //    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        //    // Y ekseninde d�zg�n bir �ekilde d�nmesi i�in interpolasyon (lerp) kullan
        //    angleY.rotation = Quaternion.Slerp(angleY.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        //}
        // Hedefin taretle olan fark vekt�r�n� hesapla
        Vector3 directionToTarget = target.position - rotationClass.angleY.position;

        // E�er directionToTarget s�f�r vekt�r� de�ilse
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            // Hedefe y�nelmek i�in gereken rotasyonu hesapla
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Y ekseninde d�zg�n bir �ekilde d�nmesi i�in interpolasyon (lerp) kullan
            rotationClass.angleY.rotation = Quaternion.Slerp(rotationClass.angleY.rotation, targetRotation, Time.deltaTime * rotationClass.rotationSpeed);
        }
    }
    #endregion

    #region Fire Funcs
    void Firing()
    {
        if (!uiState) Fire(weaponClass.barrels.Count, selectedFireType);
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
                Debug.LogError(" Desteklenmeyen namlu say�s�: " + barrelCount);
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
                if (weaponClass.barrelTimer == 0)
                {
                    FireBulletFromPool(0);
                }
                weaponClass.barrelTimer += weaponClass.barrelTimerRate;
                if (weaponClass.barrelTimer >= weaponClass.barrelTimerLine)
                {
                    FireBulletFromPool(1);
                    weaponClass.barrelTimer = 0;
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
            if (weaponClass.barrelTimer == 0)
            {
                FireBulletFromPool(0);
                FireBulletFromPool(1);
            }
            weaponClass.barrelTimer += weaponClass.barrelTimerRate;
            if (weaponClass.barrelTimer >= weaponClass.barrelTimerLine)
            {
                FireBulletFromPool(2);
                FireBulletFromPool(3);
                weaponClass.barrelTimer = 0;
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

        laserClass.lineRenderer.SetPosition(0, weaponClass.barrels[0].position);
        laserClass.lineRenderer.SetPosition(1, target.position);

        Vector3 dir = weaponClass.barrels[0].position - target.position;

        laserClass.impactEffect.transform.position = target.position + dir.normalized;

        laserClass.impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }
    public void FireBulletFromPool(int barrelIndex)
    {

        if (bulletClass.bullets.Count > 0)
        {
            GameObject bullet = bulletClass.bullets[0];
            bullet.transform.SetParent(null);
            bulletClass.bullets.RemoveAt(0);
            bullet.SetActive(true);
            gunShot.Invoke();
            // Mermiyi namlunun pozisyonuna ve rotasyonuna ayarla
            bullet.transform.position = weaponClass.barrels[barrelIndex].position;
            bullet.transform.rotation = weaponClass.barrels[barrelIndex].rotation;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero; // Eski h�z�n� s�f�rla
            rb.angularVelocity = Vector3.zero; // Eski d�nme h�z�n� s�f�rla
            rb.AddForce(weaponClass.barrels[barrelIndex].forward * weaponClass.shotForce);

        }
        else
        {
            // E�er obje havuzunda mermi yoksa yeni bir mermi olu�tur
            GameObject newBullet = Instantiate(bulletClass.bulletPrefab, weaponClass.barrels[barrelIndex].position, weaponClass.barrels[barrelIndex].rotation);
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            rb.AddForce(weaponClass.barrels[barrelIndex].forward * weaponClass.shotForce);
            gunShot.Invoke();

        }
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        bullet.SetActive(false);
        bullet.transform.SetParent(weaponClass.barrels[0]);
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localRotation = Quaternion.identity;

        bulletClass.bullets.Add(bullet);
    }

    #endregion
}

public enum FireType
{
    Bullet,
    Laser   
}