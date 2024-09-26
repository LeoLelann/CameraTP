using System;
using UnityEngine;

[Serializable]
public struct CameraConfig
{
    public float Yaw;
    public float Pitch;
    public float Roll;
    public Vector3 Pivot;
    public Vector3 Distance;
    public float Fov;

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(Yaw, Pitch, Roll);
    }

    public Vector3 GetPos()
    {
        return Pivot + Distance;
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(Pivot, 0.25f);
        Vector3 position = GetPos();
        Gizmos.DrawLine(Pivot, position);
        Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, Fov, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
