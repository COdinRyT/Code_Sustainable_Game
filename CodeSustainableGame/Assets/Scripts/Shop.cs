using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{
    //public Text ShopButton;
    public GameObject Panel;
    public GameObject smallTruck;
    TruckAI truck;

    [SerializeField] int smallTruckCost = 125000;
    [SerializeField] private int turnsuntilnewtruck = 4;
    //private int currentTurn;
    public bool truckSpawns;

    public static Shop Instance { get; private set; }

    public void ShopButtonClick()
    {
        if (Panel.activeSelf)
        {
            Panel.SetActive(false);
        }
        else
        {
            Panel.SetActive(true);
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        truck = FindAnyObjectByType<TruckAI>();
        truckSpawns = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (truck.isLeft && truckSpawns == true)
        {
            turnsuntilnewtruck--;
            truckSpawns = false;
            if(turnsuntilnewtruck <= 0)
            {
                SpawnTruck();
                truck.isLeft = false;
            }
        }
    }

    public void SpawnTruck()
    {
        if(GameManager.Instance.currentMoney >= 125000 || truck.isLeft == true)
        {
            if (smallTruck != null)
            {

                Instantiate(smallTruck, smallTruck.transform.position, Quaternion.identity);
                GameManager.Instance.currentMoney -= smallTruckCost;
            }
            else
            {
                Debug.LogWarning("Small truck prefab is not assgined");
            }
        }
        
    }
}
