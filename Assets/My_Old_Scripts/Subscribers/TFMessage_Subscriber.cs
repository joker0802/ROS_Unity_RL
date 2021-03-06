using UnityEngine;
using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.sensor_msgs;
using ROSBridgeLib.tf2_msgs;

public class TFMessage_Subscriber : ROSBridgeSubscriber
{
    public static TFMessageMsg ros_tf;

    public new static string GetMessageTopic()
    {
        return "tf";
    }

    public new static string GetMessageType()
    {
        return "tf2_msgs/TFMessage";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new TFMessageMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        ros_tf = (TFMessageMsg)msg;
        //Debug.Log("ros_scan: " + ros_scan);
    }
}
