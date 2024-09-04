using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TurretController;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
{

    private Rigidbody rb;
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

    public float moveSpeed;



     [Header("Unity Stuff")]
    public Image healthBar;

    private Animator animator;

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

    #region State Machine Variables
    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set;}
    public EnemyAttackState AttackState { get; set; }

    #endregion

    #region Idle Veriables
    public float RandomMovementrange = 5f;
    public float randomMovementSpeed = 1f;
    public Rigidbody bulletPrefab;

    #endregion

    [field: SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    Rigidbody IEnemyMoveable.rb { get; set; }
    public bool IsMovingForward { get; set; } = true;

    public bool IsAggroed { get; set; }
    public bool IsWithinStrikeingDistance { get; set; }

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        CurrentHealth = MaxHealth;
        StateMachine.Initialize(IdleState);
        Debug.Log("+++++++Object Name = "+gameObject.name);
        InvokeRepeating("UpdateTargetWithGizmos", 0f, 0.5f);
    }
    private void Update()
    {
        MoveEnemy(gameObject, moveSpeed);
        WalkDirectionControl();
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
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
        Debug.Log("+++++++ScannedObject++++++++ ");
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
    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        healthBar.fillAmount = CurrentHealth / MaxHealth;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public void MoveEnemy(GameObject e, float moveSpeed)
    {
        Debug.Log("MoveEnemy entry "+e.name + "asdas " + moveSpeed);
        e=gameObject;
        transform.Translate(e.transform.forward * moveSpeed * Time.deltaTime);
    }

    public void CheckForForwardOrBackFacing(Vector3 velocity)
    {
        if (IsMovingForward && moveSpeed<0)
        {
            IsMovingForward = !IsMovingForward;
        }     
        else if (!IsMovingForward && moveSpeed>0)
        {
            IsMovingForward = !IsMovingForward;
        }

    }

    #region Animation Triggers

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }
    #region Distance Checks
    public void SetAggroStatus(bool aggroStatus)
    {
        IsAggroed = aggroStatus;
    }

    public void SetStrikingDistanceBool(bool isWithinStrikeingDistance)
    {
        IsWithinStrikeingDistance = isWithinStrikeingDistance;
    }
    #endregion


    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepSound
    }
    #endregion
}
