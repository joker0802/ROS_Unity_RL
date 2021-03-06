using UnityEngine;
using SimpleJSON;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;

public class Cmd_Vel_Subscriber : ROSBridgeSubscriber
{
    public static TwistMsg ctrl_vel;

    public new static string GetMessageTopic()
    {
        return "cmd_vel";
    }

    public new static string GetMessageType()
    {
        return "geometry_msgs/Twist";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg)
    {
        return new TwistMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg)
    {
        //TwistMsg cmd_vel_msg = (TwistMsg)msg;
        ctrl_vel = (TwistMsg)msg;
 

        //Debug.Log("cmd_vel: " + ctrl_vel);
    }
}
