using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera CameraComp;
    private CameraConfig currentConfig;
    private CameraConfig targetConfig;

    private static CameraController _instance = null;
    public static CameraController Instance => _instance;

    public float smoothingSpeed = 5f;
    //public CameraConfig target;
    //public CameraConfig current;
    //private Vector3 _target;
    private List<AView> activeViews = new List<AView>();
    //private Vector3 _velocity;

    //set as true by default to avoid a smoothing at start; computeAverage and currentConfig = targetConfig at start return errors
    private bool isCutRequested = true;

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
        //_target = new Vector3(Configuration.Yaw, Configuration.Pitch, Configuration.Roll);
    }

    private void Update()
    {
        if(activeViews.Count == 0)
        {
            return;
        }

        targetConfig = ComputeAverage();

        if(isCutRequested)
        {
            currentConfig = targetConfig;
            isCutRequested = false;
        }

        SmoothConfiguration();
        ApplyConfiguration();
    }

    private void SmoothConfiguration()
    {
        float alpha = Time.deltaTime* smoothingSpeed;
        Vector2 currentYaw = new Vector2(Mathf.Cos(currentConfig.Yaw * Mathf.Deg2Rad),
        Mathf.Sin(currentConfig.Yaw * Mathf.Deg2Rad));

        Vector2 targetYaw = new Vector2(Mathf.Cos(targetConfig.Yaw * Mathf.Deg2Rad),
        Mathf.Sin(targetConfig.Yaw * Mathf.Deg2Rad));

        Vector2 yawResult = Vector2.Lerp(currentYaw, targetYaw, alpha);
        currentConfig.Yaw = Vector2.SignedAngle(Vector2.right, yawResult);

        currentConfig.Pitch = Mathf.Lerp(currentConfig.Pitch, targetConfig.Pitch, alpha);
        currentConfig.Roll = Mathf.Lerp(currentConfig.Roll, targetConfig.Roll, alpha);

        currentConfig.Pivot = Vector3.Lerp(currentConfig.Pivot, targetConfig.Pivot, alpha);
        currentConfig.Distance = Vector3.Lerp(currentConfig.Distance, targetConfig.Distance, alpha);

        currentConfig.Fov = Mathf.Lerp(currentConfig.Fov, targetConfig.Fov, alpha);
    }

    private void ApplyConfiguration()
    {
        CameraComp.transform.position = currentConfig.GetPos();
        CameraComp.transform.rotation = currentConfig.GetRotation();
        CameraComp.fieldOfView = currentConfig.Fov;
    }

    public void Cut()
    {
        isCutRequested = true;
    }

    private void OnDrawGizmos()
    {
        currentConfig.DrawGizmos(Color.red);
    }

    private void OnDrawGizmosSelected()
    {
        currentConfig.DrawGizmos(Color.red);
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

        foreach (var view in activeViews)
        {
            config += view.GetConfiguration() * view.weight;
            somme += view.weight;
        }
        config /= somme;
        config.Yaw = ComputeAverageYaw();
        return config;
    }
}
