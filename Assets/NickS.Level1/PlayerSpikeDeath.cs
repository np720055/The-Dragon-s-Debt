using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerDeathHandler : MonoBehaviour
{
    public Sprite[] deathSprites;           // Assign your death animation sprites in inspector
    public float animationFrameRate = 0.1f; // Time between frames
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private bool isDead = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes") && !isDead)
        {
            StartCoroutine(DieAndPlayAnimation());
        }
    }

    private IEnumerator DieAndPlayAnimation()
    {
        isDead = true;

        // Disable controls here if needed
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerPlatformStick>().enabled = false;

        // Freeze player physics and disable collider
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        playerCollider.enabled = false;

        // Play your death sprite animation coroutine
        yield return StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        for (int i = 0; i < deathSprites.Length; i++)
        {
            spriteRenderer.sprite = deathSprites[i];
            yield return new WaitForSeconds(animationFrameRate);
        }

        // Reload the current scene to reset the level and player progress
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
