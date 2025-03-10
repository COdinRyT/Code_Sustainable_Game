using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TruckAI : MonoBehaviour
{
    private Vector3 targetDestination; // Assign this in inspector
    public NavMeshAgent agent;
    public float truckSpeed = 5f;

    private void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.speed = truckSpeed;
        agent.isStopped = false; // Ensure the agent is moving
    }

    private void Start()
    {
        MoveTruck();
    }

    public void MoveTruck()
    {
        Vector3 position = new Vector3(29f, transform.position.y, -50f);
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

    private void Update()
    {
        Debug.Log("Has Path: " + agent.hasPath);
        Debug.Log("Path Status: " + agent.pathStatus);

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 5f))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }


}
