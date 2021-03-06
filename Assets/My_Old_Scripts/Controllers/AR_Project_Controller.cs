using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;
using ROSBridgeLib.rosgraph_msgs;

public class AR_Project_Controller : MonoBehaviour {

    // ros bridge websocket connection parameter
    private ROSBridgeWebSocketConnection ros = null;

    // game object for the player
    public GameObject tb3_0;
    public GameObject tb3_1;

    //public My_Controller my_controller;
    public Laser_Scanner laser_scanner_0;
    public Laser_Scanner laser_scanner_1;

    public static float timer;

    void Start()
    {
        // Where the rosbridge instance is running, could be localhost, or some external IP
        ros = new ROSBridgeWebSocketConnection("ws://192.168.137.85", 9090);

        // Add subscribers and publishers (if any)
        ros.AddPublisher(typeof(LaserScan_Publisher_0));
        ros.AddPublisher(typeof(Joint_States_Publisher_0));
        ros.AddPublisher(typeof(PoseStamped_Publisher_0));
        ros.AddPublisher(typeof(LaserScan_Publisher_1));
        ros.AddPublisher(typeof(Joint_States_Publisher_1));
        ros.AddPublisher(typeof(PoseStamped_Publisher_1));

        ros.AddSubscriber(typeof(LaserScan_Subscriber_0));
        ros.AddSubscriber(typeof(Pose_Subscriber_0));

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

        //============Initial Parameters============//
        //sequence ID
        int tb3_seq = 0;

        //time stamp
        timer += Time.deltaTime;
        int time_sec = Mathf.RoundToInt(timer);
        int time_nsec = Mathf.RoundToInt((timer - Mathf.Floor(timer)) * 100000000);
        TimeMsg tb3_stamp = new TimeMsg(time_sec, time_nsec);

        // get the tb3_0 position
        float p_0_x = tb3_0.transform.position.x-11; //shift left -11
        float p_0_y = tb3_0.transform.position.y;
        float p_0_z = tb3_0.transform.position.z;
        //get the tb3_0  quaternion
        float q_0_x = tb3_0.transform.rotation.x;
        float q_0_y = tb3_0.transform.rotation.y;
        float q_0_z = tb3_0.transform.rotation.z;
        float q_0_w = -tb3_0.transform.rotation.w;
        //set the tb3_0 poses
        PointMsg tb3_0_p = new PointMsg(p_0_x, p_0_y, p_0_z);
        QuaternionMsg tb3_0_q = new QuaternionMsg(q_0_x, q_0_y, q_0_z, q_0_w);
        PoseMsg tb3_0_pose = new PoseMsg(tb3_0_p, tb3_0_q);

        // get the tb3_1 position
        float p_1_x = tb3_1.transform.position.x;
        float p_1_y = tb3_1.transform.position.y;
        float p_1_z = tb3_1.transform.position.z;
        //get the tb3_1  quaternion
        float q_1_x = tb3_1.transform.rotation.x;
        float q_1_y = tb3_1.transform.rotation.y;
        float q_1_z = tb3_1.transform.rotation.z;
        float q_1_w = -tb3_1.transform.rotation.w;
        //set the tb3_1 poses
        PointMsg tb3_1_p = new PointMsg(p_1_x, p_1_y, p_1_z);
        QuaternionMsg tb3_1_q = new QuaternionMsg(q_1_x, q_1_y, q_1_z, q_1_w);
        PoseMsg tb3_1_pose = new PoseMsg(tb3_1_p, tb3_1_q);

        //Set the Headers with (seq, time, frame_id);
        HeaderMsg tb3_0_Header = new HeaderMsg(tb3_seq, tb3_stamp, "tb3_0/base_scan"); 
        HeaderMsg joint_0_Header = new HeaderMsg(tb3_seq, tb3_stamp, "tb3_0/joints"); 
        HeaderMsg pose_0_Header = new HeaderMsg(tb3_seq, tb3_stamp, "tb3_0/pose"); 

        HeaderMsg tb3_1_Header = new HeaderMsg(tb3_seq, tb3_stamp, "tb3_1/base_scan"); 
        HeaderMsg joint_1_Header = new HeaderMsg(tb3_seq, tb3_stamp, "tb3_1/joints"); 
        HeaderMsg pose_1_Header = new HeaderMsg(tb3_seq, tb3_stamp, "tb3_1/pose"); 

        //set the rest parameters for laser scan
        float tb3_angle_min = 0;
        float tb3_angle_max = 6.28f;
        float tb3_angle_increment = 0.01749f;
        float tb3_time_increment = 0;
        float tb3_scan_time = 0;
        float tb3_range_min = 0.1f; //default: 1.2f
        float tb3_range_max = 3.5f; //default: 3.5f

        //Scan distance data
        float[] tb3_0_ranges = new float[360];
        float[] tb3_1_ranges = new float[360];
        tb3_0_ranges = laser_scanner_0.GetHits();
        tb3_1_ranges = laser_scanner_1.GetHits();

        //joint data and names
        float[] joint_position = new float[2] { 0, 0 };
        string[] joint_names = new string[2] { "wheel_right_joint", "wheel_left_joint" };
        //Temporarily useless (use for joint states)
        float[] tb3_intensities = new float[1];
        float[] joint_velocity = new float[1];
        float[] joint_effort = new float[1];

        //============set values to the messages============//
        LaserScanMsg LaserScanMsg_0 = new LaserScanMsg(tb3_0_Header, tb3_angle_min, tb3_angle_max, tb3_angle_increment, tb3_time_increment, tb3_scan_time, tb3_range_min, tb3_range_max, tb3_0_ranges, tb3_intensities);
        JointStateMsg JointStateMsg_0 = new JointStateMsg(joint_0_Header, joint_names, joint_position, joint_velocity, joint_effort);
        PoseStampedMsg PoseStampedMsg_0 = new PoseStampedMsg(pose_0_Header, tb3_0_pose);

        LaserScanMsg LaserScanMsg_1 = new LaserScanMsg(tb3_1_Header, tb3_angle_min, tb3_angle_max, tb3_angle_increment, tb3_time_increment, tb3_scan_time, tb3_range_min, tb3_range_max, tb3_1_ranges, tb3_intensities);
        JointStateMsg JointStateMsg_1 = new JointStateMsg(joint_1_Header, joint_names, joint_position, joint_velocity, joint_effort);
        PoseStampedMsg PoseStampedMsg_1 = new PoseStampedMsg(pose_1_Header, tb3_1_pose);

        //============publish the messages============//
        ros.Publish(LaserScan_Publisher_0.GetMessageTopic(), LaserScanMsg_0);
        ros.Publish(Joint_States_Publisher_0.GetMessageTopic(), JointStateMsg_0);
        ros.Publish(PoseStamped_Publisher_0.GetMessageTopic(), PoseStampedMsg_0);

        ros.Publish(LaserScan_Publisher_1.GetMessageTopic(), LaserScanMsg_1);
        ros.Publish(Joint_States_Publisher_1.GetMessageTopic(), JointStateMsg_1);
        ros.Publish(PoseStamped_Publisher_1.GetMessageTopic(), PoseStampedMsg_1);

        tb3_seq++;
    }
}
