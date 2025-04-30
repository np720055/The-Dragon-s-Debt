using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;             // Drag the UI fill image here
    public GameObject player;           // Drag your player GameObject here
    public float maxHealth = 100f;

    private float currentHealth;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        // Just for testing — remove in final game
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
