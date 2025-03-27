using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TruckAI : MonoBehaviour
{
    [SerializeField]private Vector3 targetDestination; // Assign this in inspector
    public NavMeshAgent agent;
    public float truckSpeed = 5f;
    EmptyTrashCollection emptyTrash;
    Shop shop;
    private int timesCollected;
    public Vector3 spawnPositions;

    public GameObject[] collectCounters;

    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        gameObject.SetActive(true);
        agent.speed = truckSpeed;
        agent.isStopped = false; // Ensure the agent is moving
        emptyTrash = FindAnyObjectByType<EmptyTrashCollection>();
        shop = FindAnyObjectByType<Shop>();

        if(emptyTrash != null)
        {
            emptyTrash.OnTrashEmptied += CollectTrash; //Subscribe to trash emptied event
        }

        timesCollected = collectCounters.Length; //The amount of times the truck collects trash
        //is equal to the amount of box trackers on top of the truck
    }

    private void Start()
    {
        transform.position = spawnPositions; 
        MoveTruck();
        CollectTrash();
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
                if (timesCollected > 0 && gameObject != null)
                {
                    int index = collectCounters.Length;
                    for (int i = 0; i < index; i++)
                    {
                        collectCounters[index].SetActive(true);
                    }
                }
            }
            
        }

    }

    public void CollectTrash()
    {
        if (emptyTrash != null && emptyTrash.isDisposed)
        {
            Debug.Log("Trash was collected");
            emptyTrash.isDisposed = false;

            if(timesCollected > 0)
            {
                timesCollected--;
                if(collectCounters.Length > 0)
                {
                    int indexToDisable = collectCounters.Length - timesCollected - 1;
                    if(indexToDisable >= 0 && indexToDisable < collectCounters.Length)
                    {
                        collectCounters[indexToDisable].SetActive(false);
                    }
                }
            }
        }
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
    }

    private void OnDestroy()
    {
        if(emptyTrash != null)
        {
            emptyTrash.OnTrashEmptied -= CollectTrash; //Unsubscribe from event
        }
    }


}
