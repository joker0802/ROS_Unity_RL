using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.sensor_msgs;


public class PoseStamped_Publisher_1 : ROSBridgePublisher
{
    // The following three functions are important
    public static string GetMessageTopic()
    {
        return "tb3_1/pose";
    }

    public static string GetMessageType()
    {
        return "geometry_msgs/PoseStamped";
    }

    public static string ToYAMLString(PoseStampedMsg msg)
    {
        return msg.ToYAMLString();
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new PoseStampedMsg(msg);
    }
}
