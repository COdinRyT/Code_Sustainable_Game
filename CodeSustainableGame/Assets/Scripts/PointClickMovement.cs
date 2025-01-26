using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private InputAction mouseClick;

    private Camera camera;
    private Coroutine coroutine;

    public float playerSpeed = 10f;

    private Vector3 targetPosition;


    private void Awake()
    {
        camera = Camera.main;
    }
    private void OnEnable()
    {
        mouseClick.Enable();
        mouseClick.performed += Move;
    }

    private void OnDisable()
    {
        mouseClick.performed -= Move;
        mouseClick.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider)
        {
            if(coroutine != null) StopCoroutine(coroutine);            
            coroutine = StartCoroutine(PlayerMoveTowards(hit.point));
            targetPosition = hit.point;
        }
    }

    private IEnumerator PlayerMoveTowards(Vector3 target)
    {
        while(Vector2.Distance(transform.position, target) > 0.1f)
        {
            Vector3 destination = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);
            transform.position = destination;
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetPosition, 1);
    }


}
