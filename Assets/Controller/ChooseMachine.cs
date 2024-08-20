using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseMachine : MonoBehaviour
{
    public GameObject turret;
    public GoldMain goldMain;
    public int cost;
    void Start()
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
        if (goldMain.Gold > turret.transform.GetComponent<TurretController>().turretModel.Cost)
        {
            goldMain.AddGold(turret.transform.GetComponent<TurretController>().turretModel.Cost);
            chooseMachineSpawn();
           
        } else
        {
            Debug.LogError("not yet");
        }
    }
}
