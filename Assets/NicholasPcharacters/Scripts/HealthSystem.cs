using UnityEngine;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 10;
    public Sprite[] hurtSprites;
    public Sprite[] deathSprites;
    public float animationFrameRate = 0.1f;

    public int currentHealth;
    private SpriteRenderer spriteRenderer;
    private bool isDead = false;

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

        // If this is a dragon, trigger the dragon death animation
        if (gameObject.name == "Dragon")
        {
            DragonAI dragonAI = GetComponent<DragonAI>();
            if (dragonAI != null)
            {
                dragonAI.OnDeath();  // Call the DragonAI's OnDeath method
            }
        }

        // Disable player or character controller if necessary (e.g., for player)
        if (TryGetComponent<PlayerController>(out var pc)) pc.enabled = false;

        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayHurtAnimation()
    {
        for (int i = 0; i < hurtSprites.Length; i++)
        {
            spriteRenderer.sprite = hurtSprites[i];
            yield return new WaitForSeconds(animationFrameRate);
        }
    }

    private IEnumerator PlayDeathAnimation()
    {
        for (int i = 0; i < deathSprites.Length; i++)
        {
            spriteRenderer.sprite = deathSprites[i];
            yield return new WaitForSeconds(animationFrameRate);
        }

        // After the death animation, destroy the object (only for dragons)

    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
