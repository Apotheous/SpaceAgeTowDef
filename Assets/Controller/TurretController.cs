using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretController : MonoBehaviour
{


    private TurretModel turretModel;
    public TowerBuildManager towerBuildManager;
    public Transform mybuilder;

    GameObject currentTurret;
    int previusCount;
    public bool uiState;

    public List<Transform> barrels = new List<Transform>();
    public GameObject bullet;

    public GameObject targetGamObject;
    public Transform target;    // D��man hedef
    public Transform angleX;    // X ekseninde d�nen par�a
    public Transform angleY;    // Y ekseninde d�nen par�a
    public float rotationSpeed = 5f; // D�nme h�z�

    void Start()
    {

        turretModel = new TurretModel(gameObject.name, LevelProp.LEVEL_ONE, 0);

        Debug.Log("Turret Name: " + gameObject.name);
        Debug.Log("Level: " + turretModel.Level);
        Debug.Log("Shooting Frequency: " + turretModel.ShootingFrequency);
        Debug.Log("Damage: " + turretModel.Damage);
        Debug.Log("Range of Vision: " + turretModel.RangeOfVision);
        Debug.Log("Firing Range: " + turretModel.FiringRange);
        Debug.Log("Health: " + turretModel.Health);
        Debug.Log("cost: " + turretModel.Cost);

        mybuilder=TowerBuildManager.builderTransform;
        previusCount = mybuilder.childCount;

        targetGamObject = GameObject.FindWithTag("Enemy");
        target=targetGamObject.transform;
    }

    void OnMouseDown()
    {
        if (uiState==true)
        {
            mybuilder.GetComponent<TowerBuildManager>().DestroyCurrentTurret();
            currentTurret = Instantiate(gameObject, TowerBuildManager.builderTransform);
            mybuilder.GetComponent<TowerBuildManager>().currentTurret=currentTurret;
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
        if (collider.gameObject.layer==LayerMask.NameToLayer("UiObjects"))
        {
            Debug.Log("+++++" + collider.name);

        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Firing();
        }

        if (target != null)
        {
            // X ekseninde hedefin pozisyonunu takip et
            AimAtTargetX();

            // Y ekseninde hedefin pozisyonunu takip et
            AimAtTargetY();
        }
    }


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
    void Firing()
    {
        // �rnek olarak 1 namlulu mermi atan turret
        Fire(barrels.Count, FireType.Bullet);

        //// �rnek olarak 2 namlulu mermi atan turret
        //Fire(2, FireType.Bullet);

        //// �rnek olarak 4 namlulu lazer atan turret
        //Fire(4, FireType.Laser);
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

    private void FireBullet(int numberOfBarrels)
    {
        for (int i = 0; i < numberOfBarrels; i++)
        {
            // Mermi ate�leme i�lemi
            Debug.Log(numberOfBarrels + " namludan mermi ate�leniyor");

            // Mermiyi namludan d�nya koordinatlar�nda instantiate edin
            GameObject firingBullet = Instantiate(bullet, barrels[i].transform.position, barrels[i].transform.rotation);

            // Rigidbody'e do�ru y�nle kuvvet uygulay�n
            Rigidbody rb = firingBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrels[i].transform.forward * 5000f);
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

    public void FireOneBarrel(FireType fireType)
    {
        switch (fireType)
        {
            case FireType.Bullet:
                FireBullet(1);
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
                FireBullet(2);
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
                FireBullet(4);
                break;
            case FireType.Laser:
                FireLaser(4);
                break;
        }
    }
}

public enum FireType
{
    Bullet, // Mermi
    Laser // Lazer
}
