using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TruckAI : MonoBehaviour
{
    [SerializeField]private Vector3 targetDestination; // Assign this in inspector
    public NavMeshAgent agent;
    public NavMeshSurface surface;
    public float truckSpeed = 5f;
    EmptyTrashCollection emptyTrash;
    Shop shop;
    private int timesCollected;
    public Vector3 spawnPositions;
    DepositTrash depositTrash;

    private GameObject player;
    [SerializeField]public float collectionRange = 3f;
    public bool withinRange;
    public bool isLeft;

    public Slider collectionSlider;
    [SerializeField] private int garbageCollected = 0;
    [SerializeField] private int maxGarbageCollect = 5;
    public bool isDisposed;
    public int turnCounter = 3;

    public int moneyAmount;

    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

        gameObject.SetActive(true);
        agent.speed = truckSpeed;
        agent.isStopped = false; // Ensure the agent is moving
        emptyTrash = FindAnyObjectByType<EmptyTrashCollection>();
        shop = FindAnyObjectByType<Shop>();
        depositTrash = FindAnyObjectByType<DepositTrash>();

        //timesCollected = collectCounters.Length; //The amount of times the truck collects trash
        //is equal to the amount of box trackers on top of the truck

        Invoke("EnableNavMesh", 0.025f);
    }

    private void EnableNavMesh()
    {
        agent.enabled = true;
    }

    private IEnumerator DelayedMoveTruck()
    {
        yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds to ensure agent registers
        MoveTruck(); // Now call MoveTruck safely
    }

    private void Start()
    {
        if(surface == null)
        {
            surface = FindObjectOfType<NavMeshSurface>();
        }     

        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPositions, out hit, 5f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogError("Truck spawn position is NOT on the NavMesh!");
        }

        StartCoroutine(DelayedMoveTruck()); // Delay movement to avoid NavMesh issues

        if (collectionSlider != null)
        {
            collectionSlider = GetComponentInChildren<Slider>();
            collectionSlider.value = garbageCollected;
            collectionSlider.maxValue = maxGarbageCollect;
        }
        else
        {
            Debug.LogWarning("Collection slider is null! Ensure it is assigned.");
        }

        withinRange = false;
        isLeft = false;
        gameObject.SetActive(true);
    }
    private int turnDisappear = -1; // Ryan is this where this goes?
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }

        MoveTruck();
        UpdateSlider();

        //if (isLeft && GameManager.Instance.currentTurn >= GameManager.Instance.currentTurn + 3)
        //{
        //    gameObject.GetComponent<Renderer>().enabled = true; // Make it reappear
        //    MoveTruck() ;

        //}
    }

    public void MoveTruck()
    {
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing on TruckAI!");
            return;
        }

        if (!agent.isActiveAndEnabled)
        {
            Debug.LogError("NavMeshAgent is not active or enabled!");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("Truck is NOT on a NavMesh!");
            return;
        }

        if (collectionSlider.value >= 0)
        {
            Vector3 position = targetDestination;
            //int randomPoint = Random.Range(0, targetDestination.Length);

            //Vector3 position = targetDestination[randomPoint];
            NavMeshHit hit;
            if (NavMesh.SamplePosition(position, out hit, 5f, NavMesh.AllAreas))
            {
                targetDestination = hit.position;
                agent.SetDestination(targetDestination);
                //targetDestination[randomPoint] = hit.position;
                //agent.SetDestination(targetDestination[randomPoint]);
            }
            else
            {
                Debug.LogError("No valid NavMesh position found for target destination!");
            }
        }

        if (collectionSlider.value == maxGarbageCollect)
        {
            //int randomPos = Random.Range(0, targetDestination.Length);
            Vector3 moveToSpawnPosition = spawnPositions;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(moveToSpawnPosition, out hit, 10f, NavMesh.AllAreas))
            {
                targetDestination = hit.position;
                //targetDestination[randomPos] = hit.position;
            }
            else
            {
                Debug.LogWarning("No valid NavMesh position found! Moving truck to fallback location.");
            }
            agent.SetDestination(targetDestination);
            //agent.SetDestination(targetDestination[randomPos]);

            if (Vector3.Distance(transform.position, spawnPositions) < 0.5f) // Check if the truck reached the spawn
            {
                collectionSlider.value = 0;
                isLeft = true;
                GainMoney();
                Destroy(gameObject);
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
        //Debug.Log("Updating Slider: " + garbageCollected);
        collectionSlider.value = garbageCollected;
    }

    private void GainMoney()
    {
        GameManager.Instance.currentMoney += moneyAmount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, 14);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            withinRange = true;
            Debug.Log($"Character {other.gameObject.name} is within range");
            TrashCollection();
            //if (withinRange)
            //{
            //    depositTrash.UpdateProgress();
            //    if(depositTrash.depositSlider.value == depositTrash.depositSlider.maxValue)
            //    {
            //        TrashCollection();
            //    }
            //}
            
        }
    }
}
