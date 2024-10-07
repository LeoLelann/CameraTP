using UnityEngine;

public class DollyView : AView {
    public float Roll, Fov = 60, distanceOnRail, speed = 1;
    public Vector3 Distance;
    public GameObject target;
    public Rail rail;

    public override CameraConfig GetConfiguration()
    {
        CameraConfig config = new CameraConfig();
        config.Roll = Roll;
        config.Fov = Fov;
        config.Distance = Distance;

        int direction = 0;
        if(Input.GetKey(KeyCode.D))
        {
            direction = 1;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            direction = -1;
        }

        distanceOnRail += Time.deltaTime * speed * direction;

        config.Pivot = rail.GetPosition(distanceOnRail);

        Vector3 heading = target.transform.position - config.Pivot;
        float distance = heading.magnitude;
        Vector3 targetDirection = heading / distance;

        float currentYaw = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
        float currentPitch = -Mathf.Asin(targetDirection.y) * Mathf.Rad2Deg;

        config.Yaw = currentYaw;
        config.Pitch = currentPitch;

        return config;
    }
}
