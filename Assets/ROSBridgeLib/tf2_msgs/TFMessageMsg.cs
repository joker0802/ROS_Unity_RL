using SimpleJSON;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.geometry_msgs;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

/**
 * Message for TFMessage
 * http://docs.ros.org/jade/api/tf2_msgs/html/msg/TFMessage.html
 * 
 * @author Yuzhou Liu
 * @version 1.0
 */

namespace ROSBridgeLib
{
    namespace tf2_msgs
    {
        public class TFMessageMsg : ROSBridgeMsg
        {
            private TransformStampedMsg[] _transforms;

            public TFMessageMsg(JSONNode msg)
            {
                _transforms = new TransformStampedMsg[msg["transforms"].Count];
                for (int i = 0; i < _transforms.Length; i++)
                {
                    _transforms[i] = new TransformStampedMsg(msg["transforms"][i]);
                    //Debug.Log(_transforms[i].ToString());
                }
            }
          
            public TFMessageMsg(TransformStampedMsg[] transforms)
            {
                _transforms = transforms;
            }

            public static string GetMessageType()
            {
                return "geometry_msgs/TFMessage";
            }



            public override string ToString()
            {
                return "TransformStamped [transforms=" + _transforms.ToString() + "]";
            }

            public override string ToYAMLString()
            {
                string tf_array = "[";
                for (int i = 0; i < _transforms.Length; i++)
                {
                    tf_array = tf_array + _transforms[i];
                    if (_transforms.Length - i > 1)
                        tf_array += ",";
                }
                tf_array += "]";

                return "{\"transforms\" : " + tf_array + "}";
            }
        }
    }
}
