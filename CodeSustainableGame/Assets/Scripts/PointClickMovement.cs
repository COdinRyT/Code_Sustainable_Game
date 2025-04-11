using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

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

    private Vector3 madeUpVector3;

    private void Awake()
    {
        camera = Camera.main;
        rb = GetComponent<Rigidbody>();
        gameManager = FindAnyObjectByType<GameManager>();

        if (this.enabled == true)
        {
            gameManager.ConfirmVolunteer(gameObject);
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
                cube.transform.position = cube.transform.position + new Vector3(0, 0.1f, 0) * speedFactor;
                if (cube.transform.position.y > 5)
                {
                    flashup = false;
                    flashdown = true;
                }
            }
            if (flashdown)
            {
                cube.transform.position = cube.transform.position - new Vector3(0, 0.1f, 0) * speedFactor;
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
    }

    // When this function is called, make the player the selected game object
    public void SelectPlayer(GameObject player)
    {
        selectedPlayer = player;
    }

    public void SelectTile(GameObject tile, GameObject marker)
    {
        selectedTile = tile;
        if (currentMarker != null)
            Destroy(currentMarker);

        currentMarker = Instantiate(marker, selectedTile.transform.position, Quaternion.identity);
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
        flashCharacter = true;
        if (selectedPlayer == null)
        {
            flashCharacter = false;
            yield break;
        }

        // Get the NavMeshAgent from the selected player
        NavMeshAgent playerAgent = selectedPlayer.GetComponent<NavMeshAgent>();
        if (playerAgent == null)
        {
            flashCharacter = false;
            yield break;
        }

        // Enable rotation updates for the agent
        playerAgent.updateRotation = true;

        Vector3 cameraPosition = new Vector3(gameObject.transform.position.x + 6, gameObject.transform.position.y + 4, gameObject.transform.position.z);
        camera.transform.position = cameraPosition;

        // Wait for a click or check if we need to skip the move
        yield return StartCoroutine(WaitForClick());

        madeUpVector3 = new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z);
        for (int i = 0; i < allChildren.Length; i++)
        {
            if (allChildren[i].transform.position.x == madeUpVector3.x && allChildren[i].transform.position.z == madeUpVector3.z)
            {
                yield break;  // Exit the coroutine early
            }
        }

        // If skipMove is true, immediately skip the movement
        if (skipMove)
        {
            flashCharacter = false;
            skipMove = false;  // Reset skip flag
            yield break;  // Exit the coroutine early
        }

        // Get the click position (convert mouse position to world position)
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                // Set the target position to where the player clicked
                targetPosition = hit.point;

                // Round the target position to the nearest whole unit for tile-based movement
                targetPosition.x = Mathf.Round(targetPosition.x);
                targetPosition.z = Mathf.Round(targetPosition.z);
                targetPosition.y = hit.point.y;  // Keep the Y as the original height

                // Move the player to the snapped position
                playerAgent.SetDestination(targetPosition);

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
            }
        }
    }

    // Function to check if the player is on garbage
    private bool IsOnGarbage(Vector3 playerPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(playerPosition, 0.5f);  // Small radius around player to check for garbage
        foreach (Collider col in colliders)
        {
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
        while (true)
        {
            // If skipMove is triggered, exit immediately
            if (skipMove)
            {
                yield break;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.layer != 5)
                    {
                        yield break;
                    }
                }
            }

            yield return null;
        }
    }
}
