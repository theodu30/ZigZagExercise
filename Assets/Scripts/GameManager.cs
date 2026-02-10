using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float speed = 12f;
    public static event EventHandler GameLose;
    public static event EventHandler GameStart;
    public static event EventHandler GameHome;

    private static int score = 0;
    private static int bestScore = 0;
    public static event EventHandler<int> ScoreChanged;
    public static event EventHandler<int> BestScoreChanged;

    private static int levelPlayed = 0;
    public static event EventHandler<int> LevelPlayedChanged;

    private void Awake()
    {
        // Initialize variables
        score = 0;

        // Trigger GameHome event at the start
        TriggerGameHome();

        // Search for best score in PlayerPrefs
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        // Search for levels played in PlayerPrefs
        levelPlayed = PlayerPrefs.GetInt("LevelPlayed", 0);
    }

    public static void TriggerGameLose()
    {
        GameLose?.Invoke(null, EventArgs.Empty);
    }

    public static void TriggerGameStart()
    {
        score = 0;
        GameStart?.Invoke(null, EventArgs.Empty);
    }

    public static void TriggerGameHome()
    {
        GameHome?.Invoke(null, EventArgs.Empty);
    }

    public static void AddScore(int points)
    {
        score += points;
        ScoreChanged?.Invoke(null, score);
    }

    public static void UpdateBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
        }
        PlayerPrefs.SetInt("BestScore", bestScore);
        BestScoreChanged?.Invoke(null, bestScore);
    }

    public static void UpdateScore()
    {
        ScoreChanged?.Invoke(null, score);
    }

    public static void ResetScore()
    {
        score = 0;
        ScoreChanged?.Invoke(null, score);
    }

    public static void IncrementLevelPlayed()
    {
        levelPlayed++;
        PlayerPrefs.SetInt("LevelPlayed", levelPlayed);
        LevelPlayedChanged?.Invoke(null, levelPlayed);
    }

    public static void UpdateLevelPlayed()
    {
        LevelPlayedChanged?.Invoke(null, levelPlayed);
    }
}
