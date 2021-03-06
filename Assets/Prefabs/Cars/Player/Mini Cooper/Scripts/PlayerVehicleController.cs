using UnityEngine;
using UnityEngine.UI;

public class PlayerVehicleController : MonoBehaviour
{
    // Each of the four wheels of the car
    public WheelCollider WheelFL, WheelFR, WheelRL, WheelRR;
    public Transform WheelFLTrans, WheelFRTrans, WheelRLTrans, WheelRRTrans;

    // Parameters for adjust the gear sound
    private float BaseEngineSoundPitch;
    public int[] MaxSpeedForGears;

    // The steering wheel component
    public Transform SteeringWheel;

    // Different boundaries for this vehicle
    //private float MaxForwardSpeed = 200;
    //private float MaxRearSpeed = 28;
    private float MaxTorque = 2000;
    private float lowSpeedSteerAngle = 25;
    private float highSpeedSteerAngle = 5;

    // Speed of the odometer control panel
    public Text ControlPanelSpeed;  // TODO The control panel behavior can be extracted to its own class

    // Parameters which I don't know where they come
    private float currentSteerAngle;
    private float lowestSteerAtSpeed = 100;
    private float anglewheel = 0;

    // game object for the first person camera
    private GameObject firstPersonCamera;
    // game object for the third person camera
    private GameObject thirdPersonCamera;

    // Speed (in km/h) of the car
    public float Speed
    {
        get
        {
            return GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        }
    }

    void Start()
    {
        // get the first person camera
        firstPersonCamera = GameObject.FindGameObjectWithTag("FirstPersonCamera");
        // get the third person camera
        thirdPersonCamera = GameObject.FindGameObjectWithTag("ThirdPersonCamera");

        GetComponent<Rigidbody>().centerOfMass += new Vector3(0, -.75f, .25f);
        BaseEngineSoundPitch = GetComponent<AudioSource>().pitch;

        UpdateControlPanel();
    }

    public void UpdateControlPanel()
    {
        ControlPanelSpeed.text = Mathf.Round(Speed).ToString();
    }

    void Update()
    {
        AdjustEngineSound();

        // front wheels rotation
        WheelFLTrans.Rotate(-WheelFL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        WheelFRTrans.Rotate(WheelFR.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        // rear wheels rotation
        WheelRLTrans.Rotate(-WheelRL.rpm / 60 * 360 * Time.deltaTime, 0, 0);
        WheelRRTrans.Rotate(WheelRR.rpm / 60 * 360 * Time.deltaTime, 0, 0);

        // steering
        Vector3 wfl = WheelFLTrans.localEulerAngles;
        wfl.y = WheelFL.steerAngle - anglewheel;
        WheelFLTrans.localEulerAngles = wfl;

        Vector3 wfr = WheelFRTrans.localEulerAngles;
        wfr.y = WheelFR.steerAngle + anglewheel;
        WheelFRTrans.localEulerAngles = wfr;
        
        // steering wheel rotation
        Vector3 stw = SteeringWheel.localEulerAngles;
        stw.z = -10 * WheelFR.steerAngle;
        SteeringWheel.localEulerAngles = stw;
    }

    private void AdjustEngineSound()
    {
        float gearMinValue = 0f;
        float gearMaxValue = 0f;
        int i;
        for (i = 0; i < MaxSpeedForGears.Length; i++)
        {
            if (MaxSpeedForGears[i] > Speed)
            {
                break;
            }
        }
        if (i == 0)
        {
            gearMinValue = 0;
            gearMaxValue = MaxSpeedForGears[i];
        }
        else
        {
            gearMinValue = MaxSpeedForGears[i - 1];
            gearMaxValue = MaxSpeedForGears[i];
        }
        float pitchAdjustment = (Speed - gearMinValue) / (gearMaxValue - gearMinValue);
        float gearFactor = 1.5f * (i + 1);
        GetComponent<AudioSource>().pitch = BaseEngineSoundPitch * gearFactor + pitchAdjustment;
    }

    private void FixedUpdate()
    {
        UpdateControlPanel();

        DriveVehicle();

        float speedFactor = Speed / lowestSteerAtSpeed;
        currentSteerAngle = Mathf.Lerp(lowSpeedSteerAngle, highSpeedSteerAngle, speedFactor);
        currentSteerAngle *= Input.GetAxis("Horizontal") * 1.5f;

        WheelFL.steerAngle = currentSteerAngle;
        WheelFR.steerAngle = currentSteerAngle;
    }

    void DriveVehicle()
    {
        float forwardMotion = 0.0f;

        float motionValue = Input.GetAxis("Vertical");

        if (Input.GetKey("joystick 2 button 7") || Input.GetKey("r"))
        {
            firstPersonCamera.GetComponent<Camera>().enabled = false;
            thirdPersonCamera.GetComponent<Camera>().enabled = true;

            forwardMotion = -1.0f;

            motionValue *= forwardMotion;

            if (motionValue < 0)
                ThrottlePedal(motionValue);
            else
                BrakePedal(motionValue);
        }
        else
        {
            firstPersonCamera.GetComponent<Camera>().enabled = true;
            thirdPersonCamera.GetComponent<Camera>().enabled = false;

            forwardMotion = 1.0f;

            motionValue *= forwardMotion;

            if (motionValue > 0)
                ThrottlePedal(motionValue);
            else
                BrakePedal(motionValue);
        }
    }

    void ThrottlePedal(float motionValue)
    {
        WheelFL.motorTorque = MaxTorque * motionValue;
        WheelFR.motorTorque = MaxTorque * motionValue;
        WheelRL.motorTorque = MaxTorque * motionValue;
        WheelRR.motorTorque = MaxTorque * motionValue;

        WheelFL.brakeTorque = 0.0f;
        WheelFR.brakeTorque = 0.0f;
        WheelRL.brakeTorque = 0.0f;
        WheelRR.brakeTorque = 0.0f;
    }

    void BrakePedal(float motionValue)
    {
        WheelFL.motorTorque = MaxTorque * motionValue;
        WheelFR.motorTorque = MaxTorque * motionValue;
        WheelRL.motorTorque = MaxTorque * motionValue;
        WheelRR.motorTorque = MaxTorque * motionValue;

        WheelFL.brakeTorque = GetComponent<Rigidbody>().mass * 0.01f;
        WheelFR.brakeTorque = GetComponent<Rigidbody>().mass * 0.01f;
        WheelRL.brakeTorque = GetComponent<Rigidbody>().mass * 0.001f;
        WheelRR.brakeTorque = GetComponent<Rigidbody>().mass * 0.001f;
    }

}