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
    private GameObject currentMarker;
    public float playerSpeed = 5f;  // Adjust speed for turn-based feel
    public float stepDelay = 0.2f;  // Delay between tile movements

    private Rigidbody rb;
    public GameObject selectedPlayer = null;  // The character that the player selects
    public GameObject selectedTile = null;   // The tile that the player selects

    private Vector3 targetPosition;

    private Vector3 additionPos = new Vector3(0, 0.1f, 0);
    // Flashing variables
    public bool flashCharacter = false;
    private bool flashup = true;
    private bool flashdown;
    public GameObject cube;

    public float moveSpeedCube = 50f;
    private float speedFactor;

    // Skip flag for skipping movement
    public bool skipMove = false;
    public LayerMask raycastLayerMask;

    public GameObject[] allChildren;
    public GameObject GarbageStorage;

    public float x;
    public float y;
    public float z;

    private Vector3 madeUpVector3;

    private void Awake()
    {
        camera = Camera.main;
        rb = GetComponent<Rigidbody>();
        gameManager = FindAnyObjectByType<GameManager>();

        if (this.enabled == true)
        {
            //Debug.Log($"Agent {gameObject.name} has been added to queue");
            gameManager.ConfirmVolunteer(gameObject);
        }
        else
        {
            //Debug.Log("Agent is not in the queue");
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

        //Debug.Log("Player has been Selected");
    }

    public void SelectTile(GameObject tile, GameObject marker)
    {
        selectedTile = tile;
        if (currentMarker != null)
            Destroy(currentMarker);

        currentMarker = Instantiate(marker, selectedTile.transform.position, Quaternion.identity);
        //Debug.Log("Tile selected");
    }
    private void GetChildren()
    {
        allChildren = new GameObject[GarbageStorage.transform.childCount];
        for (int i = 0; i < allChildren.Length; i++)
        {
            allChildren[i] = GarbageStorage.transform.GetChild(i).gameObject;
        }
    }
    // Move the player when this function is called and wait for the player to click
    public IEnumerator MovePlayer()
    {

        //skipMove = GameManager.Instance.endTurn;
        flashCharacter = true;
        if (selectedPlayer == null)
        {
            flashCharacter = false;
            //Debug.LogError("No player selected!");
            yield break;
        }

        // Get the NavMeshAgent from the selected playerw
        NavMeshAgent playerAgent = selectedPlayer.GetComponent<NavMeshAgent>();
        if (selectedPlayer == null)
        {
            flashCharacter = false;
            Debug.LogError("No player selected!");
            yield break;
        }

        Vector3 cameraPosition = new Vector3(gameObject.transform.position.x + 6, gameObject.transform.position.y + 4, gameObject.transform.position.z);
        camera.transform.position = cameraPosition;
        //Debug.Log("Waiting for click");
        // Wait for a click or check if we need to skip the move
        yield return StartCoroutine(WaitForClick());

        madeUpVector3 = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
        for (int i = 0; i < allChildren.Length; i++)
        {
            if (allChildren[i].transform.position.x == madeUpVector3.x && allChildren[i].transform.position.z == madeUpVector3.z)
            {
                //Debug.Log("On garbage tile");
                yield break;  // Exit the coroutine early
            }
        }

        // If skipMove is true, immediately skip the movement
        if (skipMove)
        {
            Debug.Log($"{gameObject.name} => Skipping move due to skipMove = TRUE in MovePlayer()");
            flashCharacter = false;
            skipMove = false;
            yield break;
        }

        // // Only proceed with raycast if we are not over UI (like a button)
        // if (EventSystem.current.IsPointerOverGameObject())
        // {

        //     // Skip raycasting if mouse is over UI
        //     flashCharacter = false;
        //     Debug.Log("Pointer is over UI, skipping raycast.");
        //     yield break;
        // }

        // Get the click position (convert mouse position to world position)
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                //Debug.Log("Ray hit" + hit.transform.gameObject.layer);
                targetPosition = hit.point;  // Set the target position to where the player clicked

                // Round the target position to the nearest whole unit for tile-based movement
                targetPosition.x = Mathf.Round(targetPosition.x);  // Round X to nearest 1 unit
                targetPosition.z = Mathf.Round(targetPosition.z);  // Round Z to nearest 1 unit
                targetPosition.y = hit.point.y;  // Keep the Y as the original height

                // Move the player to the snapped position
                playerAgent.SetDestination(targetPosition);
                //Debug.Log($"Moving to snapped position: {targetPosition}");

                // Create marker on tile
                //Instantiate(Marker, targetPosition, Quaternion.identity);

                // Wait for the agent to reach the target
                while (playerAgent.pathPending || playerAgent.remainingDistance > 0.1f)
                {
                    yield return null;  // Continue waiting until the movement is complete
                }
                // Check if the player is standing on garbage after moving
                if (IsOnGarbage(selectedPlayer.transform.position))
                {
                    skipMove = true;  // Automatically skip the turn if the player is on garbage
                }

                flashCharacter = false;
                //Debug.Log("Movement complete!");
            }
        }
    }
    // Function to check if the player is on garbage
    private bool IsOnGarbage(Vector3 playerPosition)
    {
        // Logic to check if the player is standing on a garbage tile
        // Here, we'll assume your garbage tiles are tagged as "Garbage"
        Collider[] colliders = Physics.OverlapSphere(playerPosition, 0.5f);  // Small radius around player to check for garbage
        foreach (Collider col in colliders)
        {
            //Debug.Log(col);
            if (col.name == "SmallGarbage" || col.name == "MediumGarbage")  // Make sure the garbage objects have this tag
            {
                return true;
            }
        }
        return false;
    }
    // Wait for a click before proceeding
    private IEnumerator WaitForClick()
    {
        madeUpVector3 = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);

        for (int i = 0; i < allChildren.Length; i++)
        {
            if (allChildren[i].transform.position.x == madeUpVector3.x && allChildren[i].transform.position.z == madeUpVector3.z)
            {
                yield break;  // Exit the coroutine early
            }
        }

        while (true)
        {
            // If skipMove is triggered, exit immediately
            if (skipMove)
            {
                //Debug.Log("Skip move detected during WaitForClick()");
                yield break;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // 🟢 Draw the ray in the Scene view for debugging
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 400f); // 2 seconds

                // 🟡 Optional log
                Debug.Log("Mouse click raycast fired");

                // Make sure you're not clicking on UI
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                        Debug.Log("Hit layer: " + hit.collider.gameObject.layer);

                        // If it's NOT the layer you're expecting, stop the coroutine
                        if (hit.collider.gameObject.layer != 5)
                        {
                            Debug.Log("Hit object is not on expected layer (5), breaking coroutine.");
                            yield break;
                        }
                    }
                    else
                    {
                        Debug.Log("Raycast did not hit anything.");
                    }
                }
                else
                {
                    Debug.Log("Pointer is over UI. Ignoring click.");
                }
            }
        }
    }
}
