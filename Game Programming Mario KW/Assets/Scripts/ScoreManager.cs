using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text coin_text;
    public Text life_text;
    public Text score_text;

    private int initial_coin = 0;
    private int initial_life = 3;
    private int score = 0;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        coin_text.text = "x " + initial_coin.ToString();
        life_text.text = "x " + initial_life.ToString();
        score_text.text = score.ToString();
    }
    public void UpdateCoinText(int coins)
    {
        coin_text.text = "x " + coins.ToString();
    }
    public void UpdateLifeText(int lifes)
    {
        life_text.text = "x " + lifes.ToString();
    }
    public void AddScore(int rewards)
    {
        score += rewards;
        score_text.text = score.ToString();
    }
    public void ResetScore()
    {
        score = 0;
        score_text.text = score.ToString();
    }
}
