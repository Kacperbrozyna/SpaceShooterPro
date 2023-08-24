using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _bestScoreText;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private Image _liveImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartHintText;

    private GameManager _gameManager;
    private int _currentScore = 0;
    private int _bestScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_bestScoreText != null)
        {
            _bestScore = PlayerPrefs.GetInt("BestScore", 0);
            _bestScoreText.text = "Best: " + _bestScore;
        }

        if (_gameManager == null)
        {
            Debug.Log("game manager is null");
        }
    }


    public void UpdateScore(int points) {

        _currentScore += points;
        _scoreText.text = "Score: " + _currentScore;

        if (_bestScoreText != null && _currentScore > _bestScore)
        {
            _bestScore = _currentScore;
            _bestScoreText.text = "Best: " + _bestScore;
        }
    }

    public void UpdateLives(int currentLives) {

        if (currentLives < 0)
            return;

        _liveImage.sprite = _livesSprite[currentLives];

        if (currentLives == 0)
        {
            _gameManager.GameOver();
            _gameOverText.gameObject.SetActive(true);
            _restartHintText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlickerRoutine());
            if (_bestScoreText != null)
            {
                PlayerPrefs.SetInt("BestScore", _bestScore);
            }
        }
    }

    IEnumerator GameOverFlickerRoutine()
    { 
        while(true)
        {
            _gameOverText.text = "GAME OVER"; 
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
          
        }
    }

    public void ResumeGame()
    { 
        _gameManager.UnpauseGame();
    }

    public void BackToMainMenu() { 
        _gameManager.BackToMainMenu();
    }
}
