using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Curve
{
    public Vector3 A;
    public Vector3 B;
    public Vector3 C;
    public Vector3 D;

    [Range(0f, 1f)]
    public float t;

    public Vector3 GetPosition(float t)
    {
        return MathUtils.CubicBezier(A, B, C, D, t);
    }

    public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix)
    {
        return localToWorldMatrix.MultiplyPoint(GetPosition(t));
    }

    public void DrawGizmo(Color c, Matrix4x4 localToWorldMatrix)
    {
        Gizmos.color = c;

        Gizmos.DrawSphere(GetPosition(t, localToWorldMatrix), 0.5f);
        Gizmos.DrawLine(localToWorldMatrix.MultiplyPoint(A), localToWorldMatrix.MultiplyPoint(B));
        Gizmos.DrawLine(localToWorldMatrix.MultiplyPoint(B), localToWorldMatrix.MultiplyPoint(C));
        Gizmos.DrawLine(localToWorldMatrix.MultiplyPoint(C), localToWorldMatrix.MultiplyPoint(D));
    }
}
