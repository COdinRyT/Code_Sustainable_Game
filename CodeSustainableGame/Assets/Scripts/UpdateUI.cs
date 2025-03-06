using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    //public TMP_Text Turns;
    //public TMP_Text People;
    //public TMP_Text Money;

    public TextMeshProUGUI queueText;
    private List<string> characterNames = new List<string>();

    void Start()
    {
        queueText = GetComponent<TextMeshProUGUI>();
        UpdateUIElements();
        Debug.Log("UpdateUI GameObject active? " + gameObject.activeInHierarchy);
       
    }

    void Update()
    {
        UpdateUIElements();
    }

    public void UpdateUIElements()
    {
        //Turns.text = "Turns: " + GameManager.Instance.currentTurn.ToString() + "/" + GameManager.Instance.maxTurn.ToString();
        //People.text = "People: " + GameManager.Instance.currentPeople.ToString() + "/" + GameManager.Instance.maxPeople.ToString();
        //Money.text = "Money: " + GameManager.Instance.currentMoney.ToString() + "$";
    }

    public void UpdateQueueUI(List<GameObject> characterQueue)
    {
        if (queueText == null)
        {
            Debug.LogError("queueText is null! Make sure it's assigned in the Inspector.");
            return;
        }

        characterNames.Clear();

        foreach (GameObject character in characterQueue)
        {
            characterNames.Add(character.name);
        }

        queueText.text = "Queue:\n" + string.Join("\n", characterNames);
    }
}