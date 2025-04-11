using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class Garbage : MonoBehaviour
{
    public int smallGarbageHealth = 50;
    public int mediumGarbageHealth = 100;
    public int largeGarbageHealth = 200;
    public int currentHealth;
    public string Name;
    //public bool isCollected = false;

    TruckAI truck;

    [SerializeField]
    private FloatingHealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        truck = FindAnyObjectByType<TruckAI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Name = gameObject.name;
        if (Name == "SmallGarbage")
        {
            
            currentHealth = smallGarbageHealth;
            healthBar.UpdateHealthBar(currentHealth, smallGarbageHealth);
        }
        if (Name == "MediumGarbage")
        {
            
            currentHealth = mediumGarbageHealth;
            healthBar.UpdateHealthBar(currentHealth, mediumGarbageHealth);
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
        if (Name == "SmallGarbage")
        {
            healthBar.UpdateHealthBar(currentHealth, smallGarbageHealth);
        }
        if (Name == "MediumGarbage")
        {
            healthBar.UpdateHealthBar(currentHealth, mediumGarbageHealth);
        }
        if (currentHealth <= 0 && Name == "SmallGarbage")
        {
            GameManager.Instance.involvedAmount += 10;
            truck.isDisposed = true;
            GameManager.Instance.SmallTrashPile(50);
            //GameManager.Instance.involvedAmount += 20;
            Destroy(gameObject);
            if (truck.isDisposed)
            {
                GameManager.Instance.trashCollected++;
                //truck.isDisposed = false;
                Debug.Log("Trash Collected!");
                return;
            }
        }
        else if (currentHealth <= 0 && Name == "MediumGarbage") {
            GameManager.Instance.involvedAmount += 20;
            truck.isDisposed = true;
            Debug.Log("Destroyed");
            GameManager.Instance.MediumTrashPile(100);
            //GameManager.Instance.involvedAmount += 20;
            Destroy(gameObject);
            if (truck.isDisposed)
            {
                //GameManager.Instance.trashCollected++;
                //truck.isDisposed = false;
                Debug.Log("Trash Collected!");
                return;
            }
        }
    }
}

