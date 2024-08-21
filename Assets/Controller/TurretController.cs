using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            // Burada her bir namlu için mermi spawn iþlemi gerçekleþtirilebilir.
            GameObject firingBullet= Instantiate(bullet, barrels[i].transform);
            firingBullet.transform.position = barrels[i].transform.position;
            firingBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 5000f);
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
