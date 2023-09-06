using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu, pauseMenu, optionsMenu, endMenu, winMenu;
    public static bool isPaused = false;

    public void Update()
    {
        if (Input.GetButtonDown("Pause") && pauseMenu != null)
        {
            pauseUnpause();
        }
    }

    public void pauseUnpause()
    {
        if (!pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenu.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
    }

    public void endGame()
    {
        endMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Win()
    {
        winMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1f;
    }
    public void LoadLevel3()
    {
        SceneManager.LoadScene(3);
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    
}
