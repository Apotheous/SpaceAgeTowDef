using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMachine : MonoBehaviour
{
    public GameObject turret;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject chooseMachineSpawn()
    {
        GameObject newObj = Instantiate(turret);
        newObj.name = turret.name;
        return turret;
    }

    public void chooseMachine()
    {
        chooseMachineSpawn();
    }
}
