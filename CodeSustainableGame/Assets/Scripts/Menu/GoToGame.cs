using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToGame : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("NEW Level1 1");
    }

    public void GoToLevel()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
