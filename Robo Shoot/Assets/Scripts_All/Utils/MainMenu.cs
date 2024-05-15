using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject settingsUI;
    public GameObject mainMenuUi;
    MovementStateManager movementStateManager;

    private void Start()
    {
        movementStateManager = FindObjectOfType<MovementStateManager>();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void low()
    {
        QualitySettings.SetQualityLevel(0);
    }

    public void medium()
    {
        QualitySettings.SetQualityLevel(2);
    }

    public void max()
    {
        QualitySettings.SetQualityLevel(4);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1.0f;
    }

    public void Continue()
    {
        pauseUI.SetActive(false);
        movementStateManager.isPaused = false;
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        if (settingsUI != null)
        {
            mainMenuUi.SetActive(false);
            settingsUI.SetActive(true);
        } 
    }

    public void BackButton()
    {
        if (settingsUI != null)
        {
            mainMenuUi.SetActive(true);
            settingsUI.SetActive(false);
        } 
    }
}
