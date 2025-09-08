using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using NUnit.Framework;

public class GameManager : MonoBehaviour
{
    //Had to change something for github :3

    public PlayerInput Actions;
    public GameObject player;
    public GameObject FastEnemyPrefab;
    public GameObject NormalEnemyPrefab;
    public GameObject SlowEnemyPrefab;
    public GameObject BulletPrefab;
    public InputAction Quit;
    public InputAction Reset;
    public List<GameObject> Enemies = new List<GameObject>();

    public GameObject UI;
    public GameObject PlayingUI;
    public TMP_Text ScoreText;
    public TMP_Text HighScoreText;
    public TMP_Text LivesText;
    public TMP_Text EndScoreText;
    public TMP_Text EndHighScoreText;
    public GameObject OverUI;

    public int Score;
    public int HighScore;
    public int Lives;
    public int CurLives;

    public bool SpawnEnemy;
    public EnemySpawner SpawnerScript;

    private void Start()
    {
        Quit = Actions.currentActionMap.FindAction("Quit");
        Quit.started += Handle_QuitAction;
        Reset = Actions.currentActionMap.FindAction("Reset");
        Reset.started += Handle_ResetAction;
        CurLives = Lives;
    }

    void Update()
    {
        if (SpawnEnemy)
        {
            SpawnerScript.SpawnEnemy();
        }
    }

    private void Handle_ResetAction(InputAction.CallbackContext context)
    {
        ResetTime();
    }

    private void Handle_QuitAction(InputAction.CallbackContext context)
    {
        QuitTime();
    }

    public void IncreaseScore(int ScoreUp)
    {
        Score += ScoreUp;
        UpdateScoreUI();
    }

    public void TakeDamage()
    {
        UpdateLivesUI();
        if (CurLives > 1 && Lives > 1)
        {
            CurLives--;
        }
        else if (CurLives <= 1 && Lives >= 1)
        {
            LostGame();
        }
    }

    public void LostGame()
    {
        SpawnerScript.StopAllCoroutines();
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i] != null)
            {
                Enemies[i].GetComponent<EnemyManager>().rgbd.linearVelocity = Vector2.zero;
            }
        }
        EndHighScoreText.text = HighScoreText.text;
        EndScoreText.text = ScoreText.text;
        PlayingUI.SetActive(true);
        OverUI.SetActive(true);
    }

    public void UpdateScoreUI()
    {
        ScoreText.text = Score.ToString();
        if (Score > HighScore)
        {
            HighScore = Score;
            UpdateHighScoreUI();
        }
    }

    public void UpdateHighScoreUI()
    {
        HighScoreText.text = HighScore.ToString();
    }

    public void UpdateLivesUI()
    {
        LivesText.text = CurLives.ToString();
    }

    public void QuitTime()
    {
        SaveHighScore(HighScore);
        Application.Quit();
    }
    public void ResetTime()
    {
        SaveHighScore(HighScore);
        SceneManager.LoadScene(0);
    }

    public void SaveHighScore(int currentScore)
    {
        int previousHighScore = PlayerPrefs.GetInt("HighScore", 0);

        if (currentScore > previousHighScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();
        }
    }

}
