using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : BulletBase, ITriggerCheckable
{

    public void OnCollisionEnter(Collision collider)
    {
        ITriggerCheck(collider);

    }
    public void ITriggerCheck(Collision c)
    {
        
        IDamageable damageAble = c.transform.GetComponent<IDamageable>();

        switch (Type)
        {
            case type.BULLET:
                if (damageAble != null)
                {
                    damageAble.Damage(my_Damage);
                }
                else
                {
                    my_Damage = 0;
                }
                break;
            case type.LASER:
                DamageLaser(c);
                break;
            case type.CANNONBALL:
                if (damageAble != null)
                {
                    if (explosionRadius > 0f)
                    {
                        Explode();
                    }
                }
                break;
            case type.EXPLOSÝVE:
                break;
            default:
                break;
        }
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
    void DamageLaser(Collision c)
    {
        float laserTimer;
        laserTimer = + 0.01f*Time.deltaTime;
        if (laserTimer==0.05f)
        {
            Damage(c.transform);
        }
    }

}
