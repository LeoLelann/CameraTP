using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils {
    public static Vector3 GetNearestPointOnSegment(Vector3 a, Vector3 b, Vector3 target)
    {
        Vector3 AC = target - a;
        Vector3 n = (b - a).normalized;
        float dotResult = Vector3.Dot(AC, n);

        dotResult = Mathf.Clamp(dotResult, 0, Vector3.Distance(a, b));
        Vector3 projC = a + n * dotResult;

        return projC;
    }
}