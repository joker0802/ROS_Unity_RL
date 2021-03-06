using System.Collections;
using System.Text;
using SimpleJSON;
using UnityEngine;
using ROSBridgeLib.std_msgs;

/**
 * Message for stamped transform
 * http://docs.ros.org/jade/api/geometry_msgs/html/msg/Transform.html
 * 
 * @author Yuzhou Liu
 * @version 1.0
 */

namespace ROSBridgeLib
{
    namespace geometry_msgs
    {
        public class TransformMsg : ROSBridgeMsg
        {
            public Vector3Msg _translation;
            public QuaternionMsg _rotation;

            public TransformMsg(JSONNode msg)
            {
                _translation = new Vector3Msg(msg["translation"]);
                _rotation = new QuaternionMsg(msg["rotation"]);
            }

            public TransformMsg(Vector3Msg translation, QuaternionMsg rotation)
            {
                _translation = translation;
                _rotation = rotation;
            }

            public static string GetMessageType()
            {
                return "geometry_msgs/Transform";
            }

            public Vector3Msg GetTranslation()
            {
                return _translation;
            }

            public QuaternionMsg GetRoatation()
            {
                return _rotation;
            }

            public override string ToString()
            {
                return "geometry_msgs/Transform [translation=" + _translation.ToString() + ",  rotation=" + _rotation.ToString() + "]";
            }

            public override string ToYAMLString()
            {
                return "{\"translation\": " + _translation.ToYAMLString() + ", \"rotation\": " + _rotation.ToYAMLString() + "}";
            }
        }
    }
}