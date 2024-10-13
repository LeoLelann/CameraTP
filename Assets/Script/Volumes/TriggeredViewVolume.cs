using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggeredViewVolume : AViewVolume
{
    public GameObject target;

    private void Start()
    {
        List<Collider> colliders = new List<Collider>();
        GetComponents<Collider>(colliders);
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            SetActive(false);
        }
    }
}
