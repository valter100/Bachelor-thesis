using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    //[SerializeField] Vector3 acceleration;
    //[SerializeField] Vector3 deacceleration;

    //[SerializeField] Vector3 minAcceleration;
    //[SerializeField] Vector3 maxAcceleration;

    //[SerializeField] Vector3 velocity;
    //[SerializeField] float movement
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //acceleration += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * movementSpeed * Time.deltaTime;

        //acceleration += deacceleration * Time.deltaTime;

        //acceleration = Vector3.ClampMagnitude(acceleration, maxAcceleration.magnitude);

        //velocity += acceleration;
        //velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * movementSpeed * Time.deltaTime;
        //transform.position += transform.forward * velocity.magnitude;

        if(Input.GetMouseButton(1))
        {
            float xRotation = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
            float yRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad;

            transform.Rotate(Vector3.up, xRotation);
            transform.Rotate(Vector3.right, yRotation);
        }
    }
}
