using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyTrashCollection : MonoBehaviour
{

    [SerializeField] private Slider progressSlider;
    [SerializeField] private Image progressImage;
    [SerializeField] private int totalGarbage = 10;
    TruckAI truck;
    private int garbageDestroyed = 0;
    public bool isDisposed = false;

    public event Action OnTrashEmptied; //Event to notify the truck

    // Start is called before the first frame update
    [SerializeField]
    private void Start()
    {
        if (progressSlider != null)
        {
            progressSlider.maxValue = totalGarbage;
            progressSlider.value = 0; //Start at 0
        }
        truck = FindAnyObjectByType<TruckAI>();
    }

    public void RegisterGarbageDestruction()
    {
        garbageDestroyed++;
        UpdateProgressBar();
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
        if (garbageDestroyed > 0 && truck != null)
        {
            isDisposed = true;
            progressSlider.value -= (totalGarbage / 2);
            garbageDestroyed = (int)progressSlider.value;
            Debug.Log("Empty Trash was called! New slider value: " + progressSlider.value);
            OnTrashEmptied?.Invoke();
        }
        else
        {
            Debug.Log("There's no garbage for truck pick up");
        }
    }
}
