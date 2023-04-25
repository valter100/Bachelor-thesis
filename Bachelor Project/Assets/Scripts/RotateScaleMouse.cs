using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScaleMouse : MonoBehaviour
{
    [SerializeField] Transform xTransform;
    [SerializeField] Transform yTransform;

    [SerializeField] float rotateSpeed;
    [SerializeField] float scaleSpeed;

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
            ScaleWithMouseWheel();

        if (Input.GetMouseButton(0))
            RotateWithMouse();
    }

    public void RotateWithMouse()
    {
        xTransform.Rotate(Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime, 0, 0, Space.World);
        yTransform.Rotate(0, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime * -1, 0, Space.World);
    }

    public void RotateWithWASD()
    {
        if (Input.GetKey(KeyCode.W))
        {
            xTransform.Rotate(rotateSpeed * Time.deltaTime, 0, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.A))
        {
            yTransform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            xTransform.Rotate(rotateSpeed * Time.deltaTime * -1, 0, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            yTransform.Rotate(0, rotateSpeed * Time.deltaTime * -1, 0, Space.Self);
        }
    }

    public void ScaleWithMouseWheel()
    {
        float value = Input.mouseScrollDelta.y;
        transform.localScale += new Vector3(value, value, value) * Time.deltaTime;
    }
}
