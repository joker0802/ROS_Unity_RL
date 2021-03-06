using UnityEngine;
using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.sensor_msgs;

public class LaserScan_Subscriber : ROSBridgeSubscriber
{
    public static LaserScanMsg ros_scan;

    public new static string GetMessageTopic()
    {
        return "scan";
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
        ros_scan = (LaserScanMsg)msg;

        //Debug.Log("ros_scan: " + ros_scan);
    }
}
