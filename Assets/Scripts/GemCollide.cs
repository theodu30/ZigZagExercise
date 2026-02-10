using UnityEngine;

public class GemCollide : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.AddScore(2);
            Destroy(gameObject);
        }
    }
}
