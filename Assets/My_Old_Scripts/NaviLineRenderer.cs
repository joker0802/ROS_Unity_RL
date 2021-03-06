using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class NaviLineRenderer : MonoBehaviour
{
    private LineRenderer myNaviLineRenderer;
    public GameObject wayPoints;
    private Transform[] naviPoints;
    void Start()
    {
        myNaviLineRenderer = GetComponent<LineRenderer>();
        //myNaviLineRenderer.material.color = Color.yellow;
        myNaviLineRenderer.startWidth = 10.0f;
        myNaviLineRenderer.endWidth = myNaviLineRenderer.startWidth;
    
        myNaviLineRenderer.positionCount = 0;
    }

    void Update()
    {
        naviPoints = wayPoints.GetComponentsInChildren<Transform>();
        float dis = Vector3.Distance(new Vector3(transform.position.x, 1.0f, transform.position.z), new Vector3(naviPoints[1].position.x, 1.0f, naviPoints[1].position.z));
        //Debug.Log("x1"+ transform.position.x + "x2" + naviPoints[1].position.x + "dis" + dis);
        if ( dis < 5f)
        {
            Destroy(wayPoints.gameObject.transform.GetChild(0).gameObject);
        }
        if (naviPoints.Length > 0)
        {
            DrawPathLine();
        }

    }

    void DrawPathLine()
    {
        myNaviLineRenderer.positionCount = naviPoints.Length;
        myNaviLineRenderer.SetPosition(0,new Vector3(transform.position.x, 1.0f, transform.position.z));
        if (myNaviLineRenderer.positionCount < 2)
        {
            return;
        }
        for (int i =1; i < myNaviLineRenderer.positionCount; i++)
        {
            Vector3 pointPosition = new Vector3(naviPoints[i].position.x, 1.0f, naviPoints[i].position.z);
            myNaviLineRenderer.SetPosition(i,pointPosition);
        }
    }
}
