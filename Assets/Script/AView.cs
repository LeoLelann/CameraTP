using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AView : MonoBehaviour
{
    public float weight;
    virtual public CameraConfig GetConfiguration() => new CameraConfig();

    public void SetActive(bool isActive)
    {
        if (isActive)
        {
            CameraController.Instance.AddView(this);
        }
        else
        {
            CameraController.Instance.RemoveView(this);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        GetConfiguration().DrawGizmos(Color.green);
    }
#endif
}
