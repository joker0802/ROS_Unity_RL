using System.Collections;
using System.Text;
using SimpleJSON;
using ROSBridgeLib.std_msgs;

namespace ROSBridgeLib
{
    namespace rosgraph_msgs
    {
        public class ClockMsg : ROSBridgeMsg
        {
            private int _secs, _nsecs;

            public ClockMsg(JSONNode msg)
            {
                _secs = int.Parse(msg["secs"]);
                _nsecs = int.Parse(msg["nsecs"]);
            }

            public ClockMsg(int secs, int nsecs)
            {
                _secs = secs;
                _nsecs = nsecs;
            }

            public static string GetMessageType()
            {
                return "rosgraph_msgs/Clock";
            }

            public int GetSecs()
            {
                return _secs;
            }

            public int GetNsecs()
            {
                return _nsecs;
            }

            public override string ToString()
            {
                return "Clock [secs=" + _secs + ",  nsecs=" + _nsecs + "]";
            }

            public override string ToYAMLString()
            {
                return "{\"clock\" : {\"secs\" : " + _secs + ", \"nsecs\" : " + _nsecs + "}}";
            }
        }
    }
}
