using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPool : MonoBehaviour
{
    public float lifetime = 5f; // Merminin ömrü (saniye)
    private float lifeTimer;

    private void OnEnable()
    {
        lifeTimer = lifetime;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            // Obje havuzuna geri döndür
            TurretController.Instance.ReturnBulletToPool(this.gameObject);
        }
    }
}
