using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Scanner : MonoBehaviour {

    private float[] hits = new float[360];
    // Update is called once per frame
    void Update () {

        int RaysToShoot = 360; //360 rays per frame
        float angle = 0; //Initial angle
        float theDistance;
        
        for (int i = 0; i < RaysToShoot; i++)
        {
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / RaysToShoot;

            //Vector3 dir = new Vector3((transform.position.x + x*3.5f), transform.position.y, (transform.position.z + z*3.5f));

            //Define the direction and range of each laser beam 
            Vector3 forward = transform.TransformDirection(new Vector3(z, 0, x)) * 10.5f;

            RaycastHit hit;
            //Debug.DrawLine(transform.position, dir, Color.red); 
            //Debug.DrawRay(transform.position, forward, Color.red); //Showing red rays
            if (Physics.Raycast(transform.position, (forward), out hit)) //if (Physics.Raycast(transform.position, dir, out hit))
            {
                theDistance = hit.distance;
                //if (theDistance >= 9.5f) //measurement limits
                //{
                   // theDistance = 9.5f;
               // }
                //else if (theDistance <= 0.12f)
                //{
                    //theDistance = 0.12f;
               // }
                hits[i] = theDistance;
            }
        }
        //Debug.Log("theDistance: " + hits);
    }
    public float[] GetHits() //getter
    {
        return hits;
    }
}
