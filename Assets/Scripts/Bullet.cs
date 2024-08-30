using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string enemyGroupTag;
    private Transform target;

    public float speed = 70f;

    public int damage = 50;

    public float explosionRadius = 0f;
    public GameObject impactEffect;

    public void OnCollisionEnter(Collision collider)
    {
        Debug.Log("Bullet Contact");
        if (collider.transform.tag == enemyGroupTag)
        {
            Debug.Log("Bullet Contact  1");
            target = collider.gameObject.transform;
            target.gameObject.GetComponent<EnemyUnit>();

            HitTarget();
        }
    }
    void HitTarget()
    {
        Debug.Log("Bullet Contact  2");
        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
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
        EnemyUnit e = enemy.GetComponent<EnemyUnit>();

        if (e != null)
        {
            e.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
