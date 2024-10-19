using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMainBase : MonoBehaviour
{
    public static EnemyMainBase instance;
    public List<GameObject> myUnitList = new List<GameObject>();
    public GameObject myUnits;
    public Transform[] mySpawnPoint;
    public int mySpawnPointIndex;

    public float Timer;
    public float spawnRate;

    public SO_Controller obj_Pool;
    private int poolIndex = 0;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        // Enemy'leri pool'dan sýrayla spawn ediyoruz
        GetEnemyFromPool();
    }

    private void GetEnemyFromPool()
    {
        Timer += Time.deltaTime;

        if (Timer >= spawnRate)
        {
            // Pool'daki bir objeyi spawn ediyoruz
            GetPoolObject();

            Timer = 0f; // Timer'ý sýfýrla
        }
    }

    private void GetPoolObject()
    {
        //for (int i = 0; i < mySpawnPoint.Length; i++)
        //{

            if (poolIndex >= obj_Pool.transform.childCount)
            {
                poolIndex = 0; // Pool'daki son objeye gelindiyse baþa dön
            }

            // Pool'dan sýradaki enemy objesini alýyoruz
            GameObject myUnit = obj_Pool.transform.GetChild(poolIndex).gameObject;

            if (!myUnit.activeInHierarchy)
            {
                // Spawn point'e taþýr ve aktif hale getirir
                myUnit.transform.position = mySpawnPoint[mySpawnPointIndex].position;
                myUnit.SetActive(true);
                myUnitList.Add(myUnit);

                //myUnit.GetComponent<BoxCollider>().enabled = true;
                //myUnit.GetComponent<Rigidbody>().isKinematic = false;
                //myUnit.GetComponent<Enemy>().CurrentHealth = 50;
                //myUnit.GetComponent<Enemy>().moveSpeed = 2;

                Debug.Log(myUnit.GetComponent<Enemy>().CurrentHealth);

                poolIndex++; // Sýradaki pool elemanýna geç
                mySpawnPointIndex++;
                if (mySpawnPointIndex>=mySpawnPoint.Length)
                {
                    mySpawnPointIndex = 0;
                }
            }

        //}
        
    }


    private void EnemyCreator()
    {
        Timer += Time.deltaTime;

        if (Timer >= spawnRate)
        {
            GameObject myUnit = Instantiate(myUnits, mySpawnPoint[0]);
            myUnitList.Add(myUnit);

            Timer = 0f;
        }
    }
}
