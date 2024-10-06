using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera CameraComp;
    private CameraConfig Configuration;

    private static CameraController _instance = null;
    public static CameraController Instance => _instance;
    private List<AView> activeViews = new List<AView>();

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

    private void Start()
    {
        CameraComp = GetComponent<Camera>();
    }
    private void Update()
    {
        Configuration = ComputeAverage();
        ApplyConfiguration();
    }

    void ApplyConfiguration()
    {
        CameraComp.transform.position = Configuration.GetPos();
        CameraComp.transform.rotation = Configuration.GetRotation();
        CameraComp.fieldOfView = Configuration.Fov;
    }



    void OnDrawGizmosSelected()
    {
        Configuration.DrawGizmos(Color.red);
    }

    public void AddView(AView view)
    {
        activeViews.Add(view);
    }
    public void RemoveView(AView view)
    {
        activeViews.Remove(view);
    }

    public float ComputeAverageYaw()
    {
        Vector2 sum = Vector2.zero;
        foreach (AView view in activeViews)
        {
            CameraConfig config = view.GetConfiguration();
            sum += new Vector2(Mathf.Cos(config.Yaw * Mathf.Deg2Rad),
            Mathf.Sin(config.Yaw * Mathf.Deg2Rad)) * view.weight;
        }
        return Vector2.SignedAngle(Vector2.right, sum);
    }
    public CameraConfig ComputeAverage()
    {
        CameraConfig config = new CameraConfig();
        float somme = 0f;
        
        foreach ( var view in activeViews)
        {
            config += view.GetConfiguration() * view.weight;
            somme += view.weight;
        }
        config /= somme;
        config.Yaw = ComputeAverageYaw();
        return config;
    }
}
