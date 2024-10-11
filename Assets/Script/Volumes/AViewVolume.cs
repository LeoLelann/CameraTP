using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AViewVolume : MonoBehaviour
{
    public int Priority = 0;
    public AView View;

    public int Uid;
    public static int NextUid = 0;

    protected bool IsActive { get; private set; }

    public virtual float ComputeSelfWeight() { return 1.0f; }

    public bool isCutOnSwitch;

    protected void SetActive(bool isActive)
    {
#if UNITY_EDITOR
        if(View == null)
        {
            return;
        }
#endif

        IsActive = isActive;

        if (IsActive)
        {
            ViewVolumeBlender.Instance.AddVolume(this);
        }
        else
        {
            ViewVolumeBlender.Instance.RemoveVolume(this);
        }

        if (isCutOnSwitch)
        {
            ViewVolumeBlender.Instance.Update();
            CameraController.Instance.Cut();
        }
    }

    private void Awake()
    {
        Uid = NextUid;
        NextUid++;

#if UNITY_EDITOR
        if (View == null) {
            Debug.LogWarning(name + " has a null view");
        }
#endif
    }
}
