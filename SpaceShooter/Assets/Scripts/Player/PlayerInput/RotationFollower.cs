using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollower : MonoBehaviour
{
    public Transform Target;



    private void Update()

    {

        transform.rotation = Target.rotation;

    }
}
