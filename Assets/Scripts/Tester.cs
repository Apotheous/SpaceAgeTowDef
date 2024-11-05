using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

public class Tester : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 10f)] float timeScale = 1.0f;
    [Inject] PoolStorage poolStorage;
    
    void Start()
    {
        
    }

    void Update()
    {
        //HealthRegenEntStats();
        Time.timeScale = timeScale;

    }
}
