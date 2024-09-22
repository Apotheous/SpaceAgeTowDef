using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPool : MonoBehaviour
{
    public float lifetime = 5f; // Merminin ömrü (saniye)
    private float lifeTimer;
    private string myTurretName;

    private void OnEnable()
    {
        lifeTimer = lifetime;
        //TryModelController.Instance.gameObject.name = myTurretName;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            //if (TryModelController.Instance.name == myTurretName)
            //{
            //    TryModelController.Instance.ReturnBulletToPool(this.gameObject);
            //    Debug.Log("Bullet Tarete Döndü");
            //}
            //else
            //{
            //    Destroy(this.gameObject);
            //    Debug.Log("Bullet Destroy Edildi");
            //}
        }
    }
}
