using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyTrashCollection : MonoBehaviour
{

    [SerializeField] private Slider progressSlider;
    [SerializeField] private int totalGarbage = 10;
    [SerializeField] private int trashToRemovePerEmpty = 5;
    TruckAI truck;
    public int garbageDestroyed = 0;
    public bool isDisposed = false;
    [SerializeField]private GameObject player;

    public event Action OnTrashEmptied; //Event to notify the truck

    private void Start()
    {
        // Ensure progress bar is initialized
        if (progressSlider != null)
        {
            progressSlider.maxValue = totalGarbage;
            progressSlider.value = 0;
        }
    }

    public void RegisterGarbageDestruction()
    {
        if (garbageDestroyed < totalGarbage)
        {
            garbageDestroyed++;
            UpdateProgressBar();
        }
        else
        {
            Debug.Log("Garbage storage is full, empty before collecting more");
        }
    }

    private void UpdateProgressBar()
    {
        if (progressSlider != null)
        {
            progressSlider.value = garbageDestroyed;
        }
    }

    public void EmptyTrash()
    {
        if (garbageDestroyed > 0)
        {
            isDisposed = true;

            // Ensure we don't remove more than available
            int trashRemoved = Mathf.Min(trashToRemovePerEmpty, garbageDestroyed);
            garbageDestroyed -= trashRemoved;
            progressSlider.value += trashRemoved;

            Debug.Log($"Trash emptied! Removed {trashRemoved} garbage. Remaining: {garbageDestroyed}");
            OnTrashEmptied?.Invoke();
            UpdateProgressBar();
        }
        else
        {
            Debug.Log("No garbage to empty.");
        }
    }

    public bool CanDestroyMoreGarbage()
    {
        return garbageDestroyed < totalGarbage;
    }
}
