using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPool : MonoBehaviour
{
    public float lifetime = 5f;
    private float lifeTimer;
    public GameObject myTurret;

    private void OnEnable()
    {
        lifeTimer = lifetime;
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            if (myTurret)
            {
                myTurret.GetComponent<TryModelController>().ReturnBulletToPool(gameObject);
            }
        }
    }
}
