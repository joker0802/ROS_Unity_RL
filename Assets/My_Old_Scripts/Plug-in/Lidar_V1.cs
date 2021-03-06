using System.Text;
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Lidar_Point
{
    public float x;
    public float y;
    public float z;

    public Lidar_Point(Vector3 p)
    {
        x = p.x;
        y = p.y;
        z = p.z;
    }
}

[Serializable]
public class Point_Cloud_Array
{
    //All coordinates in World coordinate system

    public Lidar_Point lidarPos;

    public Lidar_Point lidarOrientation;

    public Lidar_Point[] points;

    public void Init(int numPoints)
    {
        points = new Lidar_Point[numPoints];
    }
}

public class Lidar_V1 : MonoBehaviour
{
    // Number of layers.
    public int Layers = 3;

    //points per degree in each sweep
    private int PointsPerDegree = 1;

    // vertical fov (in degrees).
    public float MaximumVerticalFov = 15;
    public float MinimumVerticalFov = -15;

    // Maximum range of detection (in meters).
    public float MaxRange = 50f;
    //how large radius to use when rendering debug display
    public float gizmoSize = 0.1f;

    Point_Cloud_Array pointArr;
    Ray ray;
    RaycastHit hit;

    private Vector3 rotationDirection = Vector3.up;
    private float rotationSpeed = 300f;

    private int rotateSteps = 0;

    void Start()
    {
        pointArr = new Point_Cloud_Array();
        pointArr.Init((360 * PointsPerDegree) * Layers);
    }

    void Update()
    {
        //RayCasts();
        for (int i = 0; i < (360 / 1); i++) // 1/n sweep per frame
        {
            RayCasts();
            transform.Rotate(rotationDirection, 1.0f / PointsPerDegree);
            rotateSteps++;
        }
        if (rotateSteps == 360)
        {
            rotateSteps = 0;
            //Debug.Log("1 sweep");
            //GetOutput()
        }
    }

    public void RayCasts()
    {
        ray = new Ray();

        ray.origin = this.transform.position;
        ray.direction = this.transform.forward;

        float defAngDelta = (MaximumVerticalFov - MinimumVerticalFov) / Layers;

        //start out pointing a bit up.
        Quaternion rotInit = Quaternion.AngleAxis(-MaximumVerticalFov, transform.right);
        ray.direction = rotInit * ray.direction;

        //Quaternion rotSide = Quaternion.AngleAxis(degPerSweepInc, transform.up);
        Quaternion rotDown = Quaternion.AngleAxis(defAngDelta, transform.right);

        //RaycastHit hit;

        //Sample the output texture to create rays.
        int iP = 0;

        for (int iS = 0; iS < Layers; iS++)
        {
            if (Physics.Raycast(ray, out hit, MaxRange))
            {
                Debug.DrawRay(transform.position, ray.direction * MaxRange, Color.red);
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
                pointArr.points[iP] = new Lidar_Point(pos);

                //ray.direction = rotSide * ray.direction;
                iP++;
            }
            else
            {
                Debug.DrawRay(transform.position, ray.direction * MaxRange, Color.blue);
                //ray.direction = rotSide * ray.direction;
                iP++;
            }
            ray.direction = rotDown * ray.direction;
        }
    }

    /*void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Point_Cloud_Array arr = GetOutput();

        if (arr == null)
            return;

        Vector3 pos = Vector3.zero;
        Vector3 dir = Vector3.zero;

        foreach (Lidar_Point p in arr.points)
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
            //Gizmos.DrawLine(transform.position, pos);
        }
    }*/
}
