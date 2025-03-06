using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    public TMP_Text Turns;
    public TMP_Text People;
    public TMP_Text Money;

    public Image moneyBar;
    public float moneyBarAmount;

    public Image garbageBar;
    public float garbageBarAmount;

    public Image happyBar;
    public float happyBarAmount;



    void Start()
    {
        UpdateUIElements();
    }

    void Update()
    {
        UpdateUIElements();
    }

    public void UpdateUIElements()
    {
        Turns.text = "Turns: " + GameManager.Instance.currentTurn.ToString() + "/" + GameManager.Instance.maxTurn.ToString();
        People.text = "People: " + GameManager.Instance.currentPeople.ToString() + "/" + GameManager.Instance.maxPeople.ToString();
        Money.text = "Money: " + GameManager.Instance.currentMoney.ToString() + "$";
    }
}