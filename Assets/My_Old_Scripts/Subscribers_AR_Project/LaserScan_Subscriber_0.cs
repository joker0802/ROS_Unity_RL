using UnityEngine;
using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.sensor_msgs;

public class LaserScan_Subscriber_0 : ROSBridgeSubscriber
{
    public static LaserScanMsg ros_scan_0;

    public new static string GetMessageTopic()
    {
        return "tb3_0/scan";
    }

    public new static string GetMessageType()
    {
        return "sensor_msgs/LaserScan";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new LaserScanMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        ros_scan_0 = (LaserScanMsg)msg;

        //Debug.Log("ros_scan: " + ros_scan);
    }
}

