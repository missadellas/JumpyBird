using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;
    public static GameManager Instance;

    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;
    

    enum PageState
    {
        None,
        Start,
        Gameover,
        Countdown
    }

    int score = 0;
    bool gameOver = false;

    public bool GameOver { get { return gameOver; } }

	// Use this for initialization
    void Awake()
    {
        Instance = this;
    }
    //subscribe to events
    void OnEnable()
    {
        CountDownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerScored += OnPlayerScored;
        TapController.OnPlayerDied += OnPlayerDied;

    }
    //unsubscribe to events
    void OnDisable()
    {
        CountDownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnPlayerScored -= OnPlayerScored;
        TapController.OnPlayerDied -= OnPlayerDied;
    }
    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted(); //listened to by TapController
        score = 0;
        gameOver = false;
    }
    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();

    }
    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        SetPageState(PageState.Gameover);
    }

    void SetPageState(PageState state)
    {
        switch(state){
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);

                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.Gameover:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                break;

            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;
        }
    }
    public void ConfirmGameOver()
    {
        //activated when replay button is hit
        OnGameOverConfirmed();//listened to by tapController
        scoreText.text = "0";
        SetPageState(PageState.Start);
        
    }
    public void StartGame()
    {
        //activated when play button is hit
        SetPageState(PageState.Countdown);

    }
}
