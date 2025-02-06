using UnityEngine;
using UnityEngine.UI;


public class AdvertisngManager : MonoBehaviour
{
    public GameObject adPanel;
    public Button yesButton;
    public Button noButton;
    private void Start()
    {
        adPanel.SetActive(false);

        yesButton.onClick.AddListener(AdAccepted);
        noButton.onClick.AddListener(AdDeclined);
    }

    public void AdAccepted()
    {
        Debug.Log("Advertisement sent!");

        GameManager.Instance.currentPeople++;

        FindObjectOfType<UpdateUI>().UpdateUIElements();

        adPanel.SetActive(false);
    }


    public void AdDeclined()
    {
        Debug.Log("Advertisement skipped.");
        adPanel.SetActive(false);
    }
}