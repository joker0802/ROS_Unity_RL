using UnityEngine;
using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.std_msgs;

public class Reset_Subscriber : ROSBridgeSubscriber
{
    public static BoolMsg reset;

    public new static string GetMessageTopic()
    {
        return "reset";
    }

    public new static string GetMessageType()
    {
        return "std_msgs/Bool";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new BoolMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        //TwistMsg cmd_vel_msg = (TwistMsg)msg;
        reset = (BoolMsg)msg;
    }
}
