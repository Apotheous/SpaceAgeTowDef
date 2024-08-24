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
    public Transform target;    // D��man hedef
    public Transform angleX;    // X ekseninde d�nen par�a
    public Transform angleY;    // Y ekseninde d�nen par�a
    public float rotationSpeed = 5f; // D�nme h�z�

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
        // Hedefin taretle olan fark vekt�r�n� hesapla
        Vector3 directionToTarget = target.position - angleX.position;

        // Y bile�enini s�f�rla, sadece yatay d�zlemde �al��
        directionToTarget.y = 0;

        // E�er directionToTarget s�f�r vekt�r� de�ilse
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            // Hedefe y�nelmek i�in gereken rotasyonu hesapla
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // X ekseninde d�zg�n bir �ekilde d�nmesi i�in interpolasyon (lerp) kullan
            angleX.rotation = Quaternion.Slerp(angleX.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void AimAtTargetY()
    {
        // Hedefin taretle olan fark vekt�r�n� hesapla
        Vector3 directionToTarget = target.position - angleY.position;

        // E�er directionToTarget s�f�r vekt�r� de�ilse
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            // Hedefe y�nelmek i�in gereken rotasyonu hesapla
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Y ekseninde d�zg�n bir �ekilde d�nmesi i�in interpolasyon (lerp) kullan
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
            rb.velocity = Vector3.zero; // Eski h�z�n� s�f�rla
            rb.angularVelocity = Vector3.zero; // Eski d�nme h�z�n� s�f�rla
            rb.AddForce(barrels[barrelIndex].forward * 5000f);
        }
        else
        {
            // E�er obje havuzunda mermi yoksa yeni bir mermi olu�tur
            GameObject newBullet = Instantiate(bulletPrefab, barrels[barrelIndex].position, barrels[barrelIndex].rotation);
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrels[barrelIndex].forward * 5000f);
        }
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero; // Mevcut h�z�n� s�f�rla
        rb.angularVelocity = Vector3.zero; // Mevcut d�nme h�z�n� s�f�rla

        bullet.SetActive(false);
        bullet.transform.SetParent(barrels[0]);
        bullet.transform.localPosition = Vector3.zero; // Pozisyonu s�f�rla
        bullet.transform.localRotation = Quaternion.identity; // Rotasyonu s�f�rla

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
            // Lazer ate�leme i�lemi
            Debug.Log(numberOfBarrels + " namludan lazer ate�leniyor");
            // Burada her bir namlu i�in lazer beam olu�turulabilir.
        }
    }
    #endregion

}

public enum FireType
{
    Bullet,
    Laser   
}