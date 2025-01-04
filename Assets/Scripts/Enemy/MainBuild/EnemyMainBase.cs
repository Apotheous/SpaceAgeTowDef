using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMainBase : MonoBehaviour
{
    public static EnemyMainBase instance;
    public List<GameObject> myUnitList = new List<GameObject>();
    public GameObject myUnits; // Kullanýlmýyor, EnemyCreator() metodunda kullanýlýyor ama o metod çaðrýlmýyor
    public Transform[] mySpawnPoint;
    public int mySpawnPointIndex;
    public float Timer;
    public float spawnRate;
    public SO_Controller obj_Pool;
    private int poolIndex = 0;

    private void Awake()
    {
        // Singleton pattern düzeltmesi
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Eðer sahneler arasý geçiþte korunmasý gerekiyorsa
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Baþlangýç kontrolleri
        if (obj_Pool == null)
        {
            Debug.LogError("Object pool is not assigned!");
            enabled = false;
            return;
        }

        if (mySpawnPoint == null || mySpawnPoint.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        GetEnemyFromPool();
    }

    private void GetEnemyFromPool()
    {
        Timer += Time.deltaTime;
        if (Timer >= spawnRate)
        {
            SpawnNextEnemy();
            Timer = 0f;
        }
    }

    private void SpawnNextEnemy()
    {
        if (obj_Pool.transform.childCount == 0) return;

        // Pool index kontrolü
        if (poolIndex >= obj_Pool.transform.childCount)
        {
            poolIndex = 0;
        }

        // Spawn point index kontrolü
        if (mySpawnPointIndex >= mySpawnPoint.Length)
        {
            mySpawnPointIndex = 0;
        }

        // Pool'dan düþman al
        GameObject myUnit = obj_Pool.transform.GetChild(poolIndex).gameObject;

        // Eðer obje zaten aktifse, sonraki objeyi dene
        if (myUnit.activeInHierarchy)
        {
            poolIndex++;
            return;
        }

        // Düþmaný spawn et ve ayarla
        SetupEnemy(myUnit);

        // Ýndexleri güncelle
        poolIndex++;
        mySpawnPointIndex++;
    }

    private void SetupEnemy(GameObject myUnit)
    {
        if (!myUnit || mySpawnPointIndex >= mySpawnPoint.Length) return;

        myUnit.transform.position = mySpawnPoint[mySpawnPointIndex].position;

        Enemy enemyComponent = myUnit.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.EnemyStartMeth();
            Debug.Log($"Enemy spawned with health: {enemyComponent.CurrentHealth}");
        }
        else
        {
            Debug.LogWarning($"Enemy component missing on unit: {myUnit.name}");
        }

        myUnit.SetActive(true);

        if (!myUnitList.Contains(myUnit))
        {
            myUnitList.Add(myUnit);
        }
    }
}
