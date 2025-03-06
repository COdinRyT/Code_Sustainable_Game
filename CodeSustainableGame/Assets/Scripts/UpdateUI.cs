using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    public TextMeshProUGUI queueText;
    private List<string> characterNames = new List<string>();

    void Awake()
    {
        //queueText = GetComponent<TextMeshProUGUI>();
        UpdateUIElements();
        Debug.Log("UpdateUI GameObject active? " + gameObject.activeInHierarchy);
       
    }

    void Update()
    {
        if (queueText != null)
        {
            UpdateUIElements();
        }
    }

    public void UpdateUIElements()
    {
        //Turns.text = "Turns: " + GameManager.Instance.currentTurn.ToString() + "/" + GameManager.Instance.maxTurn.ToString();
        //People.text = "People: " + GameManager.Instance.currentPeople.ToString() + "/" + GameManager.Instance.maxPeople.ToString();
        //Money.text = "Money: " + GameManager.Instance.currentMoney.ToString() + "$";
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
            queueText.text = "Queue:\n" + string.Join("\n", characterNames);
        }
    }
}