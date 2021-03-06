using System.Collections;
using System.Text;
using SimpleJSON;

/* 
 * @brief ROSBridgeLib
 * @author LIU
 */

namespace ROSBridgeLib
{
	namespace std_msgs
	{
		public class BoolMsg : ROSBridgeMsg
		{
			private sbyte _data;

			public BoolMsg(JSONNode msg)
			{
				_data = sbyte.Parse(msg["data"]);
			}

			public BoolMsg(sbyte data)
			{
				_data = data;
			}

			public static string GetMessageType()
			{
				return "std_msgs/Bool";
			}

			public sbyte GetData()
			{
				return _data;
			}

			public override string ToString()
			{
				return "Bool [data=" + _data + "]";
			}

			public override string ToYAMLString()
			{
				return "{\"data\" : " + _data + "}";
			}
		}
	}
}
