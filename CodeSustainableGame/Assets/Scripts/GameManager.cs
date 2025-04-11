using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using static TutorialManager;
public class GameManager : MonoBehaviour
{
    private bool hasStarted = false;
    public UpdateUI updateUI;
    public GameObject volunterSpawnPosition;
    public Queue<GameObject> characters = new Queue<GameObject>(); // Character queue
    public GameObject newVolunteers;
    public GameObject volunteerPrefab;  // Rename for clarity
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
    public GameObject currentActiveCharacter;

    public float involvedAmount;
    public float involvedNeededLevelUp = 100;

    public float garbageLevel = 1;
    public float maxGarbage;
    public float currentGarbageAmount; // An example is garbage will start at 100. 

    public bool readyForGetInvolved = false;
    public bool endTurn = false;

    public Button skipButton;  // Drag the Skip Button here from the Unity Editor
    private bool spawnNextTurn = false;
    private bool spawnTriggered = false;
    public GameObject TerrainGroup;
    public GameObject SmallGarbage;
    public GameObject MediumGarbage;
    public GameObject[] garbagePiles;
    public GameObject GarbageCloneMediumTutorial;
    public List<GameObject> tag_targets = new List<GameObject>();
    public Transform parentTransform;
    public Transform garbageParentTransform;
    new Camera camera;
    private bool spawnUnit = false;
    private float chanceOfSmallGarbage = 800;
    private float chanceOfMediumGarbage = 950;
    private float chanceOfGarbage = 9;
    private float randomNumber;
    private float randomNumberTwo;
    // To store tile positions with garbage already spawned
    private HashSet<Vector3> tilesWithGarbage = new HashSet<Vector3>();
    //private bool readyToGetInvolved = false;
    private bool readyToGetInvolved;
    public GameObject Volunteer;
    private bool hasTaskStarted = false;  // Add a flag to track if the task has started
    TruckAI truckAI;
    public int trashCollected;
    public bool tutorialGarbageWasRemoved = false;
    public int startingNumberUnits = 1;


    public GameObject worker1;
    public GameObject worker2;
    public GameObject worker3;
    public GameObject worker4;     
    public GameObject worker5;
    public GameObject worker6;
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
        GameObject GarbageCloneMedium;
        camera = Camera.main;
        currentTurn = startTurn;
        StartGame();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Tutorial")
        {
            //Vector3 spawnPosition = new Vector3(20, .5f, -65);
            //GarbageCloneMedium = Instantiate(GarbageCloneMediumTutorial, spawnPosition, Quaternion.identity, parentTransform);
            //GarbageCloneMedium.name = MediumGarbage.name;
        }
        else
        {
            SpawnGarbage();
        }
        updateUI = FindAnyObjectByType<UpdateUI>();
        truckAI = FindAnyObjectByType<TruckAI>();
        //updateUI.UpdateQueueUI(new List<GameObject>(characters));

        if (skipButton != null)
        {
            //skipButton.onClick.AddListener(OnSkipButtonClick);
        }
        if (volunterSpawnPosition == null)
        {
            volunterSpawnPosition = GameObject.Find("VolunteerSpawn");
            if (volunterSpawnPosition == null)
            {
                Debug.LogError("VolunteerSpawn object not found in the scene!");
            }

            if (updateUI == null)
            {
                Debug.Log("UI manager is not assigned to game manager!");
            }
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
    public void FirstPlayer()
    {
        //Volunteer = Instantiate(Volunteer, volunterSpawnPosition.transform.position, Quaternion.identity, garbageParentTransform);
        //Volunteer.name = "Worker";
        //Debug.Log("First player function");
        DoTask();
    }
    public void resetGarbage()
    {
        currentGarbageAmount = garbageLevel * 100;

        happiness += 10;
        readyToGetInvolved = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (involvedAmount >= involvedNeededLevelUp)
        {
            spawnNextTurn = true;
            spawnTriggered = true;
            if (startingNumberUnits == 1)
            {
                startingNumberUnits++;
                worker2.SetActive(true);
                ConfirmVolunteer(worker2);
            }
            else if (startingNumberUnits == 2)
            {
                startingNumberUnits++;
                worker3.SetActive(true);
                ConfirmVolunteer(worker3);
            }
            else if (startingNumberUnits == 3)
            {
                startingNumberUnits++;
                worker4.SetActive(true);
                ConfirmVolunteer(worker4);
            }
            else if (startingNumberUnits == 4)
            {
                startingNumberUnits++;
                worker5.SetActive(true);
                ConfirmVolunteer(worker5);
            }
            else if (startingNumberUnits == 5)
            {
                startingNumberUnits++;
                worker6.SetActive(true);
                ConfirmVolunteer(worker6);
            }

            involvedAmount = 0;
        }
        if (currentGarbageAmount <= 0 && hasStarted == true)
        {
            garbageLevel += 1;
            maxGarbage = garbageLevel * 100;
            //Debug.Log("Set happiness");
            resetGarbage();

        }
        if (happiness >= 100) // This is how you win the game
        {
            //Debug.Log("Won by happiness");
            EndGameWin();
        }
        if (currentTurn >= 50)// This is how you lose the game
        {
            //Debug.Log("Loss by turns");
            EndGameLose();
        }
        //updateUI.UpdateQueueUI(new List<GameObject>(characters));
        //GameManager.Instance.GetInvolvedIsTrue();
        //updateUI.UpdateQueueUI(new List<GameObject>(characters));
        //Debug.Log("Brh");
        if (endTurn && currentTurn < maxTurn)
        {
            if (spawnNextTurn)
            {
                Vector3 spawnPosition = new Vector3(20, .5f, -65);
                //GameObject newVolunteer = Instantiate(volunteerPrefab, spawnPosition, Quaternion.identity, garbageParentTransform);
                //newVolunteer.name = "Worker";
                //ConfirmVolunteer(newVolunteer);
                
                spawnNextTurn = false;
                spawnTriggered = false;
            }
            // Find all game objects with the tag "Player" (or any tag you've assigned to your characters)
            GameObject[] allCharacters = GameObject.FindGameObjectsWithTag("Player");

            // Loop through each character and call a function (e.g., CheckCollisionBetweenPlayerAndGarbage)
            foreach (GameObject character in allCharacters)
            {
                //Debug.Log($"Checking for garbage for character: {character.name}");

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
                    //Debug.LogWarning($"Player {character.name} does not have the CheckIfOnGarbage script attached.");
                }
            }
            //Debug.Log("Up");
            endTurn = false;
            currentTurn++;
            DoTask();
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
        //Debug.Log("Update glow");
        //GlowAndSparkle.Instance.transparency = 100;
    }
    void StartGame()
    {
        SetupVariables();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        //Debug.Log("Current Scene Name: " + sceneName);
        if (sceneName == "Tutorial")
        {
        }
        else
        {
            SpawnGarbage();
        }
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
            Debug.Log($"Added {character.name} to queue");

            // 👇 Immediately reset skipMove when added
            /* Jonah temp disabled to see if this is affecting queue
            var movement = character.GetComponent<PointClickMovement>();
            if (movement != null)
            {
                movement.skipMove = false;
                Debug.Log($"{character.name} => skipMove set to FALSE in ConfirmVolunteer()");
            }
            */
        }
        else
        {
            Debug.Log($"{character.name} already in queue");
        }
    }


