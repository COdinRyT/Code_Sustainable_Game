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

        if (this.enabled == true)
        {
            Debug.Log($"Agent {gameObject.name} has been added to queue");
            gameManager.ConfirmVolunteer(gameObject);
            
        }
        else
        {
            Debug.Log("Agent is not in the queue");
        }
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
    public IEnumerator MovePlayer(GameObject character, GameObject tile)
    {
        NavMeshAgent agent = character.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"{character.name} does not have a NavMeshAgent component!");
            yield break;
        }

        // Get the target position
        xdecimalPoint = Mathf.Round(tile.transform.position.x);
        zdecimalPoint = Mathf.Round(tile.transform.position.z);
        Vector3 targetPosition = new Vector3(xdecimalPoint, 1, zdecimalPoint);

        // Set the NavMesh destination
        agent.SetDestination(targetPosition);
        Debug.Log($"{character.name} moving to {targetPosition}");

        // Wait until the agent reaches the destination
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;  // Wait until next frame
        }

        // Ensure the character is exactly at the position
        character.transform.position = targetPosition;
    }

    
    
}
