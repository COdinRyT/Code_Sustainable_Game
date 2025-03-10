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
    public bool isCollected = false;

    EmptyTrashCollection empty;

    [SerializeField]
    private FloatingHealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        empty = FindAnyObjectByType<EmptyTrashCollection>();
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
        if ( Name == "SmallGarbage")
        {
            healthBar.UpdateHealthBar(currentHealth, smallGarbageHealth);
        }

        if (currentHealth <= 0)
        {
            isCollected = true;           
            GameManager.Instance.SmallTrashPile(50);
            GameManager.Instance.involvedAmount += 20;
            Destroy(gameObject);
            empty.RegisterGarbageDestruction();
        }
    }   
}
