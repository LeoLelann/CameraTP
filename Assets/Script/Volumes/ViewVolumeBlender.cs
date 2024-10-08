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
        // TODO (2: Blend)
        return;

        int maxPriority = int.MinValue;
        foreach (AViewVolume Volume in ActiveViewVolumes)
        {
            Volume.View.weight = 0;

            //if(Volume.Priority > maxPriority)
            //{
            //    maxPriority = Volume.Priority;
            //}
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
