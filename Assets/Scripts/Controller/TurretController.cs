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
    private TurretModel turretModel;
    [HideInInspector]
    private TowerBuildManager towerBuildManager;
    [HideInInspector]
    private Transform mybuilder;

    GameObject currentTurret;

    public bool uiState;

    private Transform target;

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
        public int GizmosRange;

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

        public Transform angleX;    // X ekseninde dönen parça
        public Transform angleY;    // Y ekseninde dönen parça
        public float rotationSpeed = 5f; // Dönme hýzý

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

    public FireType selectedFireType;
    public string enemyGroupTag;

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

        if (weaponClass.onTarget)
        {
            HandleFiring(weaponClass.fireRate, Firing);
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
                rotationClass.notDeepT = distanceToEnemy;
                //average value notDeep 30f
                if (rotationClass.notDeepT > rotationClass.notDeep)
                {
                    nearestEnemy = enemy;
                    target = enemy.transform;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= weaponClass.GizmosRange)
        {

            target = nearestEnemy.transform;
            //targetEnemy = nearestEnemy.GetComponent<EnemyUnit>();
        }
        else
        {
            target = null;
        }
    }
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, weaponClass.GizmosRange);
    }
    // Gizmos ile Scene penceresinde Ray'i görselleþtirmek için

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
        turretModel = new TurretModel(gameObject.name, LevelProp.LEVEL_ONE, 0);

        Debug.Log("Turret Name: " + gameObject.name);
        Debug.Log("Level: " + turretModel.Level);
        Debug.Log("Shooting Frequency: " + turretModel.ShootingFrequency);
        Debug.Log("Damage: " + turretModel.Damage);
        Debug.Log("Range of Vision: " + turretModel.RangeOfVision);
        Debug.Log("Firing Range: " + turretModel.FiringRange);
        Debug.Log("Health: " + turretModel.Health);
        Debug.Log("Cost: " + turretModel.Cost);

        mybuilder = TowerBuildManager.builderTransform;
    }

    #endregion



    #region Trackingtarget
    private void AimAtTargetX()
    {
        // Hedefin taretle olan fark vektörünü hesapla
        Vector3 directionToTarget = target.position - rotationClass.angleX.position;

        // Y bileþenini sýfýrla, sadece yatay düzlemde çalýþ
        directionToTarget.y = 0;

        // Eðer directionToTarget sýfýr vektörü deðilse
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            // Hedefe yönelmek için gereken rotasyonu hesapla
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // X ekseninde düzgün bir þekilde dönmesi için interpolasyon (lerp) kullan
            rotationClass.angleX.rotation = Quaternion.Slerp(rotationClass.angleX.rotation, targetRotation, Time.deltaTime * rotationClass.rotationSpeed);
        }
    }

    private void AimAtTargetY()
    {
        //// Taretin gövdesi ile namlusu arasýndaki yükseklik farkýný tanýmla
        //float fark = angleY.position.y - BarrelHeightAllowance;

        //// Hedefin taretle olan fark vektörünü hesapla
        //Vector3 directionToTarget = target.position - angleY.position;

        //// Eðer directionToTarget sýfýr vektörü deðilse
        //if (directionToTarget.sqrMagnitude > 0.001f)
        //{
        //    // Yükseklik farkýný hesaba kat
        //    directionToTarget.y -= fark;

        //    // Hedefe yönelmek için gereken rotasyonu hesapla
        //    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        //    // Y ekseninde düzgün bir þekilde dönmesi için interpolasyon (lerp) kullan
        //    angleY.rotation = Quaternion.Slerp(angleY.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        //}
        // Hedefin taretle olan fark vektörünü hesapla
        Vector3 directionToTarget = target.position - rotationClass.angleY.position;

        // Eðer directionToTarget sýfýr vektörü deðilse
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            // Hedefe yönelmek için gereken rotasyonu hesapla
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Y ekseninde düzgün bir þekilde dönmesi için interpolasyon (lerp) kullan
            rotationClass.angleY.rotation = Quaternion.Slerp(rotationClass.angleY.rotation, targetRotation, Time.deltaTime * rotationClass.rotationSpeed);
        }
    }
    #endregion


    #region BulletPool
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
            rb.velocity = Vector3.zero; // Eski hýzýný sýfýrla
            rb.angularVelocity = Vector3.zero; // Eski dönme hýzýný sýfýrla
            rb.AddForce(weaponClass.barrels[barrelIndex].forward * weaponClass.shotForce);
              
        }
        else
        {
            // Eðer obje havuzunda mermi yoksa yeni bir mermi oluþtur
            GameObject newBullet = Instantiate(bulletClass.bulletPrefab, weaponClass.barrels[barrelIndex].position, weaponClass.barrels[barrelIndex].rotation);
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            rb.AddForce(weaponClass.barrels[barrelIndex].forward * weaponClass.shotForce);
            gunShot.Invoke();
              
        }       
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        bullet.SetActive(false);
        bullet.transform.SetParent(weaponClass.barrels[0]);
        bullet.transform.localPosition = Vector3.zero;
        bullet.transform.localRotation = Quaternion.identity;

        bulletClass.bullets.Add(bullet);
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
            Debug.Log(numberOfBarrels + " namludan lazer ateþleniyor");
            // Burada her bir namlu için lazer beam oluþturulabilir.
        }
    }
    #endregion

}

public enum FireType
{
    Bullet,
    Laser   
}