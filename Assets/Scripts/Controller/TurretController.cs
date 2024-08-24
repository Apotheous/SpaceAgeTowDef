using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretController : MonoBehaviour
{
    public static TurretController Instance { get; private set; }
    private TurretModel turretModel;
    public TowerBuildManager towerBuildManager;
    public Transform mybuilder;

    GameObject currentTurret;
    int previusCount;
    public bool uiState;

    public List<Transform> barrels = new List<Transform>();
    public GameObject bulletPrefab;
    public List<GameObject> bulletPool = new List<GameObject>(); // Obje havuzu

    public GameObject targetGamObject;
    public Transform target;    // Düþman hedef
    public Transform angleX;    // X ekseninde dönen parça
    public Transform angleY;    // Y ekseninde dönen parça
    public float rotationSpeed = 5f; // Dönme hýzý

    public List<GameObject> bullets = new List<GameObject>();


    public float visionRange;
    public float firingRange;
    public float dist;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        TurretModelStart();
    }

    private void Update()
    {
        dist = Vector3.Distance(target.position, transform.position);
        if (Input.GetKeyDown(KeyCode.X))
        {
            Firing();
        }

        if (target != null)
        {
            AimAtTargetX();

            AimAtTargetY();
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
        previusCount = mybuilder.childCount;

        targetGamObject = GameObject.FindWithTag("Enemy");
        target = targetGamObject.transform;
    }

    #endregion


    #region TargetFallow
    private void AimAtTargetX()
    {
        // Hedefin taretle olan fark vektörünü hesapla
        Vector3 directionToTarget = target.position - angleX.position;

        // Y bileþenini sýfýrla, sadece yatay düzlemde çalýþ
        directionToTarget.y = 0;

        // Eðer directionToTarget sýfýr vektörü deðilse
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            // Hedefe yönelmek için gereken rotasyonu hesapla
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // X ekseninde düzgün bir þekilde dönmesi için interpolasyon (lerp) kullan
            angleX.rotation = Quaternion.Slerp(angleX.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void AimAtTargetY()
    {
        // Hedefin taretle olan fark vektörünü hesapla
        Vector3 directionToTarget = target.position - angleY.position;

        // Eðer directionToTarget sýfýr vektörü deðilse
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            // Hedefe yönelmek için gereken rotasyonu hesapla
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Y ekseninde düzgün bir þekilde dönmesi için interpolasyon (lerp) kullan
            angleY.rotation = Quaternion.Slerp(angleY.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    #endregion


    #region BulletPool
    public void FireBulletFromPool(int barrelIndex)
    {
        if (bullets.Count > 0)
        {
            GameObject bullet = bullets[0];
            bullets.RemoveAt(0);
            bullet.SetActive(true);

            // Mermiyi namlunun pozisyonuna ve rotasyonuna ayarla
            bullet.transform.position = barrels[barrelIndex].position;
            bullet.transform.rotation = barrels[barrelIndex].rotation;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero; // Eski hýzýný sýfýrla
            rb.angularVelocity = Vector3.zero; // Eski dönme hýzýný sýfýrla
            rb.AddForce(barrels[barrelIndex].forward * 5000f);
        }
        else
        {
            // Eðer obje havuzunda mermi yoksa yeni bir mermi oluþtur
            GameObject newBullet = Instantiate(bulletPrefab, barrels[barrelIndex].position, barrels[barrelIndex].rotation);
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrels[barrelIndex].forward * 5000f);
        }
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero; // Mevcut hýzýný sýfýrla
        rb.angularVelocity = Vector3.zero; // Mevcut dönme hýzýný sýfýrla

        bullet.SetActive(false);
        bullet.transform.SetParent(barrels[0]);
        bullet.transform.localPosition = Vector3.zero; // Pozisyonu sýfýrla
        bullet.transform.localRotation = Quaternion.identity; // Rotasyonu sýfýrla

        bullets.Add(bullet);
    }

    #endregion

    #region Fire Funcs
    void Firing()
    {
        if (!uiState) Fire(barrels.Count, FireType.Bullet);
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
                Debug.LogError("Desteklenmeyen namlu sayýsý: " + barrelCount);
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
        for (int i = 0; i < 2; i++)
        {
            switch (fireType)
            {
                case FireType.Bullet:
                    FireBulletFromPool(i);
                    break;
                case FireType.Laser:
                    FireLaser(2);
                    break;
            }
        }
    }

    public void FireFourBarrels(FireType fireType)
    {
        for (int i = 0; i < 4; i++)
        {
            switch (fireType)
            {
                case FireType.Bullet:
                    FireBulletFromPool(i);
                    break;
                case FireType.Laser:
                    FireLaser(4);
                    break;
            }
        }
    }

    private void FireLaser(int numberOfBarrels)
    {
        for (int i = 0; i < numberOfBarrels; i++)
        {
            // Lazer ateþleme iþlemi
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