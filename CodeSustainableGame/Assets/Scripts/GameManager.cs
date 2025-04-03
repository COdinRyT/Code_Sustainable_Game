using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    private bool hasStarted = false;
    public UpdateUI updateUI;
    public GameObject volunterSpawnPosition;
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

    //public float involvedAmount;
    //public float involvedNeededLevelUp = 100;

    public float garbageLevel = 1;
    public float maxGarbage;
    public float currentGarbageAmount; // An example is garbage will start at 100. 

    public bool readyForGetInvolved = false;
    public bool endTurn = false;

    public Button skipButton;  // Drag the Skip Button here from the Unity Editor

    public GameObject TerrainGroup;
    public GameObject Garbage;
    public GameObject[] garbagePiles;
    public List<GameObject> tag_targets = new List<GameObject>();
    public Transform parentTransform;
    public Transform garbageParentTransform;
    Camera camera;
    private bool spawnUnit = false;
    private float chanceOfGarbage = 9;
    private float randomNumber;

    private bool readyToGetInvolved;
    public GameObject Volunteer;
    private bool hasTaskStarted = false;  // Add a flag to track if the task has started

    public int trashCollected;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (hasStarted == false)
        {
            hasStarted = true;
            maxGarbage = garbageLevel * 100;
            currentGarbageAmount = garbageLevel * 100;
        }
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
        //updateUI.UpdateQueueUI(new List<GameObject>(characters));

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
    /*
    public void GetInvolvedClick()
    {
        if (readyToGetInvolved)
        {
            Volunteer = Instantiate(Volunteer, volunterSpawnPosition.transform.position, Quaternion.identity, garbageParentTransform);
            Volunteer.name = "Worker";
            //VolunterClone.transform.rotation *= Quaternion.Euler(0, 90f, 0);
        }
    }
    */
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
    public void OnSkipButtonClick()
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
        Volunteer = Instantiate(Volunteer, volunterSpawnPosition.transform.position, Quaternion.identity, garbageParentTransform);
        Volunteer.name = "Worker";
        Debug.Log("First player function");
        DoTask();
    }
    public void resetGarbage()
    {
        currentGarbageAmount = garbageLevel * 100;
        //involvedAmount += 20;
        happiness += 10;
        readyToGetInvolved = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (involvedAmount >= involvedNeededLevelUp)
        //{
        //    spawnUnit = true;
        //    involvedAmount = 0;
        //}
        if (currentGarbageAmount <= 0 && hasStarted == true)
        {
            garbageLevel += 1;
            maxGarbage = garbageLevel * 100;
            Debug.Log("Set happiness");
            resetGarbage();

        }
        if (happiness >= 100) // This is how you win the game
        {
            Debug.Log("Won by happiness");
            EndGameWin();
        }
        if (currentTurn >= 50)// This is how you lose the game
        {
            Debug.Log("Loss by turns");
            EndGameLose();
        }
        //updateUI.UpdateQueueUI(new List<GameObject>(characters));
        if (endTurn && currentTurn < maxTurn)
        {
            GameManager.Instance.GetInvolvedIsTrue();
            //updateUI.UpdateQueueUI(new List<GameObject>(characters));
            //Debug.Log("Brh");
            if (endTurn && currentTurn < maxTurn)
            {
                if (spawnUnit)
                {
                    Volunteer = Instantiate(Volunteer, volunterSpawnPosition.transform.position, Quaternion.identity, garbageParentTransform);
                    Volunteer.name = "Worker";
                    spawnUnit = false;
                }
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
    void EndGameLose()
    {
        SceneManager.LoadScene("LostScreen");
    }
    void EndGameWin()
    {
        SceneManager.LoadScene("WinScreen");
    }
    public void GetInvolvedIsTrue()
    {
        Debug.Log("Update glow");
        //GlowAndSparkle.Instance.transparency = 100;
    }
    void StartGame()
    {
        SetupVariables();
        SpawnGarbage();
    }
    void SetupVariables()
    {
        //currentMoney = 0;
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

            //updateUI.UpdateQueueUI(new List<GameObject>(characters));

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
    }

    private void SpawnGarbage()
    {
        GameObject GarbageClone;
        if (Garbage == null)
        {
            Debug.Log("Garbage prefab is not assigned in inspector");
            return;
        }
        if (TerrainGroup == null)
        {
            Debug.Log("Terraingroup is not assigned");
            return;
        }
        foreach (Transform child in TerrainGroup.transform)
        {
            GameObject obj = child.gameObject;
            if (obj.layer == 7)
            {
                randomNumber = UnityEngine.Random.Range(0, 10);
                if (randomNumber >= chanceOfGarbage)
                {
                    // Get the center of the tile (obj.transform.position should be the center of the tile)
                    Vector3 tileCenter = obj.transform.position;

                    // Set the garbage spawn position to be slightly above the tile (e.g., 1 unit above)
                    Vector3 spawnPosition = new Vector3(tileCenter.x, tileCenter.y + 1f, tileCenter.z);

                    // Instantiate the garbage at the calculated spawn position
                    GarbageClone = Instantiate(Garbage, spawnPosition, Quaternion.identity, parentTransform);
                    GarbageClone.transform.rotation *= Quaternion.Euler(0, 90f, 0);
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
    public void WebsiteLink() //This is to link the Pollution Probe website 
    {
        Application.OpenURL("https://www.pollutionprobe.org"); // When a player selects a button, this function will be called
    }
}

