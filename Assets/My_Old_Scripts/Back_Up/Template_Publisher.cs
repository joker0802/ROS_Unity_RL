using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.geometry_msgs;

// Turtlebot3Burger publisher: 
// Using StringMsg msgs for example
public class Template_Publisher : ROSBridgePublisher
{
    // The following three functions are important
    public static string GetMessageTopic()
    {
        return "player_pose";
    }

    public static string GetMessageType()
    {
        return "geometry_msgs/Twist";
    }

    public static string ToYAMLString(TwistMsg msg)
    {
        return msg.ToYAMLString();
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new TwistMsg(msg);
    }
}




