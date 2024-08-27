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
        TurretController.Instance.gameObject.name = myTurretName;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            if (TurretController.Instance.name == myTurretName)
            {
                TurretController.Instance.ReturnBulletToPool(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
