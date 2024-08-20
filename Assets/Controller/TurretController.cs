using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour,IClickable
{
    public TurretModel turretModel;
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
    }

    void IClickable.SelectionTurret()
    {
        
    }
}
