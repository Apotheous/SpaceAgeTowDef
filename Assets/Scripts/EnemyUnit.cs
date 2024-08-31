using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TurretController;
using static UnityEngine.GraphicsBuffer;

public class EnemyUnit : MonoBehaviour
{

    public Rigidbody rb;
    public string enemyGroupTag;
    public Transform target;

    [System.Serializable]
    public class MyWeapon
    {
        public float GizmosRange;

    }
    public MyWeapon myWeapon;

    [System.Serializable]
    public class RotationClass
    {

        public Transform angleX;    // X ekseninde dönen parça
        public Transform angleY;    // Y ekseninde dönen parça
        public float rotationSpeed = 5f; // Dönme hızı

        public float notDeep;
        public float notDeepT;
        public float BarrelHeightAllowance;
    }

    public RotationClass rotationClass;

    public float moveSpeed = 2f;  // Hareket hýzý

    public float startHealth;
    private float health;


    [Header("Unity Stuff")]
    public Image healthBar;

    public Animator animator;

    //Animation States
    const string ENEMY_IDLE = "idleGuard";
    const string ENEMY_WALK_FRONT = "walkFront";
    const string ENEMY_WALK_BACK = "walkBack";
    const string ENEMY_SHOOT_AUTO = "shootAuto";
    const string ENEMY_RUN_GUARD = "runGuard";

    //
    const string ENEMY_RELOAD = "reload";
    const string ENEMY_JUMP = "jump";
    const string ENEMY_SHOOT_SİNGLE = "shootSingle";

    private string currentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        health = startHealth;

    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        // Objenin ileri doðru (z ekseni boyunca) hareket etmesini saðla
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        WalkDirectionControl();
    }


    void WalkDirectionControl()
    {
        if (rb != null)
        {
            if (moveSpeed > 0)
            {
                ChangeAnimationState(ENEMY_WALK_FRONT);
            }
            if (moveSpeed < 0)
            {
                ChangeAnimationState(ENEMY_WALK_BACK);
            }
            if (moveSpeed == 0)
            {
                ChangeAnimationState(ENEMY_IDLE);
            }
        }
    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    #region SelectionTarget

    void UpdateTargetWithGizmos()
    {
        // Gizmos menzili içinde bulunan düşmanları tarar
        Collider[] colliders = Physics.OverlapSphere(transform.position, myWeapon.GizmosRange);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(enemyGroupTag))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    rotationClass.notDeepT = distanceToEnemy;

                    // average value notDeep 30f
                    if (rotationClass.notDeepT > rotationClass.notDeep)
                    {
                        nearestEnemy = collider.gameObject;
                        target = collider.transform;
                    }
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= myWeapon.GizmosRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
    #endregion
}
