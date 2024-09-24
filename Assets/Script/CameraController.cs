using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public Camera _camera;
    public CameraConfig configuration;

    private static CameraController instance = null;
    public static CameraController Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        _camera = GetComponent<Camera>();
    }
    void ApplyConfiguration()
    {
        _camera.transform.position = configuration.GetPos();
        _camera.transform.rotation = configuration.GetRotation();
    }
    private void Update()
    {
        ApplyConfiguration();
    }
}
