using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    bool running = false;

    Rigidbody rb;

    private void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

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
        if (!running)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                GameManager.TriggerGameStart();
            }
            return;
        }

        // Add 1 point to score every change of direction
        if (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.leftButton.wasReleasedThisFrame)
        {
            GameManager.AddScore(1);
        }

        // Move forward if left mouse button is pressed, else move right (zig-zag movement)
        if (Mouse.current.leftButton.isPressed)
        {
            transform.Translate(GameManager.speed * Time.deltaTime * Vector3.forward);
        }
        else
        {
            transform.Translate(GameManager.speed * Time.deltaTime * Vector3.right);
        }

        if (transform.position.y < -10f)
        {
            // Trigger game lose event
            GameManager.TriggerGameLose();

            // Update best score
            GameManager.UpdateBestScore();
        }
    }

    void On_GameLoseEvent(object sender, System.EventArgs e)
    {
        // Stop the player movement
        running = false;
        rb.isKinematic = true;

        // Unsubscribe from the event
        GameManager.GameLose -= On_GameLoseEvent;
    }

    void On_GameStartEvent(object sender, System.EventArgs e)
    {
        StartGame();
    }

    void On_GameHomeEvent(object sender, System.EventArgs e)
    {
        // Initialize variables
        transform.position = 3f * Vector3.up;
        transform.rotation = Quaternion.identity;
        rb.isKinematic = false;

        // Subscribe to GameLose event
        GameManager.GameLose += On_GameLoseEvent;
    }

    void StartGame()
    {
        // Initialize variables
        running = true;
    }
}
