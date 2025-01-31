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
    // Start is called before the first frame update
    void Start()
    {
        currentTurn = startTurn;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("End turn: " + endTurn);
        Debug.Log(" Current Turn: " + currentTurn);
        Debug.Log("Max turn: " + maxTurn);
        if (endTurn && currentTurn < maxTurn)
        {
            endTurn = false;
            currentTurn++;
        }
    }

    public void WebsiteLink() //This is to link the Pollution Probe website 
    {
        Application.OpenURL("https://www.pollutionprobe.org"); //When a player selects a button, this function will be called
    }
}