    public void DoTask()
    {
        if (hasTaskStarted) return;
        hasTaskStarted = true;

        //Debug.Log("DoTask() called. Resetting skipMove flags");
        /*
        foreach (GameObject character in characters)
        {
            PointClickMovement movement = character.GetComponent<PointClickMovement>();
            if (movement != null)
            {
                movement.skipMove = false;
                //Debug.Log($"{character.name} => skipMove set to FALSE in DoTask()");
            }
        }
        */
        StartCoroutine(MoveCharacterSequence());
    }

    // Coroutine to move characters one at a time, waiting for click before each character moves
    private IEnumerator MoveCharacterSequence()
    {
        // Save a temporary list of all characters in the queue
        List<GameObject> charactersInCurrentTurn = new List<GameObject>(characters);
        /*
        foreach (var character in charactersInCurrentTurn)
        {
            var movement = character.GetComponent<PointClickMovement>();
            if (movement != null)
                movement.skipMove = false;
        }
        */
        Debug.Log("Character count is: " + characters.Count);
        while (characters.Count > 0)
        {
            GameObject currentCharacter = characters.Dequeue();
            //currentActiveCharacter = currentCharacter;
            // Get the PointClickMovement component from the current character

            PointClickMovement characterMovement = currentCharacter.GetComponent<PointClickMovement>();

            if (characterMovement != null)
            {
                //Debug.Log($"{currentCharacter.name} starting MovePlayer. skipMove = {characterMovement.skipMove}");
                characterMovement.SelectPlayer(currentCharacter);
                
            }
            yield return StartCoroutine(characterMovement.MovePlayer());
            //wcurrentActiveCharacter = null;
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
        GameObject GarbageCloneMedium;
        if (SmallGarbage == null || MediumGarbage == null)
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
            if (obj.layer != 7) // Assuming layer 7 is for tiles with garbage potential
                continue;

            // Get the center of the title
            Vector3 tileCenter = obj.transform.position;

            if (tilesWithGarbage.Contains(tileCenter))
            {
                continue; // Skip spawning garbage if its already on a tile
            }
            float randomNumber = UnityEngine.Random.Range(0f, 1000f);
            if (randomNumber >= 993f)// Chance for small garbage
            {
                Vector3 spawnPosition = new Vector3(tileCenter.x, tileCenter.y + 1f, tileCenter.z);
                GarbageClone = Instantiate(SmallGarbage, spawnPosition, Quaternion.identity, parentTransform);
                GarbageClone.name = SmallGarbage.name;
                tilesWithGarbage.Add(tileCenter);
            }
            randomNumber = UnityEngine.Random.Range(0f, 1000f);
            if (randomNumber >= 997f)  // Chance for medium garbage
            {
                // Spawn medium garbage above the tile
                Vector3 spawnPosition = new Vector3(tileCenter.x, tileCenter.y + 1f, tileCenter.z);
                GarbageCloneMedium = Instantiate(MediumGarbage, spawnPosition, Quaternion.identity, parentTransform);
                GarbageCloneMedium.name = MediumGarbage.name;

                // Mark this tile as having garbage
                tilesWithGarbage.Add(tileCenter);
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

