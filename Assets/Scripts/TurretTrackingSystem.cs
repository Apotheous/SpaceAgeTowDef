using UnityEngine;

public class TurretTrackingSystem : MonoBehaviour
{
    [System.Serializable]
    public class RotationSettings
    {
        public Transform rotationPoint;
        public float rotationSpeed = 10f;
        public bool lockYAxis; // X ekseni rotasyonu için Y'yi kilitlemek üzere
        public Color debugLineColor = Color.red;
    }

    [SerializeField] private RotationSettings xAxisRotation;
    [SerializeField] private RotationSettings yAxisRotation;

    private Transform currentTarget;
    private const float MIN_ROTATION_THRESHOLD = 0.001f;

    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    public void UpdateTracking()
    {
        if (currentTarget == null) return;

        TrackTarget(xAxisRotation);
        TrackTarget(yAxisRotation);
    }

    private void TrackTarget(RotationSettings settings)
    {
        if (settings.rotationPoint == null) return;

        Collider targetCollider = currentTarget.GetComponent<Collider>();
        if (targetCollider == null) return;

        Vector3 closestPoint = targetCollider.ClosestPoint(settings.rotationPoint.position);
        Vector3 directionToTarget = closestPoint - settings.rotationPoint.position;

        if (settings.lockYAxis)
        {
            directionToTarget.y = 0;
        }

        if (directionToTarget.sqrMagnitude > MIN_ROTATION_THRESHOLD)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            settings.rotationPoint.rotation = Quaternion.Slerp(
                settings.rotationPoint.rotation,
                targetRotation,
                Time.deltaTime * settings.rotationSpeed
            );

            if (Debug.isDebugBuild)
            {
                Debug.DrawLine(settings.rotationPoint.position, closestPoint, settings.debugLineColor);
            }
        }
    }

    // Ýsteðe baðlý: Rotasyon tamamlandý mý kontrolü
    public bool IsRotationComplete(float threshold = 0.1f)
    {
        if (currentTarget == null) return false;

        bool xComplete = IsAxisRotationComplete(xAxisRotation, threshold);
        bool yComplete = IsAxisRotationComplete(yAxisRotation, threshold);

        return xComplete && yComplete;
    }

    private bool IsAxisRotationComplete(RotationSettings settings, float threshold)
    {
        if (settings.rotationPoint == null || currentTarget == null) return false;

        Vector3 targetDirection = (currentTarget.position - settings.rotationPoint.position).normalized;
        float dot = Vector3.Dot(settings.rotationPoint.forward, targetDirection);

        return dot > (1 - threshold);
    }
}
