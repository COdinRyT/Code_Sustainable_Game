using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    private Camera camera;
    public NavMeshAgent agent;
    GameManager gameManager;

    public float playerSpeed = 5f;  // Adjust speed for turn-based feel
    public float stepDelay = 0.2f;  // Delay between tile movements
    

    private Rigidbody rb;
    public GameObject selectedPlayer = null;  // The character that the player selects
    public GameObject selectedTile = null; // The tile that the player selects
    //Booleans to keep track on whether player or tile have been selected
    private bool isPlayerSelected;
    private bool isTileSelected;
    //This is to keep track of how many times an object is selected
    private int timesSelected;
    private GameObject lastPrefab;

    private Vector3 normalizePoint;
    private Vector3 midNormalizePoints;
    private float xdecimalPoint;
    private float zdecimalPoint;


    private void Awake()
    {
        camera = Camera.main;
        rb = GetComponent<Rigidbody>();
        gameManager = FindAnyObjectByType<GameManager>();
        isPlayerSelected = false;
        isTileSelected = false;
    }
    private void Update()
    {
        
    }

    //When this function is called, make the player the selected game object 
    public void SelectPlayer(GameObject player)
    {
        selectedPlayer = player;
        isPlayerSelected = true;
        Debug.Log("Player has been Selected");

        //if (selectedPlayers.Contains(player))
        //{
        //    selectedPlayers.Remove(player);
        //    Debug.Log("Player deselected");
        //}
        //else
        //{
        //    selectedPlayers.Add(player);
        //    Debug.Log("Player has been Selected");
        //}
    }

    public void SelectTile(GameObject tile, GameObject marker)
    {
        //if(lastPrefab != null)
        //{
        //    Destroy(lastPrefab);
        //}

        selectedTile = tile;
        isTileSelected = true;

        Instantiate(marker, selectedTile.transform.position, Quaternion.identity) ;
        Debug.Log("Tile selected");
    }

    //Move the player when this function is called
    public IEnumerator MovePlayer(GameObject player, GameObject targetPoint)
    {

        NavMeshAgent playerAgent = player.GetComponent<NavMeshAgent>();
        if(playerAgent == null)
        {
            Debug.Log($"{player.name} does not have their nav agent!");
            yield break;
        }

        xdecimalPoint = Mathf.Round(targetPoint.transform.position.x);
        zdecimalPoint = Mathf.Round(targetPoint.transform.position.z);
        midNormalizePoints = new Vector3(xdecimalPoint, 1, zdecimalPoint);
        //Debug.Log(normalizePoint);
        //Debug.Log(midNormalizePoints);
        playerAgent.SetDestination(midNormalizePoints);
        //CheckIfOnGarbage.Instance.x = midNormalizePoints.x;
        //CheckIfOnGarbage.Instance.y = midNormalizePoints.y;
        //CheckIfOnGarbage.Instance.z = midNormalizePoints.z;
        //CheckIfOnGarbage.Instance.CheckCollisionBetweenPlayerAndGarbage();

        while(playerAgent.pathPending || playerAgent.remainingDistance > 0.1f)
        {
            yield return null; //Wait for the next frame when the previous character
            //moves within a distance of 0.1 of target
        }
    }

    
    
}
