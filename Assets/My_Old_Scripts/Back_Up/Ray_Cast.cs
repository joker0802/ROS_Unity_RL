using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ray_Cast : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        float theDistance;

        transform.Rotate(new Vector3(0,1800,0) * Time.deltaTime); // 300 rpm

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.red);

        if (Physics.Raycast(transform.position, (forward), out hit))
        {
            theDistance = hit.distance;

            if (theDistance >= 3.5f) //measurement limits
            {
                theDistance = 3.5f;
            }
            else if (theDistance <= 0.12f)
            {
                theDistance = 0.12f;
            }

            print(theDistance + " " + hit.collider.gameObject.name);
        }
	}
}
