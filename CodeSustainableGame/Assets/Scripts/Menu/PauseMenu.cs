using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPausedMenuOpen = false;

    public GameObject pauseMenu;

    private void Update()
    {
        
    }

    public void PauseClick() 
    {
        if (isPausedMenuOpen) 
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }


    private void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPausedMenuOpen=true;
    }

    private void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPausedMenuOpen=false;
    }


}
