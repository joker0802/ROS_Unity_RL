
using UnityEngine;
using System.Collections;

public class WayRoad : MonoBehaviour
{

    //AI_Traffic_Pack//
    public Color colorRoad = new Color(0, 1, 0, 0.5f);// select color for the gizmo

    void OnDrawGizmos() //draw a gizmo (for showing the position of the waypoint)
    {
        Component[] wayroads = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform wayroad in wayroads)
        {
            Gizmos.color = colorRoad;
            Gizmos.DrawWireCube(wayroad.position, new Vector3(5, 5, 5));
        }

    }
}