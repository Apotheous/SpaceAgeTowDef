using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    // PUBLIC VARIABLES
    public List <GameObject> craftObjects = new List<GameObject>();   
    public List <GameObject> gridPlanes = new List<GameObject>();

    // PRIVATE VARIABLES
    private bool isActivePlanes = false;
 
    void OnMouseDown()
    {
        isActivePlanes = isActivePlanes ? false : true;
        for (int i = 0; i <= gridPlanes.Count; i++)
        {
            Instantiate(craftObjects[i], gridPlanes[i].transform);
            gridPlanes[i].SetActive(isActivePlanes);
        }
    }
}
