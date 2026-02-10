using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    UIDocument uiDocument;
    VisualElement root;
    VisualElement hudRoot;
    VisualElement menuRoot;
    VisualElement gameOverRoot;
    Label score;
    Label bestScore;
    Label levelPlayed;
    Button restart;
    Button menu;
    Button start;
    Button quit;

    private void Start()
    {
        // Get UIDocument component
        uiDocument = GetComponent<UIDocument>();

        // Get UI elements
        root = uiDocument.rootVisualElement;
        hudRoot = root.Q<VisualElement>("HUD");
        menuRoot = root.Q<VisualElement>("Menu");
        gameOverRoot = root.Q<VisualElement>("GameOver");

        score = hudRoot.Q<Label>("Score");
        bestScore = hudRoot.Q<Label>("BestScore");
        levelPlayed = menuRoot.Q<Label>("LevelPlayed");

        restart = gameOverRoot.Q<Button>("Restart");
        menu = gameOverRoot.Q<Button>("Menu");

        start = menuRoot.Q<Button>("Start");
        quit = menuRoot.Q<Button>("Quit");

        // Subscribe to button click events
        restart.clicked += On_RestartButtonClicked;
        menu.clicked += On_MenuButtonClicked;
        start.clicked += On_StartButtonClicked;
        quit.clicked += On_QuitButtonClicked;

        // Subscribe to GameStart, GameLose and GameHome events
        GameManager.GameStart += On_GameStartEvent;
        GameManager.GameLose += On_GameLoseEvent;
        GameManager.GameHome += On_GameHomeEvent;

        // Subscribe to ScoreChanged and BestScoreChanged event
        GameManager.ScoreChanged += On_ScoreChangedEvent;
        GameManager.BestScoreChanged += On_BestScoreChangedEvent;

        // Subscribe to LevelPlayedChanged event
        GameManager.LevelPlayedChanged += On_LevelPlayedChangedEvent;

        // Call for update displays
        GameManager.UpdateBestScore();
        GameManager.UpdateScore();
        GameManager.UpdateLevelPlayed();

        // Initialize UI state
        hudRoot.style.display = DisplayStyle.Flex;
        gameOverRoot.style.display = DisplayStyle.None;
        menuRoot.style.display = DisplayStyle.Flex;
    }

    private void OnDestroy()
    {
        // Unsubscribe from button click events
        restart.clicked -= On_RestartButtonClicked;
        menu.clicked -= On_MenuButtonClicked;
        start.clicked -= On_StartButtonClicked;
        quit.clicked -= On_QuitButtonClicked;

        // Unsubscribe from GameStart, GameLose and GameHome events
        GameManager.GameStart -= On_GameStartEvent;
        GameManager.GameLose -= On_GameLoseEvent;
        GameManager.GameHome -= On_GameHomeEvent;

        // Unsubscribe from ScoreChanged and BestScoreChanged event
        GameManager.ScoreChanged -= On_ScoreChangedEvent;
        GameManager.BestScoreChanged -= On_BestScoreChangedEvent;
    }

    void On_ScoreChangedEvent(object sender, int newScore)
    {
        score.text = newScore.ToString();
    }

    void On_BestScoreChangedEvent(object sender, int newScore)
    {
        bestScore.text = newScore.ToString();
    }

    void On_GameLoseEvent(object sender, System.EventArgs e)
    {
        // Show Game Over UI
        menuRoot.style.display = DisplayStyle.None;
        hudRoot.style.display = DisplayStyle.Flex;
        gameOverRoot.style.display = DisplayStyle.Flex;
        bestScore.text = PlayerPrefs.GetInt("BestScore", 0).ToString();

        // Unsubscribe from the event
        GameManager.GameLose -= On_GameLoseEvent;
    }

    void On_GameStartEvent(object sender, System.EventArgs e)
    {
        // Show HUD UI
        menuRoot.style.display = DisplayStyle.None;
        gameOverRoot.style.display = DisplayStyle.None;
        hudRoot.style.display = DisplayStyle.Flex;
    }

    void On_GameHomeEvent(object sender, System.EventArgs e)
    {
        // Show Menu UI
        hudRoot.style.display = DisplayStyle.Flex;
        gameOverRoot.style.display = DisplayStyle.None;
        menuRoot.style.display = DisplayStyle.Flex;

        // Subscribe to GameLose event
        GameManager.GameLose += On_GameLoseEvent;
    }

    private void On_LevelPlayedChangedEvent(object sender, int newLevelChanged)
    {
        levelPlayed.text = newLevelChanged.ToString();
    }

    void On_RestartButtonClicked()
    {
        GameManager.IncrementLevelPlayed();
        GameManager.ResetScore();
        GameManager.TriggerGameHome();
        GameManager.TriggerGameStart();
    }

    void On_MenuButtonClicked()
    {
        GameManager.TriggerGameHome();
    }

    void On_StartButtonClicked()
    {
        GameManager.IncrementLevelPlayed();
        GameManager.ResetScore();
        GameManager.TriggerGameHome();
        GameManager.TriggerGameStart();
    }

    private void On_QuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
