using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;
using ROSBridgeLib.rosgraph_msgs;
using UnityEngine;
using System;

[Serializable]
public class LaserPoints
{
    public float x;
    public float y;
    public float z;

    public LaserPoints(Vector3 p)
    {
        x = p.x;
        y = p.y;
        z = p.z;
    }
}

[Serializable]
public class LaserPointsArray
{
    //All coordinates in World coordinate system

    public LaserPoints lidarPos;

    public LaserPoints lidarOrientation;

    public LaserPoints[] points;

    public void Init(int numPoints)
    {
        points = new LaserPoints[numPoints];
    }
}

public class Visualizer_Controller : MonoBehaviour
{
    // ros bridge websocket connection parameter
    private ROSBridgeWebSocketConnection ros_visual = null;

    public GameObject TB3_1;
    public bool DisplayDebugInScene = true;
    //how large radius to use when rendering debug display
    public float gizmoSize = 0.1f;
    public Vector3[] points_pos;

    void Start()
    {
        // Where the rosbridge instance is running, could be localhost, or some external IP
        ros_visual = new ROSBridgeWebSocketConnection("ws://192.168.173.85", 9090);

        // Add subscribers and publishers (if any)
        ros_visual.AddSubscriber(typeof(LaserScan_Subscriber));

        //init tb3
        TB3_1.transform.localScale = new Vector3(0.01f,0.01f,0.01f);
        Instantiate(TB3_1); 

        // Fire up the subscriber(s) and publisher(s)
        ros_visual.Connect();
    }
    // Extremely important to disconnect from ROS. Otherwise packets continue to flow
    void OnApplicationQuit()
    {
        if (ros_visual != null)
        {
            ros_visual.Disconnect();
        }
    }

    void Update()
    {
        //Rendering
        ros_visual.Render();
        Instantiate(TB3_1);
        //Vector3Msg testMsg = (Vector3Msg)Cmd_Vel_Subscriber.ctrl_vel.GetLinear();
        //Debug.Log("x=" + testMsg.GetX());

        //LaserScanMsg from ROS
        LaserScanMsg testMsg = (LaserScanMsg)LaserScan_Subscriber.ros_scan;
        float a_inc = testMsg.GetAngleIncrement();
        float[] r = testMsg.GetRanges();

        float angle = 0;
        int points_num = 360;
        points_pos = new Vector3[points_num];
        int j = 0;

        //TB3_1.transform.position = ;
        //TB3_1.transform.rotation = ;
        for (int i = 0; i < points_num; i++)
        {
            float x = r[i] * Mathf.Sin(angle);
            float z = r[i] * Mathf.Cos(angle);
            angle += a_inc;

            if (r[i] < 3.5)
            {
                points_pos[j] = transform.TransformPoint(new Vector3(z, 0.1f, x));
                j++;
            }
            //Debug.Log(points_pos[i]);
            //Debug.DrawLine(transform.position, points_pos, Color.red);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for(int i = 0; i < points_pos.Length; i++)
        {
            //Debug.Log("points pos: " + points_pos[i]);
            Vector3 pos = points_pos[i];
            Gizmos.DrawSphere(pos, gizmoSize);
         
        }
    }

    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {

        if (DisplayDebugInScene == false)
            return;

        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        //GL.MultMatrix (transform.localToWorldMatrix);
        GL.MultMatrix(Matrix4x4.identity);
        // Draw lines
        GL.Begin(GL.LINES);

        //red
        GL.Color(new Color(1, 0, 0, 0.8F));
        for (int i = 0; i < points_pos.Length; i++)
        {
            Vector3 pos = points_pos[i];
            GL.Vertex3(pos.x, pos.y, pos.z);
            GL.Vertex3(pos.x, pos.y + gizmoSize, pos.z);
        }
        GL.End();
        GL.PopMatrix();
    }

}