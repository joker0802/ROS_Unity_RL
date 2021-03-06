using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ROSBridgeLib;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;
using System;

public class ROS_BC_Controller : MonoBehaviour
{
    private ROSBridgeWebSocketConnection ros = null;

    public Camera Camera_M;
    public RCC_CarControllerV3 RCC_CarController;

    public int resolutionWidth = 240;
    public int resolutionHeight = 160;
    public int defaultDepth = 24;

    //public RenderTexture rt;
    private Texture2D texture2D;

    static float timer;
    static int seq = 0;

    private float gas = 0;
    private float brake = 0;
    private float steer = 0;
    private int time_sec = 0;
    private int time_nsec = 0;
    private TimeMsg time_stamp;
    private HeaderMsg Header_M;

    private uint Height,Width;
    private string Encoding = "rgb8";
    private uint Is_bigendian = 0;

    void Start()
    {
        Height = (uint)resolutionHeight;
        Width = (uint)resolutionWidth;

        ros = new ROSBridgeWebSocketConnection("ws://192.168.1.108", 9090);

        // Add subscribers and publishers (if any)
        ros.AddPublisher(typeof(Camera_M_Publisher));

        ros.AddPublisher(typeof(Steer_Wheel_Publisher));
        ros.AddPublisher(typeof(Gas_Publisher));
        ros.AddPublisher(typeof(Break_Publisher));

        ros.AddSubscriber(typeof(Reset_Subscriber));
        // Fire up the subscriber(s) and publisher(s)
        ros.Connect();
    }

    // Extremely important to disconnect from ROS. Otherwise packets continue to flow
    void OnApplicationQuit()
    {
        if (ros != null)
        {
            ros.Disconnect();
        }
    }

    // Update is called once per frame in Unity
    void Update()
    {
        ros.Render();//Rendering

        gas = RCC_CarController.gasInput;
        brake = RCC_CarController.brakeInput;
        steer = RCC_CarController.steerInput;
        //============Header Parameters============//
        //time stamp
        timer += Time.deltaTime;
        time_sec = Mathf.RoundToInt(timer);
        time_nsec = Mathf.RoundToInt((timer - Mathf.Floor(timer)) * 100000000);
        time_stamp = new TimeMsg(time_sec, time_nsec);

        //Set the Headers with (seq, time, frame_id);
        Header_M = new HeaderMsg(seq, time_stamp, "m_camera");

        //============Message Parameters============//
        
        RenderTexture rt_m = new RenderTexture(resolutionWidth, resolutionHeight, defaultDepth);
        Camera_M.targetTexture = rt_m;
        Texture2D frameTextureM = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);

        Camera_M.Render();
        RenderTexture.active = rt_m;      
        frameTextureM.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);//see https://docs.unity3d.com/ScriptReference/Texture2D.ReadPixels.html
        Camera_M.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt_m);
        byte[] Data_M = frameTextureM.GetRawTextureData();
        uint Step_M = (uint)Data_M.Length / Height;
        ImageMsg ImageMsg_M = new ImageMsg(Header_M, Height, Width, Encoding, Is_bigendian, Step_M, Data_M);

        //Debug.Log(gas);

        //============publish the messages============//
        ros.Publish(Camera_M_Publisher.GetMessageTopic(), ImageMsg_M);

        ros.Publish(Steer_Wheel_Publisher.GetMessageTopic(), new Float32Msg(steer));
        ros.Publish(Gas_Publisher.GetMessageTopic(), new Float32Msg(gas));
        ros.Publish(Break_Publisher.GetMessageTopic(), new Float32Msg(brake));

        seq++;
    }

    public BoolMsg GetReset()
    {
        return Reset_Subscriber.reset;
    }
}
