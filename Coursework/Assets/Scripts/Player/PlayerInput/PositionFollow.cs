using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollow : MonoBehaviour
{

    public Transform TargetTransform;
    public Vector3 Offset;

    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;



    void Update()

    {
        Vector3 targetPosition = TargetTransform.position;

        transform.position = Vector3.SmoothDamp(transform.position,targetPosition + Offset, ref velocity, smoothTime);

    }

}
