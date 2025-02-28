using System.Collections;
using System.Collections.Generic;
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

    public GameObject TerrainGroup;
    public GameObject Garbage;
    public List<GameObject> tag_targets = new List<GameObject>();

    public Transform parentTransform;

    private float chanceOfGarbage = 9;
    private float randomNumber;
    private float currentx;
    private float currenty;
    private Vector3 spot;

    NewBehaviourScript pointClickMovement; 


    // Start is called before the first frame update
    void Start()
    {
        currentTurn = startTurn;
        SpawnGarbage();
        pointClickMovement = FindAnyObjectByType<NewBehaviourScript>();
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
                //Debug.Log(randomNumber);
                if (randomNumber >= chanceOfGarbage)
                {
                    //Debug.Log("what");
                    spot = new Vector3(obj.transform.position.x, .51f, obj.transform.position.z);
                    Instantiate(Garbage, spot, Quaternion.identity, parentTransform);
                    // Do things with obj
                }
            }
        }
    }

    public void OnClick()
    {
        if(pointClickMovement.isTileSelected)
        {
            pointClickMovement.MovePlayer(pointClickMovement.selectedTile.transform.position);
        }
    }

    public void WebsiteLink() //This is to link the Pollution Probe website 
    {
        Application.OpenURL("https://www.pollutionprobe.org"); //When a player selects a button, this function will be called
    }
}
