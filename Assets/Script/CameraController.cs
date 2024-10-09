using System.Collections.Generic;
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

    private bool isCutRequested;

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

        targetConfig = ComputeAverage();
        currentConfig = targetConfig;
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
    /*    private void Smooth()
        {
            //transform.position = Vector3.SmoothDamp(CameraComp.transform.position, target.GetPos(), ref _target, 10f, 20f);
            //_target = Vector3.SmoothDamp(CameraComp.transform.eulerAngles, _target, ref _velocity, 5f, 7f);

        }*/
    private void SmoothConfiguration()
    {
        currentConfig.Yaw = Mathf.Lerp(currentConfig.Yaw, targetConfig.Yaw, Time.deltaTime * smoothingSpeed);
        currentConfig.Pitch = Mathf.Lerp(currentConfig.Pitch, targetConfig.Pitch, Time.deltaTime * smoothingSpeed);
        currentConfig.Roll = Mathf.Lerp(currentConfig.Roll, targetConfig.Roll, Time.deltaTime * smoothingSpeed);

        currentConfig.Pivot = Vector3.Lerp(currentConfig.Pivot, targetConfig.Pivot, Time.deltaTime * smoothingSpeed);
        currentConfig.Distance = Vector3.Lerp(currentConfig.Distance, targetConfig.Distance, Time.deltaTime * smoothingSpeed);

        currentConfig.Fov = Mathf.Lerp(currentConfig.Fov, targetConfig.Fov, Time.deltaTime * smoothingSpeed);
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
