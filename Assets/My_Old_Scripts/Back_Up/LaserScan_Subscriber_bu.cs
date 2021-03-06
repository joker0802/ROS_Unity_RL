using UnityEngine;
using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.sensor_msgs;

public class LaserScan_Subscriber_bu : ROSBridgeSubscriber
{
    public static LaserScanMsg scan_data;

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
        scan_data = (LaserScanMsg)msg;
        //float RM = scan_data.GetRangeMax();RM.ToString()
        Debug.Log("scan: " + scan_data);
    }
}
