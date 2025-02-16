using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    #region ClaudeCode
    [Header("Movement Settings")]
    [SerializeField] private Vector3 moveDirection = Vector3.forward;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float targetDistance = 1000f;

    [Tooltip("Use FixedUpdate instead of Update for physics-based movement")]
    [SerializeField] private bool useFixedUpdate = false;

    [Header("References")]
    public Transform player;

    private Vector3 velocity;
    private Transform cachedTransform;
    private bool hasReachedDistance = false;

    private void Awake()
    {
        cachedTransform = transform;
        UpdateVelocity();
    }

    private void Update()
    {
        if (!useFixedUpdate)
        {
            HandleMovement(Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (useFixedUpdate)
        {
            HandleMovement(Time.fixedDeltaTime);
        }
    }

    private void HandleMovement(float deltaTime)
    {
        if (player == null) return;

        float currentDistance = Vector3.Distance(transform.position, player.position);

        if (currentDistance < targetDistance)
        {
            // Hen�z hedef mesafeye ula��lmad�ysa, moveSpeed ile uzakla�
            hasReachedDistance = false;
            Move(deltaTime);
        }
        else
        {
            // Hedef mesafeye ula��ld�ysa, mesafeyi koru
            hasReachedDistance = true;
            MaintainDistance();
        }
    }

    private void Move(float deltaTime)
    {
        cachedTransform.position += velocity * deltaTime;
    }

    private void MaintainDistance()
    {
        Vector3 direction = (transform.position - player.position).normalized;
        transform.position = player.position + direction * targetDistance;
    }

    private void UpdateVelocity()
    {
        velocity = moveDirection.normalized * moveSpeed;
    }

    private void OnValidate()
    {
        UpdateVelocity();
    }

    // G�rsel hata ay�klama i�in
    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, targetDistance);
        }
    }
    #endregion
    #region GptCode
    //[Header("Movement Settings")]
    //[SerializeField] private Vector3 moveDirection = Vector3.forward;
    //[SerializeField] private float moveSpeed = 5f;

    //[Tooltip("Use FixedUpdate instead of Update for physics-based movement")]
    //[SerializeField] private bool useFixedUpdate = false;

    //[Header("Saturn Distance Settings")]
    //[SerializeField] private Transform player;
    //[SerializeField] private float targetDistance = 1000f;
    //[SerializeField] private float moveThreshold = 1f; // Hedef mesafeye yakla��ld���nda sabitlenecek tolerans

    //private Vector3 velocity;
    //private Transform cachedTransform;
    //private bool isAtTargetDistance = false;

    //private void Awake()
    //{
    //    cachedTransform = transform;
    //    UpdateVelocity();
    //}

    //private void Update()
    //{
    //    if (!useFixedUpdate)
    //    {
    //        HandleMovement(Time.deltaTime);
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    if (useFixedUpdate)
    //    {
    //        HandleMovement(Time.fixedDeltaTime);
    //    }
    //}

    //private void HandleMovement(float deltaTime)
    //{
    //    if (isAtTargetDistance)
    //    {
    //        MaintainDistance(); // Mesafeyi koru
    //    }
    //    else
    //    {
    //        Move(deltaTime); // Uzakla�
    //        CheckDistance(); // Mesafe kontrol�
    //    }
    //}

    //private void Move(float deltaTime)
    //{
    //    cachedTransform.position += velocity * deltaTime;
    //}

    //private void CheckDistance()
    //{
    //    float currentDistance = Vector3.Distance(player.position, cachedTransform.position);
    //    if (Mathf.Abs(currentDistance - targetDistance) <= moveThreshold)
    //    {
    //        isAtTargetDistance = true; // Hedef mesafeye ula��ld�
    //    }
    //}

    //private void MaintainDistance()
    //{
    //    Vector3 direction = (cachedTransform.position - player.position).normalized;
    //    cachedTransform.position = player.position + direction * targetDistance;
    //}

    //private void UpdateVelocity()
    //{
    //    velocity = moveDirection.normalized * moveSpeed;
    //}

    //private void OnValidate()
    //{
    //    UpdateVelocity();
    //}
    #endregion


}
