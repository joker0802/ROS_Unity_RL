using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserScan_Visualizer : MonoBehaviour {

    private float angle_max;
    private float angle_min;
    private float angle_inc;
    private float[] ranges;

    private int points_num;
    private float angle = 0;

    public LaserScan_Visualizer(float a_max, float a_min, float a_inc, float[] r)
    {
        angle_max = a_max;
        angle_min = a_min;
        angle_inc = a_inc;
        ranges = r;
        points_num = 360;

        Debug.Log(ranges[1].ToString());

        for (int i = 0; i < points_num; i++)
        {
            float x = ranges[i] * Mathf.Sin(angle);
            float z = ranges[i] * Mathf.Cos(angle);
            angle += angle_inc;

            Vector3 points_pos = transform.TransformPoint(new Vector3(z, 0, x));
            Debug.DrawLine(transform.position, points_pos, Color.red);
        }
    }
}
