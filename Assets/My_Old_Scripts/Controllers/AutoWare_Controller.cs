using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;
using ROSBridgeLib.rosgraph_msgs;
using PointCloud;
using PointCloud.PointTypes.Converters;

public class AutoWare_Controller : MonoBehaviour
{
    // ros bridge websocket connection parameter
    private ROSBridgeWebSocketConnection ros = null;

    // game object for the lidar
    public GameObject Lidar;

    // player controller script
    public Lidar_Ours lidar_velodyne;

    public static float timer;

    private int frames = 0;

    void Start()
    {
        // Where the rosbridge instance is running, could be localhost, or some external IP
        ros = new ROSBridgeWebSocketConnection("ws://192.168.1.18", 9090);

        // Add subscribers and publishers (if any)
        ros.AddPublisher(typeof(AutoWare_PointCloud2_Publisher));
        ros.AddPublisher(typeof(less_flat_Publisher));
        ros.AddPublisher(typeof(imu_trans_Publisher));
        ros.AddPublisher(typeof(Joint_States_Publisher));
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
        frames++;
        //Rendering
        ros.Render();

        //============Initial Parameters============//
        //sequence ID
        int Lidar_seq = 0;

        //time stamp
        timer += Time.deltaTime;
        int time_sec = Mathf.RoundToInt(timer);
        int time_nsec = Mathf.RoundToInt((timer - Mathf.Floor(timer)) * 100000000);
        TimeMsg Lidar_stamp = new TimeMsg(time_sec, time_nsec);

        // get the lidar position
        float p_x = Lidar.transform.position.x;
        float p_y = Lidar.transform.position.y;
        float p_z = Lidar.transform.position.z;
        //get the lidar quaternion
        float q_x = Lidar.transform.rotation.x;
        float q_y = Lidar.transform.rotation.y;
        float q_z = Lidar.transform.rotation.z;
        float q_w = -Lidar.transform.rotation.w;

        //set the lidar poses
        PointMsg Lidar_p = new PointMsg(p_x, p_y, p_z);
        QuaternionMsg Lidar_q = new QuaternionMsg(q_x, q_y, q_z, q_w);
        PoseMsg Lidar_pose = new PoseMsg(Lidar_p, Lidar_q);

        //Set the Headers with (seq, time, frame_id);
        HeaderMsg pointcloud_Header = new HeaderMsg(Lidar_seq, Lidar_stamp, "camera"); //velodyne //base_scan
        HeaderMsg joint_Header = new HeaderMsg(Lidar_seq, Lidar_stamp, "player/joints");
        HeaderMsg pose_Header = new HeaderMsg(Lidar_seq, Lidar_stamp, "pose");

        if (frames == 1)
        {
            frames = 0;
            //******************************************************//
            //set the parameters for PointCloud2    
            Our_LidarPointArray out_put_arr = lidar_velodyne.GetOutput();

            //----------data----------//
            //count all valid points
            int point_count = 0;
            foreach (Our_LidarPoint p in out_put_arr.points)
            {
                if (p != null)
                    point_count++;
            }
            //convert points coordinates to byte array
            byte[] pointcloud_data = new byte[0];

            pointcloud_data = PointCloudBytes(out_put_arr, pointcloud_data); 
            //========================//

            //----------height----------//
            uint pointcloud_height = 1;
            //----------width-----------//
            uint pointcloud_width = (uint)point_count;
            //=========================//


            //----------fields----------//
            PointFieldMsg[] pointcloud_fields = new PointFieldMsg[4];
            pointcloud_fields[0] = new PointFieldMsg("x", 0, PointFieldMsg.FLOAT32, 1);
            pointcloud_fields[1] = new PointFieldMsg("z", 4, PointFieldMsg.FLOAT32, 1);
            pointcloud_fields[2] = new PointFieldMsg("y", 8, PointFieldMsg.FLOAT32, 1);
            pointcloud_fields[3] = new PointFieldMsg("intensity", 12, PointFieldMsg.FLOAT32, 1);

            //=========================//

            //----------is_bigendian----------//
            bool pointcloud_is_bigendian = false;
            //-----------point_step-----------//
            uint pointcloud_point_step = 12;
            //------------row_step------------//
            uint pointcloud_row_step = pointcloud_width * pointcloud_point_step;

            //Debug.Log("final check" + pointcloud_row_step);
            //================================//

            //-----------is_dense----------//
            bool pointcloud_is_dense = true;
            //******************************************************//

            //joint data and names
            float[] joint_position = new float[2] { 0, 0 };
            string[] joint_names = new string[2] { "wheel_right_joint", "wheel_left_joint" };
            //Temporarily useless (use for joint states)
            float[] joint_velocity = new float[1];
            float[] joint_effort = new float[1];
            //============set values to the messages============//
            JointStateMsg JointStateMsg = new JointStateMsg(joint_Header, joint_names, joint_position, joint_velocity, joint_effort);
            PoseStampedMsg PoseStampedMsg = new PoseStampedMsg(pose_Header, Lidar_pose);
            PointCloud2Msg pointCloud2Msg = new PointCloud2Msg(pointcloud_Header, pointcloud_height, pointcloud_width, pointcloud_fields,
                pointcloud_is_bigendian, pointcloud_point_step, pointcloud_row_step, pointcloud_data, pointcloud_is_dense);


            //============publish the messages============//
            ros.Publish(Joint_States_Publisher.GetMessageTopic(), JointStateMsg);
            ros.Publish(PoseStamped_Publisher.GetMessageTopic(), PoseStampedMsg);
            ros.Publish(AutoWare_PointCloud2_Publisher.GetMessageTopic(), pointCloud2Msg);
            ros.Publish(less_flat_Publisher.GetMessageTopic(), pointCloud2Msg);
            //ros.Publish(imu_trans_Publisher.GetMessageTopic(), pointCloud2Msg);

        }

        Lidar_seq++;
    }

    //FOR DEBUG
    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        LidarPointArray arr = lidar_velodyne.GetOutput();

        if (arr == null)
            return;

        Vector3 pos = Vector3.zero;

        foreach (LidarPoint p in arr.points)
        {
            if (p == null)
                continue;

            pos.x = p.x;
            pos.y = p.y;
            pos.z = p.z;

            Gizmos.DrawSphere(pos, 0.2f);
        }
    }*/

    public byte[] ArrayCombine(List<byte[]> byteList)
    {
        List<byte> newBytes = new List<byte>();
        foreach (var item in byteList)
        {
            for (int i = 0; i < item.Length; i++)
            {
                newBytes.Add(item[i]);
            }
        }
        return newBytes.ToArray();
    }

    public byte[] PointCloudBytes(Our_LidarPointArray source_arr, byte[] init_data)
    {
        List<byte[]> newBytes = new List<byte[]>();
        newBytes.Add(init_data);

        foreach (Our_LidarPoint p in source_arr.points)
        {
            if (p == null)
                continue;
            byte[] xOut = System.BitConverter.GetBytes(p.x);
            byte[] yOut = System.BitConverter.GetBytes(p.y);
            byte[] zOut = System.BitConverter.GetBytes(p.z);

            newBytes.Add(xOut);
            newBytes.Add(yOut);
            newBytes.Add(zOut);
        }
        //Debug.Log("here");
        init_data = newBytes.SelectMany(bytes => bytes).ToArray();
        //init_data = ArrayCombine(newBytes);
        return init_data;
    }
}
