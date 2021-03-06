using System.Text;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Our_LidarPoint
{
    public float x;
    public float y;
    public float z;

    public Our_LidarPoint(Vector3 p)
    {
        x = p.x;
        y = p.y;
        z = p.z;
    }
}

[Serializable]
public class Our_LidarPointArray
{
    //All coordinates in World coordinate system

    public Our_LidarPoint lidarPos;

    public Our_LidarPoint lidarOrientation;

    public Our_LidarPoint[] points;

    public void Init(int numPoints)
    {
        points = new Our_LidarPoint[numPoints];
    }
}

public class Lidar_Ours : MonoBehaviour
{
    //public GameObject Lidar_self;
    // Number of layers.
    public int Layers = 3;

    //as the ray sweeps around, how many degrees does it advance per sample
    public int degPerSweepInc = 1;

    // vertical fov (in degrees).
    public float MaximumVerticalFov = 15;
    public float MinimumVerticalFov = -15;

    // Maximum range of detection (in meters).
    public float MaxRange = 50f;
    //how large radius to use when rendering debug display
    public float gizmoSize = 0.1f;

    Our_LidarPointArray pointArr;

    void Awake()
    {
        pointArr = new Our_LidarPointArray();
        pointArr.Init((360 / degPerSweepInc) * Layers);
    }


    public Our_LidarPointArray GetOutput()
    {
        pointArr = new Our_LidarPointArray();
        pointArr.Init((360 / degPerSweepInc) * Layers);

        pointArr.lidarPos = new Our_LidarPoint(transform.position);
        pointArr.lidarOrientation = new Our_LidarPoint(transform.rotation.eulerAngles);

        Ray ray = new Ray();

        ray.origin = this.transform.position;
        ray.direction = this.transform.forward;

        float defAngDelta = (MaximumVerticalFov - MinimumVerticalFov) / Layers;

        //start out pointing a bit up.
        Quaternion rotInit = Quaternion.AngleAxis(-MaximumVerticalFov, transform.right);
        ray.direction = rotInit * ray.direction;

        int numSweep = 360 / degPerSweepInc;

        Quaternion rotSide = Quaternion.AngleAxis(degPerSweepInc, transform.up);
        Quaternion rotDown = Quaternion.AngleAxis(defAngDelta, transform.right);

        RaycastHit hit;

        //Sample the output texture to create rays.
        int iP = 0;

        for (int iS = 0; iS < Layers; iS++)
        {
            for (int iA = 0; iA < numSweep; iA++)
            {
                if (Physics.Raycast(ray, out hit, MaxRange))
                {
                    //Debug.DrawRay(transform.position, ray.direction * 15, Color.green);
                    //sample that ray at the depth given by the pixel.
                    Vector3 pos = ray.GetPoint(hit.distance);
                    //shouldn't hit this unless user is messing around in the interface with things running.
                    if (iP >= pointArr.points.Length)
                        break;

                    pos.x -= transform.position.x;
                    //pos.y -= transform.position.y;
                    pos.z -= transform.position.z;
                    pos = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.down) * pos;

                    //set iPoint
                    pointArr.points[iP] = new Our_LidarPoint(pos);

                    ray.direction = rotSide * ray.direction;
                    iP++;
                }
                else
                {
                    ray.direction = rotSide * ray.direction;
                    iP++;
                }
            }

            ray.direction = rotDown * ray.direction;
        }

        return pointArr;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Our_LidarPointArray arr = GetOutput();

        if (arr == null)
            return;

        Vector3 pos = Vector3.zero;
        Vector3 dir = Vector3.zero;

        foreach (Our_LidarPoint p in arr.points)
        {
            Vector3 pivot = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            if (p == null)
                continue;

            pos.x = p.x + transform.position.x;
            pos.y = p.y;
            pos.z = p.z + transform.position.z;

            dir = pos - pivot;
            dir = Quaternion.Euler(transform.rotation.eulerAngles) * dir;
            pos = dir + pivot;

            Gizmos.DrawSphere(pos, gizmoSize);
        }
    }
}
