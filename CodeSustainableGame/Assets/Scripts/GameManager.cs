using System.Collections;
using System.Collections.Generic;
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

    public GameObject TerrainGroup;
    public GameObject Garbage;
    public List<GameObject> tag_targets = new List<GameObject>();

    private float chanceOfGarbage = 6;
    private float randomNumber;
    // Start is called before the first frame update
    void Start()
    {
        currentTurn = startTurn;
        SpawnGarbage();
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
    private void SpawnGarbage()
    {
        foreach (Transform child in TerrainGroup.transform)
        {
            GameObject obj = child.gameObject;
            //Debug.Log(obj.layer);
            if (obj.layer == 7)
            {
                randomNumber = Random.Range(0, 10);
                Debug.Log(randomNumber);
                if (randomNumber >= chanceOfGarbage)
                {
                    Debug.Log("what");
                    Instantiate(Garbage, obj.transform.position, Quaternion.identity);
                    // Do things with obj
                }
            }
        }
    }

    public void WebsiteLink() //This is to link the Pollution Probe website 
    {
        Application.OpenURL("https://www.pollutionprobe.org"); //When a player selects a button, this function will be called
    }
}
