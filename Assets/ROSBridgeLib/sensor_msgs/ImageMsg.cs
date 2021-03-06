using SimpleJSON;
using ROSBridgeLib.std_msgs;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * Define a Image message.
 *  
 * @author Mathias Ciarlo Thorstensen
 */

namespace ROSBridgeLib {
    namespace sensor_msgs {
        public class ImageMsg : ROSBridgeMsg {
            private HeaderMsg _header;
            private uint _height;
            private uint _width;
            private string _encoding;
            private uint _is_bigendian;
            private uint _step;
            private byte[] _data;

            public ImageMsg(JSONNode msg) {
                _header = new HeaderMsg (msg ["header"]);
                _height = uint.Parse(msg ["height"]);
                _width = uint.Parse(msg ["width"]);
                _encoding = msg ["encoding"];
                _is_bigendian = uint.Parse(msg["is_bigendian"]);
                _step = uint.Parse(msg ["step"]);
                _data = System.Convert.FromBase64String(msg["data"]);
            }

            public ImageMsg(HeaderMsg header, uint height, uint width, string encoding, uint is_bigendian, uint step, byte[] data) {
                _header = header;
                _height = height;
                _width = width;
                _encoding = encoding;
                _is_bigendian = is_bigendian;
                _step = step;
                _data = data;
            }

            public HeaderMsg GetHeader()
            {
                return _header;
            }

            public uint GetWidth() {
                return _width;
            }

            public uint GetHeight() {
                return _height;
            }
                
            public uint GetStep() {
                return _step;
            }

            public byte[] GetImage() {
                return _data;
            }

            public static string GetMessageType() {
                return "sensor_msgs/Image";
            }

            public override string ToString() {
                return "Image [header=" + _header.ToString() +
                    "height=" + _height +
                    "width=" + _width +
                    "encoding=" + _encoding +
                    "is_bigendian=" + _is_bigendian +
                    "step=" + _step +
                    "data=" + _data.ToString() + "]";
            }

            public override string ToYAMLString() {

                //==new code==
                /*byte commaByte = (byte)',';
               
                byte[] _datanew = new byte[(_data.Length) * 2 - 1];

                int j = 0;
                for (int i = 0; i < _data.Length; i++)
                {
                    if (_data.Length - i > 1)
                    {
                        _datanew[j] = _data[i];
                        j++;
                        _datanew[j] = commaByte;
                        j++;
                    }
                    else
                    {
                        _datanew[j] = _data[i];
                    }
                }
                string data_sb = System.Text.Encoding.UTF8.GetString(_datanew, 0, _data.Length);*/

                //==old code==
                StringBuilder data_sb = new StringBuilder();
                data_sb.Append("[");
                for (int i = 0; i < _data.Length; i++)
                {
                    data_sb.Append(_data[i]);
                    if (_data.Length - i > 1)
                        data_sb.Append(",");
                }
                data_sb.Append("]");

                return "{\"header\" : " + _header.ToYAMLString() +
                    ", \"height\" : " + _height +
                    ", \"width\" : " + _width +
                    ", \"encoding\" : \"" + _encoding +
                    "\", \"is_bigendian\" : " + _is_bigendian +
                    ", \"step\" : " + _step +
                    //", \"data\" : [" + data_string + "]" +
                    ", \"data\" : " + data_sb.ToString() +
                    "}"; 

            }
        }
    }
}
