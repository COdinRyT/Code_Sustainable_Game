using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    private Camera camera;
    public NavMeshAgent agent;

    public float playerSpeed = 5f;  // Adjust speed for turn-based feel
    public float stepDelay = 0.2f;  // Delay between tile movements
    

    private Rigidbody rb;
    [SerializeField] private GameObject[] gridCoordinates; // Array of valid tiles
    private GameObject selectedPlayer = null;  // The character that the player selects
    private GameObject selectedTile = null; // The tile that the player selects
    //Booleans to keep track on whether player or tile have been selected
    public bool isPlayerSelected;
    private bool isTileSelected;
    //This is to keep track of how many times an object is selected
    private int timesSelected;

    private Vector3 normalizePoint;
    private Vector3 midNormalizePoints;
    private float xdecimalPoint;
    private float zdecimalPoint;

    public GameObject prefab;
    private void Awake()
    {
        camera = Camera.main;
        rb = GetComponent<Rigidbody>();
        isPlayerSelected = false;
        isTileSelected = false;

        // Find all tiles dynamically if not assigned
        if (gridCoordinates.Length == 0)
        {
            gridCoordinates = GameObject.FindGameObjectsWithTag("GridTile");
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == 6)
                {
                    SelectPlayer(hit.collider.gameObject);
                    timesSelected++;
                    if(hit.collider.gameObject.layer == 6 && timesSelected ==2)
                    {
                        isPlayerSelected=false;
                        timesSelected = 0;
                        Debug.Log("Player unselected");
                    }
                }

                if(hit.collider.gameObject.layer == 7 && isPlayerSelected == true)
                {
                    SelectTile(hit.collider.gameObject);
                }
            }
        }
    }

    //When this function is called, make the player the selected game object 
    public void SelectPlayer(GameObject player)
    {
        selectedPlayer = player;
        isPlayerSelected = true;
        Debug.Log("Player has been Selected");
    }

    public void SelectTile(GameObject tile)
    {
        selectedTile = tile;
        isTileSelected = true;        
        Instantiate(prefab, selectedTile.transform.position, Quaternion.identity) ;
        Debug.Log("Tile selected");
    }

    //Move the player when this function is called
    public void MovePlayer(Vector3 targetPoint)
    {

        xdecimalPoint = Mathf.Round(targetPoint.x);
        zdecimalPoint = Mathf.Round(targetPoint.z);
        midNormalizePoints = new Vector3(xdecimalPoint, 1, zdecimalPoint);
        //Debug.Log(normalizePoint);
        //Debug.Log(midNormalizePoints);
        agent.SetDestination(midNormalizePoints);
        Instantiate(prefab, midNormalizePoints, Quaternion.identity);
        CheckIfOnGarbage.Instance.x = midNormalizePoints.x;
        CheckIfOnGarbage.Instance.y = midNormalizePoints.y;
        CheckIfOnGarbage.Instance.z = midNormalizePoints.z;
        CheckIfOnGarbage.Instance.CheckCollisionBetweenPlayerAndGarbage();
    }

    //public void SelectPlayer(GameObject player)
    //{
    //    selectedPlayer = player;
    //    isPlayerSelected = true;
    //    Debug.Log("Player has been selected");
    //}

    //public void MoveSelectedPlayer(Vector3 targetPoint)
    //{
    //    if(selectedPlayer != null)
    //    {
    //        normalizePoint = targetPoint;

    //        //Debug.Log(normalizePoint.x);
    //        //Debug.Log(Math.Truncate(normalizePoint.x));
    //        //xdecimalPoint = Mathf.Floor(normalizePoint.x) - normalizePoint.x;
    //        //Debug.Log(xdecimalPoint);
    //        // Whole number plus 0.5 

    //        xdecimalPoint = Mathf.Round(targetPoint.x);
    //        zdecimalPoint = Mathf.Round(targetPoint.z);
    //        midNormalizePoints = new Vector3(xdecimalPoint, targetPoint.y, zdecimalPoint);
    //        Debug.Log(normalizePoint);
    //        Debug.Log(midNormalizePoints);
    //        agent.SetDestination(midNormalizePoints);
    //        Instantiate(prefab, midNormalizePoints, Quaternion.identity);
    //    }
    //    else
    //    {
    //        Debug.Log("Player is not selected");
    //    }
       
    //}

    
    //private void OnEnable()
    //{
    //    mouseClick.Enable();
    //    mouseClick.performed += SelectTile;

    //    confirmMove.Enable();
    //    confirmMove.performed += ConfirmMovement;
    //}

    ////This function is called when the script is enabled
    //private void OnDisable()
    //{
    //    mouseClick.performed -= SelectTile;
    //    mouseClick.Disable();

    //    confirmMove.performed -= ConfirmMovement;
    //    confirmMove.Disable();
    //}
    
    
    //private void SelectTile(InputAction.CallbackContext context)
    //{
    //    //This is to project a ray from the camera position onto wherever the mouse 
    //    //is currently positioned
    //    Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
    //    if (Physics.Raycast(ray, out RaycastHit hit))
    //    {
    //        foreach (GameObject gridTile in gridCoordinates)
    //        {
    //            if (hit.collider.gameObject == gridTile)
    //            {
    //                selectedTile = gridTile;
    //                Debug.Log("Selected Tile: " + selectedTile.name);
    //                return;
    //            }
    //        }
    //    }
    //}
    //*/
    
    
    //private void ConfirmMovement(InputAction.CallbackContext context)
    //{
    //    if (selectedTile != null)
    //    {
    //        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
    //        moveCoroutine = StartCoroutine(MoveStepByStep(selectedTile.transform.position));
    //    }
    //}

    
    //private IEnumerator MoveStepByStep(Vector3 target)
    //{
    //    target.y = transform.position.y; // Keep the player at the same height

    //    while (Vector3.Distance(transform.position, target) > 0.1f)
    //    {
    //        Vector3 nextStep = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);
    //        transform.position = nextStep;
    //        yield return new WaitForSeconds(stepDelay); // Adds a delay between steps for a turn-based feel
    //    }

    //    transform.position = target; // Snap to the exact position
    //}
    
}
