using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public float cameraPanSpeed;
    public float cameraZoomSpeed;
    private Vector3 lastMousePos;
    [SerializeField] private float cameraBoundaryX;
    [SerializeField] private float cameraBoundaryY;
    [SerializeField]

    void Start()
    {
        if(mainCamera == null)
        mainCamera = Camera.main;
    }

    void Update()
    {
        CameraMovement();
        ScrollZoom();   
    }

    private void CameraMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 left = -Vector3.forward;
            transform.localPosition += left * cameraPanSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 right = Vector3.forward;
            transform.localPosition += right * cameraPanSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 up = Vector3.left;
            transform.localPosition += up * cameraPanSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 down = Vector3.right;
            transform.localPosition += down * cameraPanSpeed * Time.deltaTime;
        }

        else
        {
           lastMousePos = Vector3.zero;
        }      

    }

    private void ScrollZoom()
    {

        //Using the New Input system to get the value of the scroll wheel
        float scroll = Mouse.current.scroll.ReadValue().y;
        //Debug.Log("Scroll value: " + scroll);
        if (scroll > 0)
        {
            transform.position = transform.position + transform.forward * cameraZoomSpeed * Time.deltaTime;
        }
        else if (scroll < 0)
        {
            transform.position = transform.position - transform.forward * cameraZoomSpeed * Time.deltaTime;
        }
    }

    //Assigning a boundary around the camera with changeable values
    public void CameraBoundaries()
    {
        Vector3 horizontal = new Vector3(cameraBoundaryX, transform.position.y, transform.position.z);
        Vector3 vertical = new Vector3(transform.position.x, transform.position.y, cameraBoundaryY);
        if (transform.position.x >= horizontal.x)
        {
            transform.position = horizontal;
        }
        if (transform.position.x <= -horizontal.x)
        {
            transform.position = -horizontal;
        }
        if (transform.position.z >= vertical.z)
        {
            transform.position = vertical;
        }
        if (transform.position.z <= -vertical.z)
        {
            transform.position = -vertical;
        }
    }
}
