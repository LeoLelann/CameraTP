using UnityEngine;

public class DollyView : AView {
    public float roll, fov, distance, distanceOnRail, speed;
    public GameObject target;
    public Rail rail;

    public override CameraConfig GetConfiguration()
    {
        CameraConfig config = new CameraConfig();
        config.Roll = roll;
        config.Fov = fov;
        config.Distance = rail.GetPosition(1);
        return config;
    }
}
