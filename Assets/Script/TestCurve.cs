using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCurve : MonoBehaviour
{
    public Curve curve;

    private void OnDrawGizmos()
    {
        curve.DrawGizmo(Color.green ,transform.localToWorldMatrix);
    }
}
