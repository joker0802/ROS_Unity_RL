using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.sensor_msgs;

public class Pose_Subscriber_0 : ROSBridgeSubscriber
{
    public static PoseMsg ros_pose_0;

    public new static string GetMessageTopic()
    {
        return "tb3_0/pose";
    }

    public new static string GetMessageType()
    {
        return "geometry_msgs/Pose";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new PoseMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        ros_pose_0 = (PoseMsg)msg;
        //Debug.Log("ros_scan: " + ros_scan);
    }
}
