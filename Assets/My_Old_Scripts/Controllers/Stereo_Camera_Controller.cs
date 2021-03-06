using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ROSBridgeLib;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;


public class Stereo_Camera_Controller : MonoBehaviour
{
    private ROSBridgeWebSocketConnection ros = null;

    public Camera ImageCamera_L;
    public int resolutionWidth = 640;
    public int resolutionHeight = 480;
    public int defaultDepth = 24;

    private Texture2D texture2D;

    static float timer;
    static int seq = 0;

    void Start()
    {
        // Where the rosbridge instance is running, could be localhost, or some external IP
        ros = new ROSBridgeWebSocketConnection("ws://192.168.1.25", 9090);

        // Add subscribers and publishers (if any)
        ros.AddPublisher(typeof(Camera_Image_L_Publisher));

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

        //============Header Parameters============//
        //time stamp
        timer += Time.deltaTime;
        int time_sec = Mathf.RoundToInt(timer);
        int time_nsec = Mathf.RoundToInt((timer - Mathf.Floor(timer)) * 100000000);
        TimeMsg time_stamp = new TimeMsg(time_sec, time_nsec);

        //Set the Headers with (seq, time, frame_id);
        HeaderMsg Header_L = new HeaderMsg(seq, time_stamp, "left_camera");

        //============Message Parameters============//
        uint Height_L = (uint)resolutionHeight;
        uint Width_L = (uint)resolutionWidth;
        string Encoding_L = "rgb8";
        uint Is_bigendian_L = 0;

        RenderTexture rt = new RenderTexture(resolutionWidth, resolutionHeight, defaultDepth);
        ImageCamera_L.targetTexture = rt;
        Texture2D frameTexture = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
        ImageCamera_L.Render();
        RenderTexture.active = rt;
        frameTexture.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);
        ImageCamera_L.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] Data_L = frameTexture.GetRawTextureData();
        //byte[] Data_L = frameTexture.EncodeToPNG();
        Debug.Log(Data_L.Length);
        uint Step_L = (uint)Data_L.Length/ Height_L;

        ImageMsg ImageMsg_L = new ImageMsg(Header_L, Height_L, Width_L, Encoding_L, Is_bigendian_L, Step_L, Data_L);

        //============publish the messages============//
        ros.Publish(Camera_Image_L_Publisher.GetMessageTopic(), ImageMsg_L);

        seq++;
    }

}
