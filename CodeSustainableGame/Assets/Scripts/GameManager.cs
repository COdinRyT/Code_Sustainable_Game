using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System;
public class GameManager : MonoBehaviour
{

    public UpdateUI updateUI;

    public Queue<GameObject> characters = new Queue<GameObject>(); // Character queue
    public GameObject newVolunteers;
    public GameObject prefab;

    public float stepDelay = 0.2f;  // Delay between tile movements 
    public int currentTurn;
    public int startTurn;
    public int maxTurn;
    public int currentPeople;
    public int maxPeople;
    public int currentMoney;
    public int happiness;
    public int awarenessLevel;
    public int currentPlantedTrees = 0;

    public int garbageLevel = 1;
    public int currentGarbageAmount; // An example is garbage will start at 100. 

    public bool readyForGetInvolved = false;
    public bool endTurn = false;

    public Button skipButton;  // Drag the Skip Button here from the Unity Editor

    public GameObject TerrainGroup;
    public GameObject Garbage;
    public GameObject[] garbagePiles;
    public List<GameObject> tag_targets = new List<GameObject>();
    public Transform parentTransform;
    Camera camera;

    private float chanceOfGarbage = 9;
    private float randomNumber;
    private Vector3 spot;

    //private returnVal;
    private bool hasTaskStarted = false;  // Add a flag to track if the task has started

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
        StartGame();
        SpawnGarbage();
        updateUI = FindAnyObjectByType<UpdateUI>();
        updateUI.UpdateQueueUI(new List<GameObject>(characters));

        if (skipButton != null)
        {
            //skipButton.onClick.AddListener(OnSkipButtonClick);
        }

        if (updateUI == null)
        {
            Debug.Log("UI manager is not assigned to game manager!");
        }
        FirstPlayer();
    }
    // This method is called when the skip button is clicked
    public void OnEndTurnClick()
    {
        // Set the skip flag to true for all characters
        foreach (GameObject character in characters)
        {
            PointClickMovement movement = character.GetComponent<PointClickMovement>();
            if (movement != null)
            {
                movement.skipMove = false;
                updateUI.IncreaseTurnCount();
            }
        }
    }
    void OnSkipButtonClick()
    {
        // Set the skip flag to true for all characters
        foreach (GameObject character in characters)
        {
            PointClickMovement movement = character.GetComponent<PointClickMovement>();
            if (movement != null)
            {
                movement.skipMove = true;
                movement.flashCharacter = false;
            }
        }
    }

    public void FirstPlayer()
    {
        Debug.Log("First player function");
        DoTask();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("End turn: " + endTurn);
        //Debug.Log(" Current Turn: " + currentTurn);
        //Debug.Log("Max turn: " + maxTurn);
        if (happiness >= 100) // This is how you win the game
        {
            EndGame();
        }
        if (currentTurn >= 50)// This is how you lose the game
        {
            EndGame();
        }
        if(currentGarbageAmount <= 0)
        {
            EndGame();
        }

        if (endTurn && currentTurn < maxTurn)
        {
            GameManager.Instance.GetInvolvedIsTrue();
            updateUI.UpdateQueueUI(new List<GameObject>(characters));
            if (endTurn && currentTurn < maxTurn)
            {
                // Find all game objects with the tag "Player" (or any tag you've assigned to your characters)
                GameObject[] allCharacters = GameObject.FindGameObjectsWithTag("Player");

                // Loop through each character and call a function (e.g., CheckCollisionBetweenPlayerAndGarbage)
                foreach (GameObject character in allCharacters)
                {
                    Debug.Log($"Checking for garbage for character: {character.name}");

                    // Assuming each character has a script (like CheckIfOnGarbage) attached with a function you want to call
                    CheckIfOnGarbage playerScript = character.GetComponent<CheckIfOnGarbage>();

                    if (playerScript != null)
                    {
                        // Call the function to check for garbage (or any other function you want to execute)
                        //Debug.Log("Test");
                        playerScript.CheckCollisionBetweenPlayerAndGarbage();
                    }
                    else
                    {
                        Debug.LogWarning($"Player {character.name} does not have the CheckIfOnGarbage script attached.");
                    }
                }

                Debug.Log("Up");
                endTurn = false;
                currentTurn++;
                DoTask();
            }
        }
    }
    void EndGame()
    {
        SceneManager.LoadScene("EndGame");
    }
    public void GetInvolvedIsTrue()
    {
        Debug.Log("Update glow");
        GlowAndSparkle.Instance.transparency = 100;
    }
    void StartGame()
    {
        SetupVariables();
        SpawnGarbage();
    }
    void SetupVariables()
    {
        if (garbageLevel == 1)
        {
            currentGarbageAmount = 500;
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
            //yield return StartCoroutine(WaitForClick());

            // Move the current player to the clicked position
            yield return StartCoroutine(characterMovement.MovePlayer());
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

    private void SpawnGarbage()
    {
        //Debug.Log("Spawn garbo");
        GameObject GarbageClone;
        foreach (Transform child in TerrainGroup.transform)
        {
            GameObject obj = child.gameObject;
            //Debug.Log("obj: " + obj.name);
            if (obj.layer == 7)
            {
                // Debug.Log("Random num");
                randomNumber = UnityEngine.Random.Range(0, 10);
                if (randomNumber >= chanceOfGarbage)
                {
                    // Get the center of the tile (obj.transform.position should be the center of the tile)
                    Vector3 tileCenter = obj.transform.position;

                    // Set the garbage spawn position to be slightly above the tile (e.g., 1 unit above)
                    Vector3 spawnPosition = new Vector3(tileCenter.x, tileCenter.y + 1f, tileCenter.z);

                    // Instantiate the garbage at the calculated spawn position
                    GarbageClone = Instantiate(Garbage, spawnPosition, Quaternion.identity, parentTransform);
                    GarbageClone.name = Garbage.name;
                }
            }
        }
    }

    public void SpreadAwareness(int spreadAwarenessValue)
    {
        awarenessLevel += spreadAwarenessValue;
        //return returnVal;
    }

    public void SmallTrashPile(int smallTrashPileValue)
    {
        currentGarbageAmount -= smallTrashPileValue;
    }

    public void MediumTrashPile(int mediumTrashPileValue)
    {
        currentGarbageAmount -= mediumTrashPileValue;
    }

    public void InstantiateVolunteer()
    {
        if(currentGarbageAmount > 0)
        {
            Debug.Log("Can't get involved yet, clean more garbage");
        }
        else
        {
            Instantiate(newVolunteers);
        }        
    }

    public void WebsiteLink() //This is to link the Pollution Probe website 
    {
        Application.OpenURL("https://www.pollutionprobe.org"); // When a player selects a button, this function will be called
    }
}

