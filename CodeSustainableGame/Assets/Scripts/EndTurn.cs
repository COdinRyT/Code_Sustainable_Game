using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTurn : MonoBehaviour
{
    public GameObject TutorialText1;
    public GameObject TutorialText2;
    public GameObject TutorialText3;
    public void OnClick()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Tutorial")
        {
            if (TutorialText1 != null)
            {
                TutorialText1.SetActive(false);
            }
            if (TutorialText2 != null)
            {
                TutorialText2.SetActive(true);
            }
        }
        if (GameManager.Instance.tutorialGarbageWasRemoved == true)
        {
            if (TutorialText2 != null)
            {
                TutorialText2.SetActive(false);
            }

            if (TutorialText3 != null)
            {
                TutorialText3.SetActive(true);
            }
            
        }
        Shop.Instance.truckSpawns = true;

        //Debug.Log("CLicked");
        if (GameManager.Instance == null)
        {
            //Debug.LogError("GameManager.Instance is null");
            return;
        }

        GameManager.Instance.endTurn = true;
        //Debug.Log("End turn end turn");
    }
}