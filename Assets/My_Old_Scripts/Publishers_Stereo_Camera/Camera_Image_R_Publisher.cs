using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.sensor_msgs;

public class Camera_Image_R_Publisher : ROSBridgePublisher
{
    // The following three functions are important
    public static string GetMessageTopic()
    {
        return "Camera_Image_R"; //pointcloud2
    }

    public static string GetMessageType()
    {
        return "sensor_msgs/Image ";
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
