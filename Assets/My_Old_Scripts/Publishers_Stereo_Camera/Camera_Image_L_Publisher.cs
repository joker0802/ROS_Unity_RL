using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.sensor_msgs;

public class Camera_Image_L_Publisher : ROSBridgePublisher
{
    // The following three functions are important
    public static string GetMessageTopic()
    {
        return "camera"; 
    }

    public static string GetMessageType()
    {
        return "sensor_msgs/Image";
    }

    public static string ToYAMLString(ImageMsg msg)
    {
        return msg.ToYAMLString();
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new ImageMsg(msg);
    }
}

