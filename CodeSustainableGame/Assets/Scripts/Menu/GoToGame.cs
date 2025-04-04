using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToGame : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Demo_Level");
    }

    public void GoToLevel()
    {
        SceneManager.LoadScene("NEW Level1 1");
    }
}
