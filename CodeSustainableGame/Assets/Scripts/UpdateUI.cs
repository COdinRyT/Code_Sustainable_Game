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
    public Image garbageBar;
    public Image happyBar;

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
        Turns.text = GameManager.Instance.currentTurn.ToString() + "/" + GameManager.Instance.maxTurn.ToString();
        People.text = GameManager.Instance.currentPeople.ToString() + "/" + GameManager.Instance.maxPeople.ToString();
        Money.text = GameManager.Instance.currentMoney.ToString() + "$";

        moneyBar.fillAmount = GameManager.Instance.currentMoney / 9999f; // If the money amount is larger than 9999 than the bar will not fill up any more.
        garbageBar.fillAmount = GameManager.Instance.currentGarbageAmount / 100f;
        happyBar.fillAmount = GameManager.Instance.happiness / 100f;
    }
}