﻿using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.sensor_msgs;

public class Camera_L_Publisher : ROSBridgePublisher
{
    // The following three functions are important
    public static new string GetMessageTopic()
    {
        return "cameraL";
    }

    public static new string GetMessageType()
    {
        return "sensor_msgs/Image";
    }

    public static string ToYAMLString(ImageMsg msg)
    {
        return msg.ToYAMLString();
    }

    public static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new ImageMsg(msg);
    }
}

