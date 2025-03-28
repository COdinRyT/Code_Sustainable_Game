using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TruckAI : MonoBehaviour
{
    [SerializeField]private Vector3 targetDestination; // Assign this in inspector
    public NavMeshAgent agent;
    public float truckSpeed = 5f;
    EmptyTrashCollection emptyTrash;
    Shop shop;
    private int timesCollected;
    public Vector3 spawnPositions;

    private GameObject player;
    [SerializeField]public float collectionRange = 3f;
    private bool withinRange;

    public Slider collectionSlider;
    [SerializeField] private int garbageCollected = 0;
    [SerializeField] private int maxGarbageCollect = 10;
    public bool isDisposed;

    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        gameObject.SetActive(true);
        agent.speed = truckSpeed;
        agent.isStopped = false; // Ensure the agent is moving
        emptyTrash = FindAnyObjectByType<EmptyTrashCollection>();
        shop = FindAnyObjectByType<Shop>();

        //timesCollected = collectCounters.Length; //The amount of times the truck collects trash
        //is equal to the amount of box trackers on top of the truck
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = spawnPositions; 
        MoveTruck();
        if(collectionSlider != null)
        {
            collectionSlider = GetComponentInChildren<Slider>();
            collectionSlider.value = garbageCollected;
            collectionSlider.maxValue = maxGarbageCollect;
        }
        else
        {
            Debug.Log("Collection slider is null! ensure it is assigned");
        }
        withinRange = false;
        gameObject.SetActive(true);
        //CollectTrash();
    }

    public void MoveTruck()
    {
        if(timesCollected > 0)
        {
            Vector3 position = targetDestination;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(position, out hit, 5f, NavMesh.AllAreas))
            {
                targetDestination = hit.position;
                agent.SetDestination(targetDestination);
            }
            else
            {
                Debug.LogError("No valid Nav");
            }
        }
        if(timesCollected <= 0)
        {
            Vector3 moveToSpawnPosition = spawnPositions;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(moveToSpawnPosition, out hit, 10f, NavMesh.AllAreas))
            {
                targetDestination = hit.position;
            }
            else
            {
                Debug.LogWarning("No valid NavMesh position found! Moving truck to a fallback location.");
                //targetDestination = new Vector3(0, 0, 0); // Example fallback position
            }
            agent.SetDestination(targetDestination);
            if (gameObject.transform.position == spawnPositions)
            {
                gameObject.SetActive(false);
                timesCollected = 2;
            }
            
        }

    }

    public void TrashCollection()
    {
        
        if(GameManager.Instance.trashCollected > 0 && withinRange)
        {
            garbageCollected++;
            UpdateSlider();
            GameManager.Instance.trashCollected--;
        }
        else
        {
            Debug.Log("No garbage to collect or get closer");
        }
    }

    private void UpdateSlider()
    {
        Debug.Log("Updating Slider: " + garbageCollected);
        collectionSlider.value = garbageCollected;
    }

    private void Update()
    {
        //Debug.Log("Has Path: " + agent.hasPath);
        //Debug.Log("Path Status: " + agent.pathStatus);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
        MoveTruck();
        UpdateSlider();
    }
}
