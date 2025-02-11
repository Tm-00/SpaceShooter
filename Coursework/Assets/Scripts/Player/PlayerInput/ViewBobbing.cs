using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PositionFollow))]

public class ViewBobbing : MonoBehaviour
{

    public float EffectIntensity;
    public float EffectIntensityX;
    public float EffectSpeed;



    private PositionFollow FollowerInstance;
    private Vector3 OriginalOffset;
    private float SinTime;



    // Start is called before the first frame update

    void Start()

    {

        FollowerInstance = GetComponent<PositionFollow>();

        OriginalOffset = FollowerInstance.Offset;

    }



    // Update is called once per frame

    void Update()

    {

        Vector3 inputVector = new Vector3(Input.GetAxisRaw("Vertical"), 0f, Input.GetAxisRaw("Horizontal"));

        if (inputVector.magnitude > 0f)

        {

            SinTime += Time.deltaTime * EffectSpeed;

        }

        else

        {

            SinTime = 0f;

        }



        float sinAmountY = -Mathf.Abs(EffectIntensity * Mathf.Sin(SinTime));

        Vector3 sinAmountX = FollowerInstance.transform.right * EffectIntensity * Mathf.Cos(SinTime) * EffectIntensityX;



        FollowerInstance.Offset = new Vector3

        {

            x = OriginalOffset.x,

            y = OriginalOffset.y + sinAmountY,

            z = OriginalOffset.z

        };

        FollowerInstance.Offset += sinAmountX;

    }
}
