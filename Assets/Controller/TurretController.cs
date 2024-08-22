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
    public Transform target;    // Düþman hedef
    public Transform angleX;    // X ekseninde dönen parça
    public Transform angleY;    // Y ekseninde dönen parça
    public float rotationSpeed = 5f; // Dönme hýzý

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
    void Firing()
    {
        // Örnek olarak 1 namlulu mermi atan turret
        Fire(barrels.Count, FireType.Bullet);

        //// Örnek olarak 2 namlulu mermi atan turret
        //Fire(2, FireType.Bullet);

        //// Örnek olarak 4 namlulu lazer atan turret
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
                Debug.LogError("Desteklenmeyen namlu sayýsý: " + barrelCount);
                break;
        }
    }

    private void FireBullet(int numberOfBarrels)
    {
        for (int i = 0; i < numberOfBarrels; i++)
        {
            // Mermi ateþleme iþlemi
            Debug.Log(numberOfBarrels + " namludan mermi ateþleniyor");

            // Mermiyi namludan dünya koordinatlarýnda instantiate edin
            GameObject firingBullet = Instantiate(bullet, barrels[i].transform.position, barrels[i].transform.rotation);

            // Rigidbody'e doðru yönle kuvvet uygulayýn
            Rigidbody rb = firingBullet.GetComponent<Rigidbody>();
            rb.AddForce(barrels[i].transform.forward * 5000f);
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
