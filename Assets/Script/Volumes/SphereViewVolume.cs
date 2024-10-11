using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    public GameObject target;
    public float outerRadius;
    public float innerRadius;
    private float distance;

    private void Update()
    {
#if UNITY_EDITOR
        if(target == null)
        {
            Debug.LogWarning("Add a target to " + name);
            return;
        }
#endif

        distance = Vector3.Distance(target.transform.position, transform.position);

        if(distance <= outerRadius && !IsActive)
        {
            SetActive(true);
        }

        if(distance > outerRadius && IsActive) 
        {
            SetActive(false);
        }
    }

    public override float ComputeSelfWeight()
    {
        return Mathf.InverseLerp(outerRadius, innerRadius, distance);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (innerRadius > outerRadius)
        {
            outerRadius = innerRadius;
        }

        Gizmos.DrawWireSphere(transform.position, innerRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }
#endif
}
