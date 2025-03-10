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
    public Vector3 spawnPos;

    [SerializeField] int smallTruckCost = 125000;
    private int turnsUntilNewTruck = 4;
    private int currentTurn;
    private bool truckSpawns;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTruck()
    {
        if(GameManager.Instance.currentMoney >= 125000)
        {
            if (smallTruck != null)
            {
                Instantiate(smallTruck, spawnPos, Quaternion.identity);
                GameManager.Instance.currentMoney -= smallTruckCost;
            }
            else
            {
                Debug.Log("Small truck prefab is not assgined");
            }
        }       

    }
}
