using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepositTrash : MonoBehaviour
{
    [SerializeField] private float progressAmount = 10;
    public Slider depositSlider;
    private Canvas canvas;
    TruckAI truckAI;

    // Start is called before the first frame update
    void Start()
    {
        depositSlider.maxValue = progressAmount;
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false;
        truckAI = FindAnyObjectByType<TruckAI>();
    }

    private void Update()
    {
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        if (truckAI.withinRange)
        {
            canvas.enabled = true;
            float increaseValue = 1;
            if (depositSlider.value! >= depositSlider.maxValue)
            {
                depositSlider.value += increaseValue * Time.deltaTime;
                Debug.Log($"Progress: {depositSlider.value}");
            }
        }
              
    }
}
