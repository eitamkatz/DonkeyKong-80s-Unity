using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI highScoreLabel;
    [SerializeField] private Setup setup;
    [SerializeField] private GameObject[] marioLives;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    private const int DIE_DELAY = 5;
    private const int DIGIT_NUM = 6;
    private const int MAX_LIVE = 3;
    public GameObject character;
    public bool inGame;
    public bool oilFire;
    public bool hammerMode;
    public bool destroyBarrels;
    private int _lives = 3;
    private int _score;
    private static int _highScore;

    #endregion

    public static GameManager Shared { get; private set; }

    private void Awake()
    {
        Shared = this;
    }

    private void Start()
    {
        oilFire = true;
        for (int i = MAX_LIVE; i > setup.lives; i--)
        {
            Destroy(marioLives[i-1]);
        }
        WriteHighScore();
    }

    private void Update()
    {
        if (_lives == MAX_LIVE)
            inGame = true;
        if (Input.GetKeyDown(KeyCode.Escape))
            OnApplicationQuit();
    }

    public void DecreaseLive()
    {
        setup.lives--;
        destroyBarrels = true;
        Destroy(marioLives[setup.lives]);
        StartCoroutine((DecreaseLiveDelay()));
    }

    private IEnumerator DecreaseLiveDelay()
    {
        yield return new WaitForSeconds(DIE_DELAY);
        inGame = false;
        if (setup.lives == 0)
        {
            setup.lives = MAX_LIVE;
            SceneManager.LoadScene("End");
        }
        else
            SceneManager.LoadScene("Open");
    }
    
    public void AddScore(int amount)
    {
        _score += amount;
        scoreLabel.text = "";
        for (int i = 0; i < DIGIT_NUM - _score.ToString().Length; i++)
        {
            scoreLabel.text += "0";
        }
        scoreLabel.text += _score.ToString();
        if (_score > setup.highScore)
        {
            setup.highScore = _score;
            UpdateHighScore();
        }
    }

    private void WriteHighScore()
    {
        for (int i = 0; i < DIGIT_NUM - setup.highScore.ToString().Length; i++)
        {
            highScoreLabel.text += "0";
        }
        highScoreLabel.text += setup.highScore.ToString();
    }

    private void UpdateHighScore()
    {
        highScoreLabel.text = scoreLabel.text;
    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}