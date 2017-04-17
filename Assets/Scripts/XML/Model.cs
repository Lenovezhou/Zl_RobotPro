using UnityEngine;
using System.Collections;

public class Model : MonoBehaviour
{
    public Vector3 OriginPos;
    public Vector3 TargetPos;

    public bool Start=false;

    private float m_speed=0.1f;
    void Update()
    {
        if (Start)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPos, m_speed);
            if (Vector3.Distance(transform.position, TargetPos) < 0.01f)
            {
                Start = false;
            }
        }
    }
}
