using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    bool running = false;

    private void Start()
    {
        // Subscribe to GameStart event
        GameManager.GameStart += On_GameStartEvent;
        GameManager.GameHome += On_GameHomeEvent;
    }

    private void OnDestroy()
    {
        // Unsubscribe from GameStart event
        GameManager.GameStart -= On_GameStartEvent;
        GameManager.GameHome -= On_GameHomeEvent;
    }

    private void Update()
    {
        if (!running) return;

        transform.Translate(GameManager.speed * Time.deltaTime * (Vector3.forward + Vector3.right) / 2);
    }

    void On_GameLoseEvent(object sender, System.EventArgs e)
    {
        // Stop the camera movement
        running = false;

        // Unsubscribe from the event
        GameManager.GameLose -= On_GameLoseEvent;
    }

    void On_GameStartEvent(object sender, System.EventArgs e)
    {
        StartLevel();
    }

    void On_GameHomeEvent(object sender, System.EventArgs e)
    {
        // Initialize variables
        transform.position = 3f * Vector3.up;

        // Subscribe to GameLose event
        GameManager.GameLose += On_GameLoseEvent;
    }

    void StartLevel()
    {
        // Initialize variables
        running = true;
    }
}
