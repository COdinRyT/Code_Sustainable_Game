using UnityEngine;
using UnityEngine.UI;


public class AdvertisngManager : MonoBehaviour
{
    public GameObject adPanel;
    public Button yesButton;
    public Button noButton;
    public int awarenessIncrease = 1;

    public GameObject[] volunteers;
    private void Start()
    {
        adPanel.SetActive(false);

        yesButton.onClick.AddListener(AdAccepted);
        //noButton.onClick.AddListener(AdDeclined);
    }

    public void AdAccepted()
    {
        Debug.Log("Advertisement sent!");

        //GameManager.Instance.currentPeople++;
        GameManager.Instance.SpreadAwareness(awarenessIncrease);
        GameManager.Instance.GetInvolvedIsTrue();
        FindObjectOfType<UpdateUI>().UpdateUIElements();

        adPanel.SetActive(false);
    }


    public void AdDeclined()
    {
        Debug.Log("Advertisement skipped.");
        adPanel.SetActive(false);
    }

    public void SpawnVolunteers()
    {
        //Spawn volunteer
        int volunteerAmount = volunteers.Length;
        for(int i = 0; i < volunteerAmount; i++)
        {
            Instantiate(volunteers[i], new Vector3(1,0,1), Quaternion.identity);
        }
    }
}