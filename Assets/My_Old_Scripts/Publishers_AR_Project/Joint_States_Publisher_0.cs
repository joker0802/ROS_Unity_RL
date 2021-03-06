using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;

public class Joint_States_Publisher_0 : ROSBridgePublisher
{
    // The following three functions are important
    public static string GetMessageTopic()
    {
        return "tb3_0/joint_states";
    }

    public static string GetMessageType()
    {
        return "sensor_msgs/JointState";
    }

    public static string ToYAMLString(JointStateMsg msg)
    {
        return msg.ToYAMLString();
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new JointStateMsg(msg);
    }
}
