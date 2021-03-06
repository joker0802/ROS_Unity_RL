using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    // game object for the player
    public GameObject player;

    // variables for the mouse current X & Y coordinates
    private float mouseCurrentX, mouseCurrentY;

    /* ******************************************************************************** */

    // it is called once the object is active
    void Start()
    {
        // TODO: update them for 3rd person camera
        mouseCurrentX = 0;
        mouseCurrentY = 0;
    }

    // it is called every frame update
    void Update()
    {
        // TODO: update them for 3rd person camera
        //mouseCurrentX += Input.GetAxis("Mouse X");
        //mouseCurrentY += Input.GetAxis("Mouse Y");
    }

    // it is called every frame after all updates
    void LateUpdate()
    {
        // move the camera to follow the position of the player, taking the offset into consideration
        Vector3 direction = new Vector3(0, 20, -20);
        Quaternion roation = Quaternion.Euler(mouseCurrentX, mouseCurrentY, 0);
        transform.position = player.transform.position + roation * direction;
        transform.LookAt(player.transform.position);
        //transform.position = player.transform.position + offset;
    }
}
