using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerBuildManager : MonoBehaviour, IClickable
{
    // PUBLIC VARIABLES
    public GameObject MyTower;
    public GameObject currentTurret;
    public List <GameObject> craftObjects = new List<GameObject>();   
    public List <GameObject> gridPlanes = new List<GameObject>();
    public static Transform builderTransform;
    // PRIVATE VARIABLES
    private bool isActivePlanes = false;
    private int gridPlanesCount;

    private void Start()
    {
        gridPlanesCount = gridPlanes.Count;
        gameObject.name = MyTower.name + "_Builder";
    }
    public void StartSpawnTurrets()
    {
        builderTransform = GetComponent<Transform>().transform;

        isActivePlanes = isActivePlanes ? false : true;
        for (int i = 0; i < gridPlanes.Count; i++)
        {
            Instantiate(craftObjects[i], gridPlanes[i].transform);
            craftObjects[i].GetComponent<TurretController>().uiState = true;
            gridPlanes[i].SetActive(isActivePlanes);
        }
    }

    public void planeClose()
    {
        for (int i = 0; i < gridPlanes.Count; i++)
        {
            gridPlanes[i].SetActive(false);
        }
    }   
    public void planeOpen()
    {
        builderTransform = GetComponent<Transform>().transform;

        isActivePlanes = isActivePlanes ? false : true;
        for (int i = 0; i < gridPlanes.Count; i++)
        {
            gridPlanes[i].SetActive(isActivePlanes);
        }
    }
    public void DestroyCurrentTurret()
    {
        Destroy(currentTurret); 
    }

    public void ICilkable()
    {
        planeOpen();
        // Kule týklandýðýnda GameManager'daki metodu çaðýr
        GameManager.Instance.OnTowerClicked(gameObject);
    }
}
