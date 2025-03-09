using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField]
    private Image garbageHealthBar;

    public void UpdateHealthBar (float currentValue, float maxValue)
    {
        garbageHealthBar.fillAmount = currentValue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
