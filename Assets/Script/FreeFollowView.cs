using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFollowView : AView
{
    float[] pitch = new float[3];
    float[] roll = new float[3];
    float[] fov = new float[3];
    float yaw;
    float yawSpeed;
    GameObject targetGO;
    Curve curve;
    float curvePos;
    float curveSpeed;


}
