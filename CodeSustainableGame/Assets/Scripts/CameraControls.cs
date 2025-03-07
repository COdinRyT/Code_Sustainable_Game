using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
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
    private float zoomStep;


    

    // Start is called before the first frame update
    void Start()
    {
        if(mainCamera == null)
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
        ScrollZoom();
        CameraBoundaries();      
    }

    private void CameraMovement()
    {
        if (Input.GetMouseButton(2))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            if (lastMousePos != Vector3.zero)
            {
                Vector3 mouseMovement = currentMousePosition - lastMousePos;

                Vector3 pan = new Vector3(mouseMovement.x * cameraPanSpeed * Time.deltaTime, 0f, mouseMovement.y * cameraPanSpeed * Time.deltaTime);

                transform.Translate(pan, Space.World);
            }
            //Resett the last mouse position when the middle mouse button is released
            lastMousePos = Input.mousePosition;
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
        Debug.Log("Scroll value: " + scroll);
        if (scroll > 0)
        {
            transform.position = transform.position + transform.forward * zoomStep * Time.deltaTime;
        }
        else if (scroll < 0)
        {
            transform.position = transform.position - transform.forward * zoomStep * Time.deltaTime;
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
