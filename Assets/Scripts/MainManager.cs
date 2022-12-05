using System;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text HighScoreText;
    public GameObject NewHighScoreText;


    private bool m_Started;
    private int m_Points;
    private bool m_GameOver;

    private void Start()
    {
        InitBricks();
        var gameData = GameDataManager.Instance.GameData;
        UpdateHighScoreText(gameData.userName, gameData.score);
    }

    private void InitBricks()
    {
        const float step = 0.6f;
        var perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = { 1, 1, 2, 2, 5, 5 };
        for (var i = 0; i < LineCount; ++i)
        {
            for (var x = 0; x < perLine; ++x)
            {
                var position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                var randomDirection = Random.Range(-1.0f, 1.0f);
                var forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        if (m_Points > GameDataManager.Instance.GameData.score)
        {
            NewHighScoreText.SetActive(true);
        }
        else
        {
            GameOverText.SetActive(true);
        }
    }

    public void SaveHighScore(String userName)
    {
        GameDataManager.Instance.GameData.userName = userName;
        GameDataManager.Instance.GameData.score = m_Points;
        NewHighScoreText.SetActive(false);
        UpdateHighScoreText(userName, m_Points);
        // save new highscore to file
        GameDataManager.Instance.SaveGameData();
    }

    public void UpdateHighScoreText(String userName, int score)
    {
        HighScoreText.text = $"Best Score : {userName} : {score}";
    }
}