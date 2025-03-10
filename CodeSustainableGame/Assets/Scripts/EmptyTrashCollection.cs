using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyTrashCollection : MonoBehaviour
{

    [SerializeField] private Slider progressSlider;
    [SerializeField] private int totalGarbage = 10;
    private int garbageDestroyed = 0; 
    public bool isDisposed = false;

    // Start is called before the first frame update
    [SerializeField]
    private void Start()
    {
        if (progressSlider != null)
        {
            progressSlider.maxValue = totalGarbage;
            progressSlider.value = 0; //Start at 0
        }
        
    }

    public void RegisterGarbageDestruction()
    {
        garbageDestroyed++;
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        if(progressSlider != null)
        {
            progressSlider.value = garbageDestroyed;
        }
    }

    public void EmptyTrash()
    {
        isDisposed = true;
        progressSlider.value -= totalGarbage;
    }
}
