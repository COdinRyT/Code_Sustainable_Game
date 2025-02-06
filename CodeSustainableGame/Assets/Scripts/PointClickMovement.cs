using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    //[SerializeField] private InputAction mouseClick;   // Click to select a tile
    //[SerializeField] private InputAction confirmMove;  // Press to confirm movement

    private Camera camera;
    public NavMeshAgent agent;
    public Button confirmButton; //UI button to confirm movement

    public float playerSpeed = 5f;  // Adjust speed for turn-based feel
    public float stepDelay = 0.2f;  // Delay between tile movements
    private Vector3 targetPosition;

    private Rigidbody rb;
    [SerializeField] private GameObject[] gridCoordinates; // Array of valid tiles
    private GameObject selectedTile = null;  // The tile the player selects

    private Vector3 normalizePoint;
    private Vector3 midNormalizePoints;
    private float xdecimalPoint;
    private float zdecimalPoint;

    public GameObject prefab;
    private void Awake()
    {
        camera = Camera.main;
        rb = GetComponent<Rigidbody>();


        // Find all tiles dynamically if not assigned
        if (gridCoordinates.Length == 0)
        {
            gridCoordinates = GameObject.FindGameObjectsWithTag("GridTile");
        }

        //if (confirmButton != null)
        //{
        //    confirmButton.onClick.AddListener(MoveToSelectTile);
        //}
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                normalizePoint = hit.point;

                //Debug.Log(normalizePoint.x);
                //Debug.Log(Math.Truncate(normalizePoint.x));
                //xdecimalPoint = Mathf.Floor(normalizePoint.x) - normalizePoint.x;
                //Debug.Log(xdecimalPoint);
                // Whole number plus 0.5 
                
                xdecimalPoint = Mathf.Round(hit.point.x);
                zdecimalPoint = Mathf.Round(hit.point.z);
                midNormalizePoints = new Vector3(xdecimalPoint, hit.point.y, zdecimalPoint);
                Debug.Log(normalizePoint);
                Debug.Log(midNormalizePoints);
                agent.SetDestination(midNormalizePoints);
                Instantiate(prefab, midNormalizePoints, Quaternion.identity);
            }
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        agent.SetDestination(hit.point);
        //    }
        //}
    }

    public void MoveToSelectTile()
    {
        if(selectedTile != null)
        {
            agent.SetDestination(targetPosition);
            Debug.Log("Moving to tile");
            selectedTile = null; //Reset tile to null 
        }
    }
    /*
    private void OnEnable()
    {
        mouseClick.Enable();
        mouseClick.performed += SelectTile;

        confirmMove.Enable();
        confirmMove.performed += ConfirmMovement;
    }

    //This function is called when the script is enabled
    private void OnDisable()
    {
        mouseClick.performed -= SelectTile;
        mouseClick.Disable();

        confirmMove.performed -= ConfirmMovement;
        confirmMove.Disable();
    }
    
    /// <summary>
    /// Selects a grid tile when clicked.
    /// </summary>
    private void SelectTile(InputAction.CallbackContext context)
    {
        //This is to project a ray from the camera position onto wherever the mouse 
        //is currently positioned
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            foreach (GameObject gridTile in gridCoordinates)
            {
                if (hit.collider.gameObject == gridTile)
                {
                    selectedTile = gridTile;
                    Debug.Log("Selected Tile: " + selectedTile.name);
                    return;
                }
            }
        }
    }
    */
    /*
    /// <summary>
    /// Confirms movement and moves player toward the selected tile step-by-step.
    /// </summary>
    private void ConfirmMovement(InputAction.CallbackContext context)
    {
        if (selectedTile != null)
        {
            if (moveCoroutine != null) StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveStepByStep(selectedTile.transform.position));
        }
    }

    /// <summary>
    /// Moves the player step by step toward the target position.
    /// </summary>
    private IEnumerator MoveStepByStep(Vector3 target)
    {
        target.y = transform.position.y; // Keep the player at the same height

        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            Vector3 nextStep = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);
            transform.position = nextStep;
            yield return new WaitForSeconds(stepDelay); // Adds a delay between steps for a turn-based feel
        }

        transform.position = target; // Snap to the exact position
    }
    */
}
