using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
//using UnityEngine.EventSystems;  // Include this to check for UI interaction

public class PointClickMovement : MonoBehaviour
{
    private Camera camera;
    public NavMeshAgent agent;
    GameManager gameManager;

    public GameObject Marker;

    public float playerSpeed = 5f;  // Adjust speed for turn-based feel
    public float stepDelay = 0.2f;  // Delay between tile movements

    private Rigidbody rb;
    public GameObject selectedPlayer = null;  // The character that the player selects
    public GameObject selectedTile = null;   // The tile that the player selects

    private Vector3 targetPosition;


    private Vector3 additionPos = new Vector3(0,0.1f,0);
    // Flashing variables
    public bool flashCharacter = false;
    private bool flashup = true;
    private bool flashdown;
    public GameObject cube;

    public float moveSpeedCube = 50f;
    private float speedFactor;

    // Skip flag for skipping movement
    public bool skipMove = false;


    private void Awake()
    {
        camera = Camera.main;
        rb = GetComponent<Rigidbody>();
        gameManager = FindAnyObjectByType<GameManager>();

        if (this.enabled == true)
        {
            Debug.Log($"Agent {gameObject.name} has been added to queue");
            gameManager.ConfirmVolunteer(gameObject);
        }
        else
        {
            Debug.Log("Agent is not in the queue");
        }
    }

    private void Update()
    {
        speedFactor = moveSpeedCube * Time.deltaTime;
        if (flashCharacter)
        {
            cube.SetActive(true);
            if (flashup)
            {
                cube.transform.position = cube.transform.position + additionPos * speedFactor;
                if (cube.transform.position.y > 5)
                {
                    flashup = false;
                    flashdown = true;
                }
            }
            if (flashdown)
            {
                cube.transform.position = cube.transform.position - additionPos * speedFactor;
                if (cube.transform.position.y < 3)
                {
                    flashup = true;
                    flashdown = false;
                }
            }
        }
        else
        {
            cube.SetActive(false);
        }
        // Handle player selection here if needed (already done by GameManager)
    }

    // When this function is called, make the player the selected game object
    public void SelectPlayer(GameObject player)
    {
        selectedPlayer = player;

        Debug.Log("Player has been Selected");
    }

    public void SelectTile(GameObject tile, GameObject marker)
    {
        selectedTile = tile;
        Instantiate(marker, selectedTile.transform.position, Quaternion.identity);
        Debug.Log("Tile selected");
    }

    // Move the player when this function is called and wait for the player to click
    public IEnumerator MovePlayer()
    {
        flashCharacter = true;
        if (selectedPlayer == null)
        {
            flashCharacter = false;
            Debug.LogError("No player selected!");
            yield break;
        }

        // Get the NavMeshAgent from the selected player
        NavMeshAgent playerAgent = selectedPlayer.GetComponent<NavMeshAgent>();
        if (playerAgent == null)
        {
            flashCharacter = false;
            Debug.LogError("Selected player does not have a NavMeshAgent!");
            yield break;
        }

        // Wait for a click or check if we need to skip the move
        yield return StartCoroutine(WaitForClick());

        // If skipMove is true, immediately skip the movement
        if (skipMove)
        {
            Debug.Log("Move skipped due to skip flag.");
            flashCharacter = false;
            skipMove = false;  // Reset skip flag
            yield break;  // Exit the coroutine early
        }

        // Only proceed with raycast if we are not over UI (like a button)
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Skip raycasting if mouse is over UI
            flashCharacter = false;
            Debug.Log("Pointer is over UI, skipping raycast.");
            yield break;
        }

        // Get the click position (convert mouse position to world position)
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = hit.point;  // Set the target position to where the player clicked

            // Round the target position to the nearest whole unit for tile-based movement
            targetPosition.x = Mathf.Round(targetPosition.x);  // Round X to nearest 1 unit
            targetPosition.z = Mathf.Round(targetPosition.z);  // Round Z to nearest 1 unit
            targetPosition.y = hit.point.y;  // Keep the Y as the original height

            // Move the player to the snapped position
            playerAgent.SetDestination(targetPosition);
            Debug.Log($"Moving to snapped position: {targetPosition}");

            // Create marker on tile
            Instantiate(Marker, targetPosition, Quaternion.identity);

            // Wait for the agent to reach the target
            while (playerAgent.pathPending || playerAgent.remainingDistance > 0.1f)
            {
                yield return null;  // Continue waiting until the movement is complete
            }
            flashCharacter = false;
            Debug.Log("Movement complete!");
        }
    }
    // Wait for a click before proceeding
    private IEnumerator WaitForClick()
    {
        bool clicked = false;

        // While we haven't clicked and haven't skipped, keep waiting
        while (!clicked && !skipMove)
        {
            if (Input.GetMouseButtonDown(0))  // Left mouse button clicked
            {
                clicked = true;
            }
            yield return null;  // Wait until the next frame
        }
    }
}
