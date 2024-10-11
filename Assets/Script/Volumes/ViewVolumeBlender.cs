using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour{
    private List<AViewVolume> ActiveViewVolumes = new List<AViewVolume>();
    private Dictionary<AView, List<AViewVolume>> VolumesPerViews = new Dictionary<AView, List<AViewVolume>>();

    private static ViewVolumeBlender _instance = null;
    public static ViewVolumeBlender Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        for (int i = 0; i < ActiveViewVolumes.Count; i++)
        {
            if (i + 1 == ActiveViewVolumes.Count)
            {
                break;
            }

            ActiveViewVolumes[i].View.weight = 0;

            if (ActiveViewVolumes[i].Priority == ActiveViewVolumes[i + 1].Priority)
            {
                if(ActiveViewVolumes[i].Uid < ActiveViewVolumes[i + 1].Uid)
                {
                    continue;
                }
            }

            if (ActiveViewVolumes[i].Priority < ActiveViewVolumes[i + 1].Priority)
            {
                continue;
            }

            AViewVolume copy = ActiveViewVolumes[i];
            ActiveViewVolumes[i] = ActiveViewVolumes[i + 1];
            ActiveViewVolumes[i + 1] = copy;
        }

        foreach (AViewVolume v in ActiveViewVolumes)
        {
            float weight = Mathf.Clamp01(v.ComputeSelfWeight());
            float remainingWeight = 1.0f - weight;

            foreach (AViewVolume volume in ActiveViewVolumes)
            {
                volume.View.weight *= remainingWeight;
            }

            v.View.weight += weight;
        }
    }

    public void AddVolume(AViewVolume volume)
    {
        ActiveViewVolumes.Add(volume);

        bool bFindView = VolumesPerViews.TryGetValue(volume.View, out List<AViewVolume> views);

        if (!bFindView)
        {
            views = new List<AViewVolume>();
            VolumesPerViews.Add(volume.View, views);
            volume.View.SetActive(true);

        }

        views.Add(volume);
    }

    public void RemoveVolume(AViewVolume volume)
    {
        ActiveViewVolumes.Remove(volume);

        VolumesPerViews.TryGetValue(volume.View, out List<AViewVolume> views);

        views.Remove(volume);
        
        if(views.Count == 0)
        {
            VolumesPerViews.Remove(volume.View);
            volume.View.SetActive(false);
        }
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.Label("Actives volumes:");
        foreach (AViewVolume volume in ActiveViewVolumes)
        {
            GUILayout.Label(volume.name);
        }
    }
#endif
}
