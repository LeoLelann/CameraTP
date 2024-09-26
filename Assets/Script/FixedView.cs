using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedView : AView
{
    public float Yaw;
    public float Pitch;
    public float Roll;
    public float Fov;

    public override CameraConfig GetConfiguration()
    {
        CameraConfig config = new CameraConfig();
        config.Pivot = transform.position;
        config.Distance = Vector3.zero;
        config.Pitch = Pitch;
        config.Roll = Roll;
        config.Yaw = Yaw;
        config.Fov = Fov;
        return config;
    }
}
