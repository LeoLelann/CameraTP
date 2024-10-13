using UnityEngine;

public class FixedFollowView : AView {
    public float Roll, Fov;
    public GameObject target;

    public GameObject centralPoint;

    [Range(0.0f, 360.0f)] public float yawOffsetMax;
    [Range(0.0f, 360.0f)] public float pitchOffsetMax;

    public override CameraConfig GetConfiguration()
    {
        CameraConfig config = new CameraConfig();
        config.Pivot = transform.position;
        config.Distance = Vector3.zero;
        config.Roll = Roll;
        config.Fov = Fov;

        Vector3 centralPointDirection = GetDirection(centralPoint);
        float centralYaw = ComputeYaw(centralPointDirection);
        float centralPitch = ComputePitch(centralPointDirection);

        Vector3 targetDirection = GetDirection(target);
        float currentYaw = ComputeYaw(targetDirection);
        float currentPitch = ComputePitch(targetDirection);

        float yawDelta = currentYaw - centralYaw;
        float pitchDelta = currentPitch - centralPitch;

        while (yawDelta > 180.0f)
        {
            yawDelta -= 360.0f;
        }

        while(yawDelta < -180.0f)
        {
            yawDelta += 360.0f;
        }

        config.Yaw = Mathf.Clamp(yawDelta, -yawOffsetMax, yawOffsetMax) + centralYaw;
        config.Pitch = Mathf.Clamp(pitchDelta, -pitchOffsetMax, pitchOffsetMax) + centralPitch;

        return config;
    }

    private Vector3 GetDirection(GameObject gameObject)
    {
#if UNITY_EDITOR
        if(gameObject == null)
        {
            return Vector3.zero;
        }
#endif
        Vector3 heading = gameObject.transform.position - transform.position;
        float distance = heading.magnitude;
        Vector3 direction = heading / distance;
        return direction;
    }

    private float ComputeYaw(Vector3 direction)
    {
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }

    private float ComputePitch(Vector3 direction)
    {
        return -Mathf.Asin(direction.y) * Mathf.Rad2Deg;
    }
}
