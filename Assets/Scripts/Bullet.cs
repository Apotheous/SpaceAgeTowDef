using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    public string enemyGroupTag;
    private Transform target;
    private Rigidbody rb;
    public float speed = 70f;

    public float damage = 50;

    public float explosionRadius = 0f;
    public GameObject impactEffect;
    
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision collider)
    {

        
        if (collider.transform.tag == enemyGroupTag)
        {
            target = collider.gameObject.transform;
            target.gameObject.GetComponent<EnemyUnit>();

            HitTarget();
        }
        else
        {
            damage = 0;
        }
    }
    void HitTarget()
    {
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
            damage = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
