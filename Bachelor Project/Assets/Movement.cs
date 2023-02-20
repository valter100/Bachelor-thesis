using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float currentSpeed;
    [SerializeField] float maxMovementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float acceleration;
    //[SerializeField] Vector3 deacceleration;

    //[SerializeField] Vector3 minAcceleration;
    //[SerializeField] Vector3 maxAcceleration;

    //[SerializeField] Vector3 velocity;
    //[SerializeField] float movement;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float xRotation = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
            float yRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad * -1;

            transform.Rotate(Vector3.up, xRotation);
            transform.Rotate(Vector3.right, yRotation);

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            currentSpeed += acceleration * Time.deltaTime;
        else
            currentSpeed -= acceleration * Time.deltaTime * 2;

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxMovementSpeed);

        transform.position += transform.forward * Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;
        transform.position += transform.right * Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
    }
}
