using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private string[] tutorialDetails;
    public TextMeshProUGUI tutorialText;
    public GameObject panel;

    public TMP_InputField nameInput;
    private string playerName; //Store player's name

    public enum Tutorial
    {
        start,
        icon, 
        turnCounter, 
        sponsorMoney,
        helpTab, 
        menuTab, 
        money, 
        garbage, 
        happiness, 
        nextTurnButton, 
        oceanProbeLogo, 
        marketPlace, 
        vehicles,
        restore,
        oceanProbeWebsite,
        selling,
        setIdealWorker,
        finishingTrial,
    };

    public Tutorial currentDetail;

    // Start is called before the first frame update
    void Start()
    {
        tutorialDetails = new string[]
        {
            "Thank you for joining us on this incredible journey in cleaning up our planet. We really think your company is heavily in line with Ocean Probe and is looking forward to our partnership plus hope to see more in the future. With that being said, we are willing to provide/sponsor you with our ads once we believe you have made enough progress and have given you a good foundation of money which will help you hit the ground running. Just note that our contract will expire in 50 turns, so make sure you achieve the goal of bringing life back to this land and the overall viewpoint of people that they can start enjoying this area again. ",
            "This icon at the top shows how many volunteers are working on a task, and which ones have yet to be assigned a task.",
            "The turn counter shows you what turn you are on and once you hit turn 50 it will be game over and you will have the option to restart if you want to try again.",
            "This project is going to require you to manage a lot of funds, and this is where your money will go for easy access of knowing what amount of money you have. ",
            "By clicking on this function, you will be able to review any of these tutorial panels at a later point in time. ",
            "If you want to quit the game at any point and go back to the main menu screen or adjust the volume levels, you can do that here. ",
            "You are given a set amount of money from donations, and with this money have the choice of what to spend it on in the shop for your goal of cleaning up the environment. ",
            "Your garbage meter is a set number you need to achieve to get more people willing to become volunteers and more eyes on your project to get more donations. ",
            "Happiness is how the total community feels about the landscape, with 100% happiness being how you win. Happiness is achieved by cleaning trash, planting trees and volunteering. ",
            "This is how you progress once there are no more actions left to take. Just make sure there are volunteers all doing their job, because the last thing you need is a lack of production from missing a couple of days. ",
            "Once you click on the Ocean Probe symbol it will show a dropdown menu of all the main interactable functions for this game. If you click on it once more, the drop-down menu will go back to being invisible. ",
            "The market has two set things in the shop you can acquire when you click on it, with a menu system at the bottom that opens ups. \r\n\r\nNote that there are 2 small circle buttons in the corner. The x symbol is to close out of the shop when you are finished with it and the _ symbol is to minimize the store front, but you can still see the names as they will be on the bottom of the screen which will open back up if you click on the shop or the shop menu button. ",
            "Vehicles \r\n\r\nThe vehicles tab provides 3 trucks with different prices and 2 main differences between them are: 1 - The cheapest truck does not have a large capacity for holding as much trash compared to the most expensive vehicles. 2 - The cheaper trucks will take more turns for traveling than compared to the expensive trucks.",
            "Restore \r\n\r\nThis shop tab is for bringing life back by planting trees in the environment and by doing this action it’s the most effective way to gain happiness. Note that this resource is scarce, so it only shows up when you attract more volunteers to join. ",
            "Ocean Probe Website link \r\n\r\nFor more information on Ocean Probe and their set value of what they are trying to accomplish, click on this button and you will simply be taken to their website to learn more about it. ",
            "Selling  \r\n\r\nBy clicking on this option, you can sell trucks for 70 % of their value. By Selling a selected truck, it will ask you to confirm. You will then immediately go back to the standard gameplay setting following this action. ",
            "Set ideal worker button \r\n\r\nWhen a worker is a new hire or does not have a set task, you can select this button which will locate the volunteer on the map and at this point you will have the character activated to do whatever task you have chosen to do with them next. \r\n\r\nIf you were to click on this button again once this action has already had a volunteer selected, it will randomly start to work on a random trash pile which may or may not be close to them. (Note this might not be best for optimization.) ",
            "Finishing the trial run \r\n\r\nAwesome! You completed it already, so you sure know what you're doing .... Tell you what, let's get you started with a messier stage to play on now and see how well you fair with it! ",

        };

        //panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentDetail)
        {
            case Tutorial.start:
                tutorialText.text = tutorialDetails[0]; break;
            case Tutorial.icon:
                tutorialText.text = tutorialDetails[1];
                break;
            case Tutorial.turnCounter:
                tutorialText.text = tutorialDetails[2];
                break;
            case Tutorial.sponsorMoney:
                tutorialText.text = tutorialDetails[3];
                break;
            case Tutorial.helpTab:
                tutorialText.text = tutorialDetails[4];
                break;
            case Tutorial.menuTab:
                tutorialText.text = tutorialDetails[5];
                break;
            case Tutorial.money:
                tutorialText.text = tutorialDetails[6];
                break;
            case Tutorial.garbage:
                tutorialText.text = tutorialDetails[7];
                break;
            case Tutorial.happiness:
                tutorialText.text = tutorialDetails[8];
                break;
            case Tutorial.nextTurnButton:
                tutorialText.text = tutorialDetails[9];
                break;
            case Tutorial.oceanProbeLogo:
                tutorialText.text = tutorialDetails[10];
                break;
            case Tutorial.marketPlace:
                tutorialText.text = tutorialDetails[11];
                break;
            case Tutorial.vehicles:
                tutorialText.text = tutorialDetails[12];
                break;
            case Tutorial.restore:
                tutorialText.text = tutorialDetails[13];
                break;
            case Tutorial.oceanProbeWebsite:
                tutorialText.text = tutorialDetails[14];
                break;
            case Tutorial.selling:
                tutorialText.text = tutorialDetails[15];
                break;
            case Tutorial.setIdealWorker:
                tutorialText.text = tutorialDetails[16];
                break;
            case Tutorial.finishingTrial:
                tutorialText.text = tutorialDetails[17];
                break;
        }


    }

    public void StartingTutorial()
    {
        panel.SetActive(false);
    }

    public void PanelActivates(string tutorial)
    {
        GameObject foundPanel = GameObject.Find(tutorial);

        if (foundPanel != null)
        {
            foundPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Panel with name '" + tutorial + "' not found.");
        }
    }

    public string SetPlayerName()
    {
        playerName = nameInput.text;
        return playerName;
    }

    public void ActualLevel()
    {
        SceneManager.LoadScene("The Actual Level 2");
    }
}
