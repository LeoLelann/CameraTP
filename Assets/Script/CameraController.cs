using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera CameraComp;
    public CameraConfig Configuration;

    private static CameraController _instance = null;
    public static CameraController Instance => _instance;

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

    void ApplyConfiguration()
    {
        CameraComp.transform.position = Configuration.GetPos();
        CameraComp.transform.rotation = Configuration.GetRotation();
        CameraComp.fieldOfView = Configuration.Fov;
    }

    private void Update()
    {
        ApplyConfiguration();
    }

    void OnDrawGizmos()
    {
        Configuration.DrawGizmos(Color.red);
    }

    void OnDrawGizmosSelected()
    {
        Configuration.DrawGizmos(Color.red);
    }
}
