using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    // ball movement speed
    public float speed;

    // rigid body object component
    private new Rigidbody rigidbody;

    /* ******************************************************************************** */

    // it is called once the object is active
    void Start()
    {
        // find the rigid body object component
        rigidbody = GetComponent<Rigidbody>();
    }

    // it is called on a regular time basis before physics calculation
    void FixedUpdate()
    {
        // get horizontal movement input from the keyboard arrows
        float moveHorizontal = Input.GetAxis("Horizontal");
        // get vertical movement input from the keyboard arrows
        float moveVertical = Input.GetAxis("Vertical");
        // movement vecory with the horizontal and veritcal movement with none in the altitude
        Vector3 movementVector = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // move the body with the desired movement and adjusted speed
        rigidbody.AddForce(movementVector * speed);
    }
}