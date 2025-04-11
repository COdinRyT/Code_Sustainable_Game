using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTurn : MonoBehaviour
{
    public GameObject TutorialText1;
    public GameObject TutorialText2;
    public void OnClick()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Tutorial")
        {
            TutorialText1.SetActive(false);
            TutorialText2.SetActive(true);
        }
        Shop.Instance.truckSpawns = true;

        Debug.Log("CLicked");
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null");
            return;
        }

        if (GameManager.Instance.endTurn == false)
        {
            GameManager.Instance.endTurn = true;
            Debug.Log("End turn end turn");
        }
    }
}