using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour,IClickable
{
    private TurretModel turretModel;
    TowerBuildManager towerBuildManager;
    public Transform mybuilder;
    GameObject currentTurret;
    int previusCount;
    public bool uiState;
    void Start()
    {
        
        turretModel = new TurretModel(gameObject.name, LevelProp.LEVEL_ONE, 0);

        Debug.Log("Turret Name: " + gameObject.name);
        Debug.Log("Level: " + turretModel.Level);
        Debug.Log("Shooting Frequency: " + turretModel.ShootingFrequency);
        Debug.Log("Damage: " + turretModel.Damage);
        Debug.Log("Range of Vision: " + turretModel.RangeOfVision);
        Debug.Log("Firing Range: " + turretModel.FiringRange);
        Debug.Log("Health: " + turretModel.Health);
        Debug.Log("cost: " + turretModel.Cost);

        mybuilder=TowerBuildManager.builderTransform;
        previusCount = mybuilder.childCount;
    }

    void OnMouseDown()
    {
        if (uiState==true)
        {

            mybuilder.GetComponent<TowerBuildManager>().DestroyCurrentTurret();
            currentTurret = Instantiate(gameObject, TowerBuildManager.builderTransform);
            mybuilder.GetComponent<TowerBuildManager>().currentTurret=currentTurret;
            currentTurret.GetComponent<TurretController>().uiState = false;
        }
    }


    void IClickable.SelectionTurret()
    {
        Debug.Log("name: " + turretModel.gameObject.name);
    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.GetComponent<TowerMan>() != null)
        {
            collider.gameObject.GetComponent<TowerMan>().currentTurret = gameObject;
        }
        if (collider.gameObject.layer==LayerMask.NameToLayer("UiObjects"))
        {
            Debug.Log("+++++" + collider.name);

        }
    }
}
