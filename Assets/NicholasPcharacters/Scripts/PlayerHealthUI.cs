using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public HealthSystem playerHealth;
    public Image healthBarFill;

    void Update()
    {
        if (playerHealth != null && healthBarFill != null)
        {
            float fillAmount = (float)playerHealth.GetCurrentHealth() / playerHealth.maxHealth;
            healthBarFill.fillAmount = fillAmount;
            Debug.Log("Health: " + playerHealth.GetCurrentHealth());

        }
    }
}
