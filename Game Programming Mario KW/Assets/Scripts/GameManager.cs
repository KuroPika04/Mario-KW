using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int world { get; private set; }
    public int stage { get; private set; }
    public int lives { get; private set; }
    public int coins { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        
        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        LoadLevel(1,1);
        ScoreManager.instance.UpdateLifeText(lives);
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;

        SceneManager.LoadScene($"Level{world}-{stage}");
    }

    public void NextLevel()
    {
        if(world == 1 && stage == 10)
        {
            LoadLevel(world + 1, 1);
        }
        LoadLevel(world, stage + 1);
    }

    public void ResetLevel(float delay)
    {
        coins = 0;
        ScoreManager.instance.UpdateCoinText(coins);
        CancelInvoke(nameof(ResetLevel));
        Invoke(nameof(ResetLevel), delay);
    }

    public void ResetLevel()
    {
        lives--;

        if (lives > 0)
        {
            Debug.Log("Go to Checkpoint");
            LoadLevel(world, stage);
        }
        else
        {
            ScoreManager.instance.ResetScore();
            GameOver();
        }
    }

    private void GameOver()
    {
        NewGame();
    }

    public void AddCoin()
    {
        coins++;

        if(coins == 100)
        {
            AddLife();
            coins = 0;
        }
        ScoreManager.instance.AddScore(100);
        ScoreManager.instance.UpdateCoinText(coins);
    }

    public void AddLife()
    {
        lives++;
        Debug.Log("Current Live : " + lives);
        ScoreManager.instance.AddScore(200);
        ScoreManager.instance.UpdateLifeText(lives);
    }
}
