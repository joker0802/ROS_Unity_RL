using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.rosgraph_msgs;


public class Clock_Publisher : ROSBridgePublisher
{
    // The following three functions are important
    public static string GetMessageTopic()
    {
        return "clock";
    }

    public static string GetMessageType()
    {
        return "rosgraph_msgs/Clock";
    }

    public static string ToYAMLString(ClockMsg msg)
    {
        return msg.ToYAMLString();
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new ClockMsg(msg);
    }
}