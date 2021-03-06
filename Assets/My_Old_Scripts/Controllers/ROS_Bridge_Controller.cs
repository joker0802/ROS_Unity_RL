using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;
using ROSBridgeLib.rosgraph_msgs;


public class ROS_Bridge_Controller : MonoBehaviour
{
    // ros bridge websocket connection parameter
    private ROSBridgeWebSocketConnection ros = null;

    // game object for the player
    public GameObject TurtleBot;

    // player controller script
    public My_Controller my_controller;
    public Laser_Scanner laser_scanner;

    public static float timer;

    void Start()
    {
        // Where the rosbridge instance is running, could be localhost, or some external IP
        ros = new ROSBridgeWebSocketConnection("ws://10.201.35.192", 9090);
        //ros = new ROSBridgeWebSocketConnection("ws://172.16.0.15", 9090);

        // Add subscribers and publishers (if any)
        ros.AddSubscriber(typeof(Cmd_Vel_Subscriber));
        ros.AddPublisher(typeof(LaserScan_Publisher));
        ros.AddPublisher(typeof(Joint_States_Publisher));
        ros.AddPublisher(typeof(Clock_Publisher));
        ros.AddPublisher(typeof(PoseStamped_Publisher));

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
        //Rendering
        ros.Render();

        //Initial parameters
        //===========================================================================//
        //sequence ID
        int tb3_seq = 0;

        //time stamp
        timer += Time.deltaTime;
        int time_sec = Mathf.RoundToInt(timer);
        int time_nsec = Mathf.RoundToInt((timer - Mathf.Floor(timer)) * 100000000);
        TimeMsg tb3_stamp = new TimeMsg(time_sec, time_nsec);

        // get the tb3 position
        float p_x = TurtleBot.transform.position.x;
        float p_y = TurtleBot.transform.position.y;
        float p_z = TurtleBot.transform.position.z;
        //get the tb3  quaternion
        float q_x = TurtleBot.transform.rotation.x;
        float q_y = TurtleBot.transform.rotation.y;
        float q_z = TurtleBot.transform.rotation.z;
        float q_w = -TurtleBot.transform.rotation.w;//very important!!!
        //set poses
        PointMsg tb3_p = new PointMsg(p_x, p_y, p_z);
		QuaternionMsg tb3_q = new QuaternionMsg(q_x, q_y, q_z, q_w);
        PoseMsg tb3_pose = new PoseMsg(tb3_p, tb3_q); 

        //Set the Headers with (seq, time, frame_id);
        HeaderMsg tb3_Header   = new HeaderMsg(tb3_seq, tb3_stamp, "base_scan"); //scanner
        HeaderMsg joint_Header = new HeaderMsg(tb3_seq, tb3_stamp, "joints"); //joints
        HeaderMsg pose_Header = new HeaderMsg(tb3_seq, tb3_stamp, "pose"); //joints

        //set the rest parameters for laser scan
        float tb3_angle_min = 0;
        float tb3_angle_max = 6.28f;
        float tb3_angle_increment = 0.01749f;
        float tb3_time_increment = 0;
        float tb3_scan_time = 0;
        float tb3_range_min = 0; //default: 1.2f
        float tb3_range_max = 9.5f; //default: 3.5f

        //Scan distance data
        float[] tb3_ranges = new float[360];
        tb3_ranges = laser_scanner.GetHits();

        //joint data and names
        float[] joint_position = new float[2] { 0, 0 };
        //Debug.Log("joints: " + quatR.x + "joints: " + quatL.x);
        string[] joint_names = new string[2]{"wheel_right_joint","wheel_left_joint"};
        //Temporarily useless (use for joint states)
        float[] tb3_intensities = new float[1];
        float[] joint_velocity  = new float[1];
        float[] joint_effort    = new float[1];

        // set the laser scan message 
        LaserScanMsg LaserScanMsg = new LaserScanMsg(tb3_Header, tb3_angle_min, tb3_angle_max, tb3_angle_increment, tb3_time_increment, tb3_scan_time, tb3_range_min, tb3_range_max, tb3_ranges, tb3_intensities);
        // set the joint states message 
        JointStateMsg JointStateMsg = new JointStateMsg(joint_Header, joint_names, joint_position, joint_velocity, joint_effort);
        // set the clock message 
        ClockMsg ClockMsg = new ClockMsg(time_sec, time_nsec);
        // set the pose
        PoseStampedMsg PoseStampedMsg = new PoseStampedMsg(pose_Header, tb3_pose);

        // publish the message
        ros.Publish(LaserScan_Publisher.GetMessageTopic(), LaserScanMsg);
        ros.Publish(Joint_States_Publisher.GetMessageTopic(), JointStateMsg);
        //ros.Publish(Clock_Publisher.GetMessageTopic(), ClockMsg);
        ros.Publish(PoseStamped_Publisher.GetMessageTopic(), PoseStampedMsg);

        tb3_seq++;
        // get player speed
        //float playerSpeed = (float)template_Subscriber.playerSpeed;
        //Vector3Msg testMsg = (Vector3Msg)Cmd_Vel_Subscriber.ctrl_vel.GetLinear();
        //float playerSpeed = (float)testMsg.GetX();

        // update player speed
        //my_controller.MotorForce = playerSpeed;//(playerSpeed == 0) ? 0 : playerSpeed;
        //OnApplicationQuit();
    }
}