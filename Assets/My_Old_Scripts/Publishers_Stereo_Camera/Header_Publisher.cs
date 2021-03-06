using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.std_msgs;

public class Header_Publisher : ROSBridgePublisher
{
    public static string GetMessageTopic()
    {
        return "header";
    }

    public static string GetMessageType()
    {
        return "std_msgs/Header";
    }

    public static string ToYAMLString(HeaderMsg msg)
    {
        return msg.ToYAMLString();
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new HeaderMsg(msg);
    }
}
