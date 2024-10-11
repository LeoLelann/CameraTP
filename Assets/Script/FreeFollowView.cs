using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowView : AView
{
    public float[] pitch = new float[3];
    public float[] roll = new float[3];
    public float[] fov = new float[3];

    float yaw;
    public float yawSpeed = 1;
    public GameObject targetGO;
    public Curve curve = new Curve();

    [Range(0f, 1f)]
    public float curvePos;
    public float curveSpeed = 1;

    public override CameraConfig GetConfiguration() {
        CameraConfig config = new CameraConfig();

        yaw += Input.GetAxis("Mouse X") * Time.deltaTime * yawSpeed;
        config.Yaw = yaw;

        curvePos += Input.GetAxis("Mouse Y") * Time.deltaTime * curveSpeed;
        curvePos = Mathf.Clamp01(curvePos);

        int leftIndex = curvePos < 0.5 ? 0 : 1;
        int rightIndex = curvePos < 0.5 ? 1 : 2;

        config.Pitch = Mathf.Lerp(pitch[leftIndex], pitch[rightIndex], curvePos);
        config.Roll = Mathf.Lerp(roll[leftIndex], roll[rightIndex], curvePos);
        config.Fov = Mathf.Lerp(fov[leftIndex], fov[rightIndex], curvePos);

        Quaternion quaternion = Quaternion.Euler(config.Pitch, yaw, config.Roll);
        Matrix4x4 curveToWorldMatrix = Matrix4x4.TRS(Vector3.zero, quaternion, targetGO.transform.localScale);

        config.Pivot = targetGO.transform.position;
        config.Distance = curve.GetPosition(curvePos, curveToWorldMatrix);

        return config;
    }
}
