using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Slider playerHealthSlider;
    public HealthSystem playerHealth;

    void Start()
    {
        if (playerHealth != null)
            playerHealthSlider.maxValue = playerHealth.maxHealth;
    }

    void Update()
    {
        if (playerHealth != null)
            playerHealthSlider.value = playerHealth.currentHealth;
    }
}
