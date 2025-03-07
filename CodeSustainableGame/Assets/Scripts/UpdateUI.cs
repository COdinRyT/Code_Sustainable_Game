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
    public TMP_Text GarbageLeft;

    //public Image moneyBar;
    //public Image garbageBar;
    //public Image happyBar;

    private int startingTurn = 0;

    public TextMeshProUGUI queueText;

    private List<string> characterNames = new List<string>();

    void Start()
    {
        GameManager.Instance.currentTurn = startingTurn;
        UpdateUIElements();        
    }

    void Update()
    {
        UpdateUIElements();
    }

    public void UpdateUIElements()
    {
        Turns.text = "Turns: " + GameManager.Instance.currentTurn.ToString() + "/" + GameManager.Instance.maxTurn.ToString();
        People.text = GameManager.Instance.currentPeople.ToString() + "/" + GameManager.Instance.maxPeople.ToString();
        Money.text = GameManager.Instance.currentMoney.ToString() + "$";
        GarbageLeft.text = "Garbage Left: " + GameManager.Instance.currentGarbageAmount.ToString();

        //moneyBar.fillAmount = GameManager.Instance.currentMoney / 9999f; // If the money amount is larger than 9999 than the bar will not fill up any more.
        //garbageBar.fillAmount = GameManager.Instance.currentGarbageAmount / 100f;
        //happyBar.fillAmount = GameManager.Instance.happiness / 100f;
    }

    public void IncreaseTurnCount()
    {
        GameManager.Instance.currentTurn++;
        UpdateUIElements();
    }

    public void UpdateQueueUI(List<GameObject> characterQueue)
    {
        characterNames.Clear();

        foreach (GameObject character in characterQueue)
        {
            characterNames.Add(character.name);
        }
        if (queueText != null)
        {
            Debug.Log("Updating");
            queueText.text = "Queue: \n" + string.Join("\n", characterNames);
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------
}