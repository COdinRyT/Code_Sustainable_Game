using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private InputAction mouseClick; //To keep track of the mouse click inputs with a variable

    private Camera camera;
    private Coroutine coroutine;

    public float playerSpeed = 10f; //How fast the characters will move on screen

    private Vector3 targetPosition; //Keeping track of wherever the player clicks on screen in a Vactor 3 


    private void Awake()
    {
        camera = Camera.main;
    }

    //This function is called when the script is enabled
    private void OnEnable() 
    {
        mouseClick.Enable(); //Enables the player to use mouse inputs
        mouseClick.performed += Move; //Once a mouse input is performed, the Move script is called
    }

    //This function is called when the script is enabled
    private void OnDisable()
    {
        mouseClick.performed -= Move; //Cancels the movement action if mouse click is performed
        mouseClick.Disable(); //Disables the players' mouse inputs
    }

    //Main movement function for when a mouse click input id performed
    private void Move(InputAction.CallbackContext context)
    {
        //This is to project a ray from the camera position onto wherever the mouse 
        //is currently positioned
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        //Perform a raycast and check if it hits a collider
        if(Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider)
        {
            //If another movement coroutine is already running, stop it
            if(coroutine != null) StopCoroutine(coroutine);            

            //Start new coroutine to move player towards wherever the point clicked is
            coroutine = StartCoroutine(PlayerMoveTowards(hit.point));
            targetPosition = hit.point; //For Gizmos, update position on screen
        }
    }

    //Coroutine to move player towards a target point 
    private IEnumerator PlayerMoveTowards(Vector3 target)
    {
        //While player is over 0.1 unity units away from point 
        while(Vector2.Distance(transform.position, target) > 0.1f)
        {
            Vector3 destination = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);
            transform.position = destination;
            yield return null;
        }
    }

    private void OnDrawGizmos() //For debugging purposes 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetPosition, 1); //Draw a sphere indicator at position where the mouse is clicked on screen
    }


}
