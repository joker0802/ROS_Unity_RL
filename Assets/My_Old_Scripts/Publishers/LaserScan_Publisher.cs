using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;


public class LaserScan_Publisher : ROSBridgePublisher
{
    // The following three functions are important
    public static string GetMessageTopic()
    {
        return "scan";
    }

    public static string GetMessageType()
    {
        return "sensor_msgs/LaserScan";
    }

    public static string ToYAMLString(LaserScanMsg msg)
    {
        return msg.ToYAMLString();
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new LaserScanMsg(msg);
    }
}
