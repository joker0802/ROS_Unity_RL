
using UnityEngine;
using System.Collections;

public class AI_Traffic_Car_PRO : MonoBehaviour
{
    //AI_Traffic_Pack//
    public GameObject steerWheel;
    public WheelCollider F_L_Wheel; 
    public WheelCollider F_R_Wheel;
    public WheelCollider B_L_Wheel;
    public WheelCollider B_R_Wheel;
    public GameObject BreakeLight;
    public GameObject waypoint;
    public float EngineMoment = 100.0f;
    public float Speed = 0;
    public float MaxSpeed = 100.0f;
    public CMode Mode = CMode.Sample;
    public CType2 CarType = CType2.City;

    private int SteeringWheel = 12;
    private float vehicleCenterOfMass = 0.0f;
    private float side_ray = 12.0f;
    private float breake_far = 20.0f;//was 10
    private int currentGear = 0;
    private float EngineRPM = 0.0f;
    private Transform[] waypoints; 

    public int currentWaypoint = 0;
    public int BrakeForce = 2000; //was 800

    private float inputSteer = 0.0f;
    private float inputTorque = 0.0f;
    private int RayDistance = 1;
    private float MaxEngineRPM = 3000.0f;
    private float MinEngineRPM = 1000.0f;

    public float EngineMoment_OldCar = 150.0f;
    public float EngineMoment_City = 250.0f;
    public float EngineMoment_HighWay = 450.0f;
    public float EngineMoment_Sport = 650.0f;
    public float EngineMoment_Custom = 650.0f;
    public string CarStatus = "Drive";



    public enum CMode
    {
        None,
        Sample,
        Advanced,
        Professional
    };


