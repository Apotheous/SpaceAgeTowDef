using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMainBase : MonoBehaviour
{
    public static EnemyMainBase instance;
    public List<GameObject> myUnitList = new List<GameObject>();
    public GameObject myUnits;
    public Transform mySpawnPoint;

    public float Timer;
    public float spawnRate;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= spawnRate)
        {
            GameObject myUnit= Instantiate(myUnits,mySpawnPoint);
            myUnitList.Add(myUnit);
            Timer = 0f;
        }
    }
}
