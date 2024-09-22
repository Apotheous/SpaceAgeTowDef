using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    public string enemyGroupTag;

    public float speed = 70f;

    public float my_Damage;

    public float explosionRadius;

    
    public void OnCollisionEnter(Collision collider)
    {
        IDamageable damageAble = collider.transform.GetComponent<IDamageable>();
        if (damageAble != null)
        {
            if (explosionRadius > 0f)
            {
                Explode();
            }
            else if (explosionRadius == 0f)
            {
                damageAble.Damage(my_Damage);
            }
        }
        else
        {
            my_Damage = 0;
        }

    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == enemyGroupTag)
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.Damage(my_Damage);
            //my_Damage = 0;
        }
    }
}
