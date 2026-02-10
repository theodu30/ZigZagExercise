using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Material platformMaterial;

    GameObject container;
    public GameObject PlatformPrefab;
    public GameObject GemPrefab;

    Vector3 currentPosition;
    public int NumberOfPlatforms = 50;
    public int maxDistance = 7;
    int numberOfPlatformsGenerated = 0;
    int goingForward = 0;
    int goingRight = 1;

    bool running = false;
    bool levelGenerated = false;
    float timer = 0f;

    public List<Color> platformColors;
    int actualColorIndex = 0;
    int previousColorIndex = 0;

    public int scoreIntervalToColorSwap = 50;
    bool colorSwapTimer = false;
    public float colorSwapInterval = 2f;
    float colorSwapTimerCounter = 0f;

    private void Start()
    {
        // Subscribe to GameStart event
        GameManager.GameStart += On_GameStartEvent;
        GameManager.GameHome += On_GameHomeEvent;

        // Subscribe to ScoreChanged event to change platform color based on score
        GameManager.ScoreChanged += On_ScoreChangedEvent;
    }

    private void OnDestroy()
    {
        // Change platform color back to default
        platformMaterial.color = platformColors[0];

        // Unsubscribe from GameStart event
        GameManager.GameStart -= On_GameStartEvent;
        GameManager.GameHome -= On_GameHomeEvent;

        // Unsubscribe from ScoreChanged event
        GameManager.ScoreChanged -= On_ScoreChangedEvent;
    }

    private void Update()
    {
        if (!running) return;

        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = (3f / GameManager.speed);
                GeneratePlatform();
            }
        }

        if (colorSwapTimer)
        {
            colorSwapTimerCounter += Time.deltaTime;

            Color lerpColor = Color.Lerp(platformColors[previousColorIndex], platformColors[actualColorIndex], colorSwapTimerCounter / colorSwapInterval);
            platformMaterial.color = lerpColor;
            if (colorSwapTimerCounter >= colorSwapInterval)
            {
                previousColorIndex = actualColorIndex;
                colorSwapTimerCounter = 0f;
                colorSwapTimer = false;
            }
        }
    }

    void GenerateLevel()
    {
        if (levelGenerated) return;
        if (container != null)
        {
            Destroy(container);
        }
        container = new GameObject("LevelContainer");
        currentPosition = transform.position;
        while (numberOfPlatformsGenerated < NumberOfPlatforms)
        {
            GeneratePlatform();
            numberOfPlatformsGenerated++;
        }
        levelGenerated = true;
    }

    void GeneratePlatform()
    {
        Transform pl = Instantiate(PlatformPrefab, currentPosition, Quaternion.identity, container.transform).transform;
        bool goForward = Random.value > .5f;
        if (goForward && goingForward < maxDistance)
        {
            currentPosition += new Vector3(0, 0, 3);
            goingForward++;
            --goingRight;
        }
        else if (!goForward && goingRight < maxDistance)
        {
            currentPosition += new Vector3(3, 0, 0);
            goingRight++;
            --goingForward;
        }
        else if (goingForward >= maxDistance)
        {
            currentPosition += new Vector3(3, 0, 0);
            goingRight++;
            --goingForward;
        }
        else if (goingRight >= maxDistance)
        {
            currentPosition += new Vector3(0, 0, 3);
            goingForward++;
            --goingRight;
        }

        // Randomly place a gem on the platform with 30% chance
        if (Random.value < 0.20f)
        {
            Vector3 gemPosition = currentPosition + 3 * Vector3.up;
            Instantiate(GemPrefab, gemPosition, Quaternion.identity, pl);
        }
    }

    void On_GameLoseEvent(object sender, System.EventArgs e)
    {
        // Stop the level generation
        running = false;
        levelGenerated = false;

        // Unsubscribe from GameLose event
        GameManager.GameLose -= On_GameLoseEvent;
    }

    void On_GameStartEvent(object sender, System.EventArgs e)
    {
        StartGame();
    }

    private void On_GameHomeEvent(object sender, System.EventArgs e)
    {
        // Change platform color back to default
        platformMaterial.color = platformColors[0];

        // Initialize variables
        currentPosition = transform.position;
        numberOfPlatformsGenerated = 0;
        goingForward = 0;
        goingRight = 1;

        // Initialize timer according to ball speed (10 m/s) and platform size (3 m) and NumberOfPlatforms / 2
        timer = (3f / GameManager.speed) * NumberOfPlatforms / 2;

        // Subscribe to GameLose event
        GameManager.GameLose += On_GameLoseEvent;
    }

    void On_ScoreChangedEvent(object sender, int newScore)
    {
        // Change platform color based on score (every 10 points)
        actualColorIndex = (newScore / scoreIntervalToColorSwap) % platformColors.Count;
        if (actualColorIndex != previousColorIndex)
        {
            colorSwapTimer = true;
        }
    }

    void StartGame()
    {
        // Initialize variables
        running = true;

        // Reset platform color
        platformMaterial.color = platformColors[0];

        // Generate the level
        GenerateLevel();
    }
}
