using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

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

    private bool isPlayerSelected;
    private bool isTileSelected;

    private Vector3 targetPosition;

    private void Awake()
    {
        camera = Camera.main;
        rb = GetComponent<Rigidbody>();
        gameManager = FindAnyObjectByType<GameManager>();
        isPlayerSelected = false;
        isTileSelected = false;

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
        // Handle player selection here if needed (already done by GameManager)
    }

    // When this function is called, make the player the selected game object
    public void SelectPlayer(GameObject player)
    {
        selectedPlayer = player;
        isPlayerSelected = true;
        Debug.Log("Player has been Selected");
    }

    public void SelectTile(GameObject tile, GameObject marker)
    {
        selectedTile = tile;
        isTileSelected = true;

        Instantiate(marker, selectedTile.transform.position, Quaternion.identity);
        Debug.Log("Tile selected");
    }

    // Move the player when this function is called and wait for the player to click
    public IEnumerator MovePlayer()
    {
        if (selectedPlayer == null)
        {
            Debug.LogError("No player selected!");
            yield break;
        }

        // Get the NavMeshAgent from the selected player
        NavMeshAgent playerAgent = selectedPlayer.GetComponent<NavMeshAgent>();
        if (playerAgent == null)
        {
            Debug.LogError("Selected player does not have a NavMeshAgent!");
            yield break;
        }

        // Wait for a click
        yield return StartCoroutine(WaitForClick());

        // Get the click position (convert mouse position to world position)
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = hit.point;  // Set the target position to where the player clicked

            // Move the player to the clicked position
            agent.SetDestination(targetPosition);
            Debug.Log($"Moving to: {targetPosition}");

            // Create marker on tile
            Instantiate(Marker, targetPosition, Quaternion.identity);

            // Wait for the agent to reach the target
            while (agent.pathPending || agent.remainingDistance > 0.1f)
            {
                yield return null;  // Continue waiting until the movement is complete
            }

            Debug.Log("Movement complete!");
        }
    }

    // Wait for a click before proceeding
    private IEnumerator WaitForClick()
    {
        bool clicked = false;

        while (!clicked)
        {
            if (Input.GetMouseButtonDown(0))  // Left mouse button clicked
            {
                clicked = true;
            }

            yield return null;  // Wait until the next frame
        }
    }
}
