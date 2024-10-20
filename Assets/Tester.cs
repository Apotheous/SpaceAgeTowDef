using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 10f)] float timeScale = 1.0f;
    void Start()
    {
        
    }

    void Update()
    {
        //HealthRegenEntStats();
        Time.timeScale = timeScale;

    }
}
