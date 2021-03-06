using SimpleJSON;
using ROSBridgeLib.std_msgs;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

/**
 * Message for LDS laser scanner
 * http://docs.ros.org/kinetic/api/sensor_msgs/html/msg/LaserScan.html
 * 
 * @author Yuzhou Liu
 * @version 1.0
 */

namespace ROSBridgeLib
{
    namespace sensor_msgs
    {
        public class LaserScanMsg : ROSBridgeMsg
        {
            private HeaderMsg _header;

            private float _angle_min;
            private float _angle_max;
            private float _angle_increment;

            private float _time_increment;

            private float _scan_time;

            private float _range_min;
            private float _range_max;

            private float[] _ranges;
            private float[] _intensities;

            private static Regex floats = new Regex("[^-0-9.,e]");
            private static Regex infinities = new Regex("null");//not "inf" =.= shit

            public LaserScanMsg(JSONNode msg)
            {
                //Debug.Log(msg);
                _header = new HeaderMsg(msg["header"]);

                _angle_min = float.Parse(msg["angle_min"]);
                _angle_max = float.Parse(msg["angle_max"]);
                _angle_increment = float.Parse(msg["angle_increment"]);

                _time_increment = float.Parse(msg["time_increment"]);

                _scan_time = float.Parse(msg["scan_time"]);

                _range_min = float.Parse(msg["range_min"]);
                _range_max = float.Parse(msg["range_max"]);

                string r_string_conv_inf = infinities.Replace(msg["ranges"].ToString(), _range_max.ToString()); //convert infinity to max range
                //Debug.Log(r_string_conv_inf);
                string[] r_strings = floats.Replace(r_string_conv_inf, "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (r_strings.Length > 0)
                {
                    _ranges = Array.ConvertAll(r_strings, float.Parse);
                    //Debug.Log(_ranges.Length);
                }

                string[] i_strings = floats.Replace(msg["intensities"].ToString(), "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (i_strings.Length > 0)
                {
                    _intensities = Array.ConvertAll(i_strings, float.Parse);
                }
            }
            //public LaserScanMsg(object header1, HeaderMsg header, float angle_min, float angle_max, float angle_increment, float time_increment, float scan_time, float range_min, float range_max, float[] ranges, float[] intensities)
            public LaserScanMsg(HeaderMsg header, float angle_min, float angle_max, float angle_increment, float time_increment, float scan_time, float range_min, float range_max, float[] ranges, float[] intensities)
            {
                _header = header;
                _angle_min = angle_min;
                _angle_max = angle_max;
                _angle_increment = angle_increment;
                _time_increment = time_increment; ;
                _scan_time = scan_time;
                _range_min = range_min;
                _range_max = range_max;
                _ranges = ranges;
                _intensities = intensities;
            }

            public static string GetMessageType()
            {
                return "sensor_msgs/LaserScan";
            }

            public HeaderMsg GetHeader()
            {
                return _header;
            }

            public float GetAngleMin()
            {
                return _angle_min;
            }

            public float GetAngleMax()
            {
                return _angle_max;
            }

            public float GetAngleIncrement()
            {
                return _angle_increment;
            }

            public float GetTimeIncrement()
            {
                return _time_increment;
            }

            public float GetScanTime()
            {
                return _scan_time;
            }

            public float GetRangeMin()
            {
                return _range_min;
            }

            public float GetRangeMax()
            {
                return _range_max;
            }

            public float[] GetRanges()
            {
                return _ranges;
            }

            public float[] GetIntensities()
            {
                return _intensities;
            }

            public override string ToString()
            {
                return "LaserScan [header=" + _header.ToString() +
                    ",  angle_min=" + _angle_min +
                    ",  angle_max=" + _angle_max +
                    ",  angle_increment=" + _angle_increment +
                    ",  time_increment=" + _time_increment +
                    ",  scan_time=" + _scan_time +
                    ",  range_min=" + _range_min +
                    ",  range_max=" + _range_max +
                    ",  ranges=" + _ranges + 
                    ",  intensities=" + _intensities + "]";
            }

            public override string ToYAMLString()
            {
                string range_array = "[";
                for (int i = 0; i < _ranges.Length; i++)
                {
                    range_array = range_array + _ranges[i];
                    if (_ranges.Length - i > 1)
                        range_array += ",";
                }
                range_array += "]";

                return "{\"header\" : " + _header.ToYAMLString() +
                    ", \"angle_min\" : " + _angle_min +
                    ", \"angle_max\" : " + _angle_max +
                    ", \"angle_increment\" : " + _angle_increment +
                    ", \"time_increment\" : " + _time_increment +
                    ", \"scan_time\" : " + _scan_time +
                    ", \"range_min\" : " + _range_min +
                    ", \"range_max\" : " + _range_max + 
                    ", \"ranges\" : " + range_array + "}";
                //", \"intensities\" : " + i_array + "}";
            }
        }
    }
}