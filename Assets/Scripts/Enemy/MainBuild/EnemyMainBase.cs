using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMainBase : MonoBehaviour
{
    public static EnemyMainBase instance;

    // Liste: D��manlar�n tutuldu�u liste
    public List<GameObject> myUnitList = new List<GameObject>();

    // Spawn ile ilgili de�i�kenler
    [SerializeField] private BasicPool myPool;
    [SerializeField] private SpawnPointGenerator spawnPointGenerator;

    [SerializeField] private GameObject asteroidsMain;
    [SerializeField] private int spawnPointCount = 10;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float stopCoroutineSec;

    private WaitForSeconds waitTime;
    private Coroutine spawnCoroutine;
    private int mySpawnPointIndex;

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Ba�lang�� kontrolleri
        if (myPool == null)
        {
            Debug.LogError("Object pool is not assigned!");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        waitTime = new WaitForSeconds(spawnInterval);
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        if (stopCoroutineSec > 0)
        {
            float elapsedTime = 0f;

            while (elapsedTime < stopCoroutineSec)
            {
                yield return waitTime;
                SpawnEnemy();

                elapsedTime += spawnInterval;
            }
        }
        else
        {
            while (true)
            {
                yield return waitTime;
                SpawnEnemy();
            }
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPointGenerator == null || myPool == null) return;

        // Rastgele bir spawn noktas� se�
        mySpawnPointIndex = Random.Range(0, spawnPointCount);

        // Havuzdan d��man al
        GameObject newEnemy = myPool.GetFromPool();
        if (newEnemy == null)
        {
            Debug.LogError("Pool returned null object!");
            return;
        }

        // Spawn noktas�n� belirle
        newEnemy.transform.position = spawnPointGenerator.GenerateSpawnPoint();

        // AsteroidsMain'e ba�la (varsa)
        if (asteroidsMain != null)
        {
            newEnemy.transform.SetParent(asteroidsMain.transform);
        }

        // D��man ayarlar�n� yap
        SetupEnemy(newEnemy);
    }

    private void SetupEnemy(GameObject myUnit)
    {


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
