using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _pauseMenu;

    private bool _isGameOver;
    public bool isCoopMode = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            if (isCoopMode)
                SceneManager.LoadScene("Co-op");
            else
                SceneManager.LoadScene("SinglePlayer");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name != "MainMenu")
                BackToMainMenu();
            else
                Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1f)
            {
                Time.timeScale = 0f;
                _pauseMenu.SetActive(true);
            }
            else
                UnpauseGame();
                
        }
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        _isGameOver= true;
    }
}
