using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to synchronize wheelcollider and wheel mesh
public class WheelAlignScript : MonoBehaviour
{

    public WheelCollider CorrespondingCollider;
    public GameObject SlipPrefab;
    public float RotationValue = 0.0f;

    void Update()
    {

        RaycastHit hit;

        Vector3 ColliderCenterPoint = CorrespondingCollider.transform.TransformPoint(CorrespondingCollider.center); //get the collider center point

        if (Physics.Raycast(ColliderCenterPoint, -CorrespondingCollider.transform.up, out hit, CorrespondingCollider.suspensionDistance + CorrespondingCollider.radius)) //Synchronize the position
        {
            transform.position = hit.point + (CorrespondingCollider.transform.up * CorrespondingCollider.radius);
        }
        else
        {
            transform.position = ColliderCenterPoint - (CorrespondingCollider.transform.up * CorrespondingCollider.suspensionDistance);
        }


        transform.rotation = CorrespondingCollider.transform.rotation * Quaternion.Euler(RotationValue, CorrespondingCollider.steerAngle, 0);//Synchronize the rotation

        RotationValue += CorrespondingCollider.rpm * (360 / 60) * Time.deltaTime; //Rotating speed


        WheelHit CorrespondingGroundHit; //Make sure the wheels touch the ground
        CorrespondingCollider.GetGroundHit(out CorrespondingGroundHit); 

        if (Mathf.Abs(CorrespondingGroundHit.sidewaysSlip) > 1.5f) //slip effect
        {
            if (SlipPrefab)
            {
                Instantiate(SlipPrefab, CorrespondingGroundHit.point, Quaternion.identity);
            }
        }

    }
}