using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this; 
    }

    public int currentTurn;
    public int startTurn;
    public int maxTurn;
    public int currentPeople;
    public int maxPeople;
    public int currentMoney;
    public bool endTurn;
    public int happiness;
    public int awarenessLevel;
    public int currentPlantedTrees = 0;

    public int garbageLevel = 1;
    public int currentGarbageAmount; // An example is garbage will start at 100. 

    public GameObject TerrainGroup;
    public GameObject Garbage;
    public List<GameObject> tag_targets = new List<GameObject>();

    public Transform parentTransform;

    private float chanceOfGarbage = 9;
    private float randomNumber;
    private float currentx;
    private float currenty;
    private Vector3 spot;

    //private returnVal;
    // Start is called before the first frame update
    void Start()
    {
        currentTurn = startTurn;
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("End turn: " + endTurn);
        //Debug.Log(" Current Turn: " + currentTurn);
        //Debug.Log("Max turn: " + maxTurn);


        if (endTurn && currentTurn < maxTurn)
        {
            endTurn = false;
            currentTurn++;
        }
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
            currentGarbageAmount = 100;
        }
    }
    private void SpawnGarbage()
    {
        foreach (Transform child in TerrainGroup.transform)
        {
            GameObject obj = child.gameObject;
            //Debug.Log(obj.layer);
            if (obj.layer == 7)
            {
                randomNumber = Random.Range(0, 10);
                //Debug.Log(randomNumber);
                if (randomNumber >= chanceOfGarbage)
                {
                    //Debug.Log("what");
                    Debug.Log("Garbage spawn x" + obj.transform.position.x + "z: " + obj.transform.position.z);
                    spot = new Vector3(obj.transform.position.x, .51f, obj.transform.position.z);
                    Instantiate(Garbage, spot, Quaternion.identity, parentTransform);
                    // Do things with obj
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
        Application.OpenURL("https://www.pollutionprobe.org"); //When a player selects a button, this function will be called
    }
}
