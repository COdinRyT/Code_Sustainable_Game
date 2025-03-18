using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyTrashCollection : MonoBehaviour
{

    [SerializeField] private int totalGarbage = 10;
    TruckAI truck;
    private int garbageDestroyed = 0; 
    public bool isDisposed = false;

    public event Action OnTrashEmptied; //Event to notify the truck

    // Start is called before the first frame update
    [SerializeField]
    private void Start()
    {
        truck = FindAnyObjectByType<TruckAI>();        
    }

    public void RegisterGarbageDestruction()
    {
        garbageDestroyed++;
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        
    }

    public void EmptyTrash()
    {
        if (garbageDestroyed > 0 && truck != null)
        {
            isDisposed = true;
            OnTrashEmptied?.Invoke();
        }
        else
        {
            Debug.Log("There's no garbage for truck pick up");
        }
    }
}
