using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Camera camera;

    UpdateUI updateUI;

    public Queue<GameObject> characters = new Queue<GameObject>(); // Character queue
    public GameObject prefab;

    public float stepDelay = 0.2f;  // Delay between tile movements 
    public int currentTurn;
    public int startTurn;
    public int maxTurn;
    public int currentPeople;
    public int maxPeople;
    public int currentMoney;
    public bool endTurn = false;

    public GameObject TerrainGroup;
    public GameObject Garbage;
    public List<GameObject> tag_targets = new List<GameObject>();

    public Transform parentTransform;

    private float chanceOfGarbage = 9;
    private float randomNumber;
    private Vector3 spot;

    private bool hasTaskStarted = false;  // Add a flag to track if the task has started

    PointClickMovement pointClickMovement;

    private int tileLimit;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        currentTurn = startTurn;
        SpawnGarbage();
        pointClickMovement = FindAnyObjectByType<PointClickMovement>();
        updateUI = FindAnyObjectByType<UpdateUI>();
        updateUI.UpdateQueueUI(new List<GameObject>(characters));

        if (updateUI == null)
        {
            Debug.Log("UI manager is not assigned to game manager!");
        }

        tileLimit = characters.Count;
        FirstPlayer();
    }

    public void FirstPlayer()
    {
        Debug.Log("First player function");
        DoTask();
    }

    // Update is called once per frame
    void Update()
    {
        tileLimit = characters.Count;

        if (endTurn && currentTurn < maxTurn)
        {
            Debug.Log("Up");
            endTurn = false;
            currentTurn++;
            DoTask();
        }
    }

    // Add character object into queue when function is called
    public void ConfirmVolunteer(GameObject character)
    {
        if (!characters.Contains(character))
        {
            characters.Enqueue(character);
            Debug.Log($"Added character: {character.name}, Total characters in queue: {characters.Count}");
        }
    }

    public void DoTask()
    {
        // Ensure we're only running the task once
        if (hasTaskStarted) return;
        hasTaskStarted = true;

        // Start the process to move characters one by one
        StartCoroutine(MoveCharacterSequence());
    }

    // Coroutine to move characters one at a time, waiting for click before each character moves
    private IEnumerator MoveCharacterSequence()
    {
        // Save a temporary list of all characters in the queue
        List<GameObject> charactersInCurrentTurn = new List<GameObject>(characters);

        while (characters.Count > 0)
        {
            GameObject currentCharacter = characters.Dequeue();

            // Debug logs to check queue sizes
            Debug.Log($"Dequeued Character: {currentCharacter.name}");
            Debug.Log($"Remaining Characters in Queue: {characters.Count}");

            updateUI.UpdateQueueUI(new List<GameObject>(characters));

            // Get the PointClickMovement component from the current character
            PointClickMovement characterMovement = currentCharacter.GetComponent<PointClickMovement>();

            if (characterMovement != null)
            {
                // Select the current character in PointClickMovement
                characterMovement.SelectPlayer(currentCharacter);
            }

            // Wait for the player to click before moving the character
            yield return StartCoroutine(WaitForClick());

            // Move the current player to the clicked position
            yield return StartCoroutine(characterMovement.MovePlayer());

            // Optional: Wait for a short delay before the next character moves
            yield return new WaitForSeconds(stepDelay);
        }

        // After all characters have moved, re-add them to the queue
        foreach (var character in charactersInCurrentTurn)
        {
            characters.Enqueue(character);  // Re-add characters to the queue
        }

        // End the turn after all characters have moved
        hasTaskStarted = false;  // Reset task flag
        Debug.Log("Turn ended");
    }

    // Wait for a mouse click before moving the current character
    private IEnumerator WaitForClick()
    {
        bool clicked = false;

        // Keep checking for a click until it happens
        while (!clicked)
        {
            if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
            {
                clicked = true;
            }
            yield return null; // Wait until the next frame
        }
    }

    private void SpawnGarbage()
    {
        foreach (Transform child in TerrainGroup.transform)
        {
            GameObject obj = child.gameObject;

            if (obj.layer == 7)
            {
                randomNumber = Random.Range(0, 10);
                if (randomNumber >= chanceOfGarbage)
                {
                    spot = new Vector3(obj.transform.position.x, .51f, obj.transform.position.z);
                    Instantiate(Garbage, spot, Quaternion.identity, parentTransform);
                }
            }
        }
    }

    public void WebsiteLink() // This is to link the Pollution Probe website 
    {
        Application.OpenURL("https://www.pollutionprobe.org"); // When a player selects a button, this function will be called
    }
}
