using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class CustomizableObjectGetter : MonoBehaviour
{

    [SerializeField] private BasicPool myPool;

    [SerializeField] private SpawnPointGenerator spawnPointGenerator;

    [ColoredHeader("Parent Gameobject Where You May Want To Send Objects", HeaderColor.Red)]
    [SerializeField] private GameObject asteroidsMain;

    [SerializeField] private int spawnPointCount = 10;

    [ColoredHeader("Spawn Behavior")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float stopCorutineSec ;

    private WaitForSeconds waitTime;
    private Coroutine spawnCoroutine;


    void Start()
    {
        waitTime = new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnRoutine());
    
    }
    private IEnumerator SpawnRoutine() 
    {

        if (stopCorutineSec > 0)
        {
            float elapsedTime = 0f;
            
            while (elapsedTime < stopCorutineSec) 
            {
                yield return waitTime;
                GetAsteroids();
                
                elapsedTime += spawnInterval;
            }
        }

        else 
        {
            while (true) 
            {
                yield return waitTime;
                GetAsteroids();
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
    private void GetAsteroids()
    {
        int xcf = Random.Range(0, spawnPointCount);
        var new_Aste = myPool.GetFromPool();
    
        new_Aste.transform.position = spawnPointGenerator.GenerateSpawnPoint();
        if (asteroidsMain!=null) new_Aste.transform.SetParent(asteroidsMain.transform);
    }
}
