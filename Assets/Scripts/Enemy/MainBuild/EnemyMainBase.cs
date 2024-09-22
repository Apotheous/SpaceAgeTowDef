using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMainBase : MonoBehaviour
{
    public static EnemyMainBase instanse;
    public List<GameObject> myUnitList = new List<GameObject>();
    public GameObject myUnits;
    public Transform mySpawnPoint;

    public float Timer;
    public float spawnRate;
    private void Awake()
    {
        if (instanse == null)
            instanse = this;
        else
            Destroy(gameObject);
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
