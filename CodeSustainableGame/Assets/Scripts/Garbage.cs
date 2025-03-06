using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    public int smallGarbageHealth = 50;
    public int mediumGarbageHealth = 100;
    public int largeGarbageHealth = 200;
    public int currentHealth;
    public string Name;
    // Start is called before the first frame update
    void Start()
    {
        Name = gameObject.name;
        if (Name == "SmallGarbage")
        {
            currentHealth = smallGarbageHealth;
        }
    }
    // Ensure the garbage has a trigger collider
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Other: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Garbage and Character are on the same tile!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
