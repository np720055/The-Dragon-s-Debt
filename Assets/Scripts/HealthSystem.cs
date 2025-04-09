using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 3;
    public Sprite[] hurtSprites;
    public Sprite[] deathSprites;
    public float animationFrameRate = 0.1f;

    public int currentHealth;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;
    private int animFrame = 0;
    private float nextAnimTime = 0f;

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log(gameObject.name + " took damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(PlayHurtAnimation());
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " died.");
        StartCoroutine(PlayDeathAnimation());
        // Optionally disable movement/AI here
        if (TryGetComponent<SkeletonAI>(out var ai)) ai.enabled = false;
        if (TryGetComponent<PlayerController>(out var pc)) pc.enabled = false;
    }

    private System.Collections.IEnumerator PlayHurtAnimation()
    {
        for (int i = 0; i < hurtSprites.Length; i++)
        {
            spriteRenderer.sprite = hurtSprites[i];
            yield return new WaitForSeconds(animationFrameRate);
        }
    }

    private System.Collections.IEnumerator PlayDeathAnimation()
    {
        for (int i = 0; i < deathSprites.Length; i++)
        {
            spriteRenderer.sprite = deathSprites[i];
            yield return new WaitForSeconds(animationFrameRate);
        }

        // Destroy after animation
        Destroy(gameObject);
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    
}
