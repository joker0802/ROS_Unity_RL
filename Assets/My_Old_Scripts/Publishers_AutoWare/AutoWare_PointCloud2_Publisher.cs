using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;

public class AutoWare_PointCloud2_Publisher : ROSBridgePublisher
{
    // The following three functions are important
    public static string GetMessageTopic()
    {
        return "velodyne_cloud_2"; //pointcloud2
    }

    public static string GetMessageType()
    {
        return "sensor_msgs/PointCloud2";
    }

    public static string ToYAMLString(PointCloud2Msg msg)
    {
        return msg.ToYAMLString();
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new PointCloud2Msg(msg);
    }
}
