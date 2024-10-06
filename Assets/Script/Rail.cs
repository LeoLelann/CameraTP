using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    public bool isLoop = false;
    private float length;
    private List<Transform> _childsTransforms = new List<Transform>();

    private void Start()
    {
        GetComponentsInChildren<Transform>(_childsTransforms);

        for (int i = 0; i < _childsTransforms.Count; i++)
        {   
            if (i + 1 == _childsTransforms.Count)
            {
                break;
            }

            length += Vector3.Distance(_childsTransforms[i].position, _childsTransforms[i + 1].position);
        }

        if (isLoop)
        {
            length += Vector3.Distance(_childsTransforms[_childsTransforms.Count - 1].position, _childsTransforms[0].position);
        }
    }

    public float GetLength() {return length;}

    public Vector3 GetPosition(float distance)
    {
        return Vector3.Lerp(_childsTransforms[0].position, _childsTransforms[_childsTransforms.Count - 1].position, distance);
    }

#if UNITY_EDITOR
    [SerializeField]
    private Color _gizmoColor = Color.white;

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;

        List<Transform> transforms = new List<Transform>();
        GetComponentsInChildren<Transform>(transforms);

        for (int i = 0; i < transforms.Count; i++)
        {
            if (i + 1 == transforms.Count)
            {
                break;
            }

            Gizmos.DrawSphere(transforms[i].position, 0.25f);
            Gizmos.DrawLine(transforms[i].position, transforms[i + 1].position);
        }

        Gizmos.DrawSphere(transforms[transforms.Count-1].position, 0.25f);

        if (isLoop)
        {
            Gizmos.DrawLine(transforms[transforms.Count-1].position, transforms[0].position);
        }
    }
#endif
}
