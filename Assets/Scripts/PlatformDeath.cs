using System.Collections;
using UnityEngine;

public class PlatformDeath : MonoBehaviour
{
    public void Death()
    {
        StartCoroutine(DeathCoroutine());
    }

    IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        // Make the platform fall
        Rigidbody rb = transform.parent.gameObject.AddComponent<Rigidbody>();
        rb.mass = 5f; // Set mass to make it fall faster
        yield return new WaitForSeconds(1f); // Wait for another 1 second
        Destroy(transform.parent.gameObject); // Destroy the platform
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Death();
            transform.parent.parent = null; // Detach from parent to avoid affecting other platforms
        }
    }
}
