using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMainBase : MonoBehaviour
{
    public static EnemyMainBase instance;
    public List<GameObject> myUnitList = new List<GameObject>();
    public GameObject myUnits; // Kullan�lm�yor, EnemyCreator() metodunda kullan�l�yor ama o metod �a�r�lm�yor
    public Transform[] mySpawnPoint;
    public int mySpawnPointIndex;
    public float Timer;
    public float spawnRate;
    public SO_Controller obj_Pool;
    private int poolIndex = 0;

    private void Awake()
    {
        // Singleton pattern d�zeltmesi
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // E�er sahneler aras� ge�i�te korunmas� gerekiyorsa
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Ba�lang�� kontrolleri
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

        // Pool index kontrol�
        if (poolIndex >= obj_Pool.transform.childCount)
        {
            poolIndex = 0;
        }

        // Spawn point index kontrol�
        if (mySpawnPointIndex >= mySpawnPoint.Length)
        {
            mySpawnPointIndex = 0;
        }

        // Pool'dan d��man al
        GameObject myUnit = obj_Pool.transform.GetChild(poolIndex).gameObject;

        // E�er obje zaten aktifse, sonraki objeyi dene
        if (myUnit.activeInHierarchy)
        {
            poolIndex++;
            return;
        }

        // D��man� spawn et ve ayarla
        SetupEnemy(myUnit);

        // �ndexleri g�ncelle
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
