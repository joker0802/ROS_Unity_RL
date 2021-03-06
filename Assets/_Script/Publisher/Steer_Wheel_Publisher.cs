﻿using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.std_msgs;

public class Steer_Wheel_Publisher : ROSBridgePublisher
{
    // The following three functions are important
    public static new string GetMessageTopic()
    {
        return "steer_wheel"; 
    }

    public static new string GetMessageType()
    {
        return "std_msgs/Float32";
    }

    public static string ToYAMLString(Float32Msg msg)
    {
        return msg.ToYAMLString();
    }

    public static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new Float32Msg(msg);
    }
}
