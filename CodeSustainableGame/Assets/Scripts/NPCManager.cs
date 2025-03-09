using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private Vector3 spawnPosition;
    public float spawnCounter;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        spawnPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckIfOnGarbage.Instance.PlayerAndGarbageCollision)
        {
            gameObject.SetActive(true);
        }
    }

}
