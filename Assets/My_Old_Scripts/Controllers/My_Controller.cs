using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class My_Controller : MonoBehaviour {

    public float MotorForce;
    public Transform centerOfMass;
    public Transform tireLeft;
    public Transform tireRight;
    public WheelCollider wheelFront;
    public WheelCollider wheelLeft;
    public WheelCollider wheelRight;

    private Rigidbody myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.centerOfMass = centerOfMass.localPosition; //Set center of mass
    }
    void Update () {
        float v1 = Input.GetAxis("Vertical") * MotorForce; //todo: to be combined with ROS msg
        float v2 = Input.GetAxis("Horizontal") * MotorForce; 
        wheelLeft.motorTorque = v1;
        wheelRight.motorTorque = v2;
        UpdateTiresMeshesPositions();
    }

    void UpdateTiresMeshesPositions() //Binding WheelColliders with tire meshes
    {
        Quaternion quatL,quatR;
        Vector3 posL,posR;
        wheelLeft.GetWorldPose(out posL, out quatL);
        wheelRight.GetWorldPose(out posR, out quatR);

        tireLeft.position = posL;
        tireLeft.rotation = quatL;
        tireRight.position = posR;
        tireRight.rotation = quatR;
    }
}
