using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.sensor_msgs;
using ROSBridgeLib.rosgraph_msgs;
using UnityEngine;
using System;

public class AR_Visual_Controller : MonoBehaviour {

    public GameObject myPrefab;
    private GameObject TB3_1;
    public GameObject TB3_0;
    public bool DisplayDebugInScene = true;
    //how large radius to use when rendering debug display
    public float gizmoSize = 1.1f;
    public Vector3[] points_pos;

    Vector3 tempPose;

    void Start()
    {
        //init tb3
        myPrefab.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        myPrefab.transform.localPosition = new Vector3(0, 0, 0);
        //tempPose = myPrefab.transform.position;
        TB3_1 = (GameObject)Instantiate(myPrefab);
    }

    void Update()
    {
        //PoseMsg from ROS
        //PoseMsg poseMsg_0 = (PoseMsg)Pose_Subscriber_0.ros_pose_0;
        //PointMsg pointMsg_0 = poseMsg_0.GetPosition();
        //QuaternionMsg quaternionMsg_0 = poseMsg_0.GetOrientation();
        
        //LaserScanMsg from ROS
        LaserScanMsg scanMsg_0 = (LaserScanMsg)LaserScan_Subscriber_0.ros_scan_0;
        float a_inc = scanMsg_0.GetAngleIncrement();
        float[] r = scanMsg_0.GetRanges();

        float angle = 0;
        int points_num = 360;
        points_pos = new Vector3[points_num];
        int j = 0;

        // get the tb3_0 position
        float p_0_x = TB3_0.transform.position.x - 11; //shift left -11
        float p_0_y = TB3_0.transform.position.y;
        float p_0_z = TB3_0.transform.position.z;
        //get the tb3_0  quaternion
        float q_0_x = TB3_0.transform.rotation.x;
        float q_0_y = TB3_0.transform.rotation.y;
        float q_0_z = TB3_0.transform.rotation.z;
        float q_0_w = TB3_0.transform.rotation.w;
            
        TB3_1.transform.position = new Vector3(p_0_x, p_0_y, p_0_z);
        TB3_1.transform.rotation = new Quaternion(q_0_x, q_0_y, q_0_z, q_0_w);

        for (int i = 0; i < points_num; i++)
        {
            float x = r[i] * Mathf.Sin(angle);
            float z = r[i] * Mathf.Cos(angle);
            angle += a_inc;

            if (r[i] <= 3.5)
            {
                //points_pos[j] = transform.TransformPoint(new Vector3(z + p_0_x, 0.1f, x + p_0_z));
                points_pos[j] = transform.TransformPoint(new Vector3(z, 0.1f, x));
                points_pos[j] = Quaternion.AngleAxis(TB3_1.transform.eulerAngles.y, Vector3.up) * points_pos[j];
                points_pos[j] = new Vector3(points_pos[j].x + p_0_x, points_pos[j].y, points_pos[j].z + p_0_z);
                j++;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < points_pos.Length; i++)
        {
            Vector3 pos = points_pos[i];
            if ((pos.x != 0)&&(pos.z != 0))
            {
                Gizmos.DrawSphere(pos, gizmoSize);
            }
            //Gizmos.DrawSphere(pos, gizmoSize);
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
            if ((pos.x != 0) && (pos.z != 0))
            {
                GL.Vertex3(pos.x, pos.y, pos.z);
                GL.Vertex3(pos.x, pos.y + gizmoSize, pos.z);
            }
        }
        GL.End();
        GL.PopMatrix();
    }
}
