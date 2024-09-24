using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField] public struct CameraConfig
{
    [SerializeField] public float yaw;
    [SerializeField] public float pitch;
    [SerializeField] public float roll;
    [SerializeField] public Vector3 pivot;
    [SerializeField] public Vector3 distance;
    [SerializeField] public float fov;
    
    public Quaternion GetRotation()
    {
        return Quaternion.Euler(yaw, pitch, roll);
    }
    public Vector3 GetPos()
    {
        return pivot + distance;
    }

    public void DrawGizmos(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(pivot, 0.25f);
        Vector3 position = GetPos();
        Gizmos.DrawLine(pivot, position);
        Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, fov, 0.5f, 0f, Camera.main.aspect);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