    public enum CType2
    {
        City,
        HighWay,
        OldCar,
        SportCar,
        Custom
    };

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 2, 0); //set center of mass
        BreakeLight.SetActive(false); //breake light off
        EngineMoment = EngineMoment_Custom; //init engine moment
        currentWaypoint++;
    }

    void FixedUpdate()
    {
        //GetComponent<Rigidbody>().centerOfMass.y = -1.5f;
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -1.5f, 0); //adjust center of mass
        //GetComponent<Rigidbody>().centerOfMass.Set(GetComponent<Rigidbody>().centerOfMass.x, -1.5f, GetComponent<Rigidbody>().centerOfMass.z);
        GetComponent<Rigidbody>().drag = GetComponent<Rigidbody>().velocity.magnitude / 300;
        GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, MaxSpeed);
        GetComponent<Rigidbody>().AddForce(-transform.up * GetComponent<Rigidbody>().velocity.magnitude);
        Speed = Mathf.Round(GetComponent<Rigidbody>().velocity.magnitude);
    }

    void Update()
    {


        breake_far = Speed + 5; //Dynamic braking distance
        side_ray = Speed / 2; //Dynamic field of view


        //\\\\\\\\\\\STATUS //////////////////////////////

        if (CarStatus == "Stop")
        {
            BreakeLight.SetActive(true);
            EngineMoment = 0;
            F_L_Wheel.brakeTorque = BrakeForce;
            F_R_Wheel.brakeTorque = BrakeForce;
        }

        if (CarStatus == "Drive")
        {

            BreakeLight.SetActive(false);
            F_L_Wheel.brakeTorque = 0;
            F_R_Wheel.brakeTorque = 0;


        }


        //\\\\\\\\\\\TYPE SELECT //////////////////////////////

        if (CarType == CType2.OldCar)
        {
            EngineMoment = EngineMoment_OldCar;
            SteeringWheel = 15;
            MaxSpeed = 25;
        }

        if (CarType == CType2.City)
        {
            EngineMoment = EngineMoment_City;
            SteeringWheel = 15;
            //MaxSpeed = 45;
        }

        if (CarType == CType2.HighWay)
        {
            EngineMoment = EngineMoment_HighWay;
            SteeringWheel = 15;
            MaxSpeed = 80;
        }

        if (CarType == CType2.SportCar)
        {
            EngineMoment = EngineMoment_Sport;
            SteeringWheel = 15;
            MaxSpeed = 120;
        }
        if (CarType == CType2.Custom)
        {
            EngineMoment = EngineMoment_Custom;
            SteeringWheel = 15;

        }

        if (B_L_Wheel.rpm == 0)
        {
        }

        if (B_L_Wheel.rpm < 1)
        {
        }

        F_L_Wheel.motorTorque = EngineMoment;
        F_R_Wheel.motorTorque = EngineMoment;
        F_L_Wheel.steerAngle = 40 * inputSteer;
        F_R_Wheel.steerAngle = 40 * inputSteer;


        //\\\\\\\\\\\MODE SELECT //////////////////////////////


        if (Mode == CMode.None)
        {
            CarStatus = "Drive";
            Navigate_to_waypoint();
        }

        if (Mode == CMode.Sample)
        {
            Sample();
        }

        if (Mode == CMode.Advanced)
        {
            Adv();
        }

        if (Mode == CMode.Professional)
        {
            Pro();
        }


    }




    void Sample() //Simple mode
    {

        Vector3 fwd = transform.TransformDirection(Vector3.forward) * side_ray;
        Vector3 fwd2 = transform.TransformDirection(Vector3.forward) * breake_far;


        Debug.DrawRay(transform.position, fwd2, Color.yellow); //Draw rays (only visible under the scene window)
        if (Physics.Raycast(transform.position, fwd2, breake_far)) //"Sense ray" detected obstacles
        {
            Debug.DrawRay(transform.position, fwd2, Color.red);

            CarStatus = "Stop"; 
        }
        else
        {

            CarStatus = "Drive";
            Navigate_to_waypoint();
        }
    }



    void Adv()
    {

        Vector3 fwd = transform.TransformDirection(Vector3.forward) * side_ray; 
        Vector3 fwd2 = transform.TransformDirection(Vector3.forward) * breake_far;


        Debug.DrawRay(transform.position, Quaternion.AngleAxis(7, transform.up) * fwd2, Color.yellow);
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(7, transform.up) * fwd2, breake_far)) //"Sense ray" detected obstacles
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(7, transform.up) * fwd2, Color.red);
            CarStatus = "Stop";
            F_L_Wheel.brakeTorque = BrakeForce;
            F_R_Wheel.brakeTorque = BrakeForce;

        }


        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-7, transform.up) * fwd2, Color.yellow);
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-7, transform.up) * fwd2, breake_far)) //Obstructions on the side
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(-7, transform.up) * fwd2, Color.red);
            CarStatus = "Stop";
            F_L_Wheel.brakeTorque = BrakeForce;
            F_R_Wheel.brakeTorque = BrakeForce;

        }


        Debug.DrawRay(transform.position, fwd2, Color.yellow);
        if (Physics.Raycast(transform.position, fwd2, breake_far)) //Obstructions in front
        {
            Debug.DrawRay(transform.position, fwd2, Color.red);

            CarStatus = "Stop";
        }
        else
        {

            CarStatus = "Drive";
            Navigate_to_waypoint();
        }
    }


    void Pro()
    {

        Vector3 fwd = transform.TransformDirection(Vector3.forward) * side_ray; 
        Vector3 fwd2 = transform.TransformDirection(Vector3.forward) * breake_far;

        Debug.DrawRay(transform.position, Quaternion.AngleAxis(15, transform.up) * fwd, Color.white);
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(15, transform.up) * fwd, breake_far)) //Turn left to avoid obstacles
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(15, transform.up) * fwd, Color.red);
            F_L_Wheel.steerAngle = -40 * inputSteer;
            F_R_Wheel.steerAngle = -40 * inputSteer;

        }


        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-15, transform.up) * fwd, Color.white);
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-15, transform.up) * fwd, breake_far)) //Turn right to avoid obstacles
        {
            F_L_Wheel.steerAngle = 40 * inputSteer;
            F_R_Wheel.steerAngle = 40 * inputSteer;

        }


        Debug.DrawRay(transform.position, Quaternion.AngleAxis(5, transform.up) * fwd2, Color.yellow);
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(5, transform.up) * fwd2, breake_far))
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(5, transform.up) * fwd2, Color.red);
            CarStatus = "Stop";


        }


        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-5, transform.up) * fwd2, Color.yellow);
        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-5, transform.up) * fwd2, breake_far))
        {
            Debug.DrawRay(transform.position, Quaternion.AngleAxis(-5, transform.up) * fwd2, Color.red);
            CarStatus = "Stop";

        }



        Debug.DrawRay(transform.position, fwd2, Color.yellow);
        if (Physics.Raycast(transform.position, fwd2, breake_far))
        {
            Debug.DrawRay(transform.position, fwd2, Color.red);

            CarStatus = "Stop";

        }
        else
        {

            CarStatus = "Drive";
            Navigate_to_waypoint();
        }
    }




    void GotWaypoints()
    {
        Transform[] potentialWaypoints = waypoint.GetComponentsInChildren<Transform>(); //get waypoints' transform

        waypoints = new Transform[potentialWaypoints.Length]; //waypoint buffer
        Debug.Log(waypoints.Length);
        foreach (Transform potentialWaypoint in potentialWaypoints) //Update current waypoint
        {
            if (potentialWaypoint != waypoint.transform)
            {
                waypoints[waypoints.Length] = potentialWaypoint; 
            }
        }
    }

    void Navigate_to_waypoint()
    {
        if (CarStatus == "Drive")
        {
            Transform[] waypoints;
            GameObject closest;
            float closestDist = Mathf.Infinity; //was not clear
            waypoints = waypoint.GetComponentsInChildren<Transform>(); 
            Vector3 RPWaypoint = transform.InverseTransformPoint(new Vector3(waypoints[currentWaypoint].position.x, transform.position.y, waypoints[currentWaypoint].position.z));

            float dist = (transform.position - waypoint.transform.position).sqrMagnitude; //Distance to wp

            if (dist < closestDist)
            {
                closestDist = dist;
                closest = waypoint;
            }
            inputSteer = RPWaypoint.x / RPWaypoint.magnitude;

            if (Mathf.Abs(inputSteer) < 1.0f)
            {
                inputTorque = RPWaypoint.z / RPWaypoint.magnitude - Mathf.Abs(inputSteer);
            }
            else
            {
                inputTorque = 0.0f;

            }
            if (RPWaypoint.magnitude < 10) //Start braking at this distance from WP (was 25)
            {
                F_L_Wheel.brakeTorque = 400; //Brake torque
                F_R_Wheel.brakeTorque = 400;
                currentWaypoint++;

                if (currentWaypoint >= waypoints.Length)
                {
                    currentWaypoint = 1;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Stop" || collision.gameObject.tag == "Player")
        {
            CarStatus = "Stop";
        }
        else
        {
            CarStatus = "Drive";
        }
    }
}