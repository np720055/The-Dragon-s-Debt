using UnityEngine;

public class StatUpgradeNPC : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;
    private GameObject[] spriteObjects;




    public int playerPoints = 0;
    public int healthUpgradeCost = 5;
    public int lightAttackUpgradeCost = 5;
    public int heavyAttackUpgradeCost = 8;
    public int healthIncreaseAmount = 5;
    public int attackIncreaseAmount = 3;

    public void AddPoints(int amount)
    {
        playerPoints += amount;
        Debug.Log("Points added! Total: " + playerPoints);
    }

    public bool TryUpgradeHealth(HealthSystem playerHealth)
    {
        if (playerPoints >= healthUpgradeCost)
        {
            playerHealth.maxHealth += healthIncreaseAmount;
            playerHealth.currentHealth = playerHealth.maxHealth;
            playerPoints -= healthUpgradeCost;
            Debug.Log("Upgraded Health "+ playerHealth.maxHealth + " Total Points : " + playerPoints);

            return true;
        }
        return false;
    }

    public bool TryUpgradeLightAttack(PlayerAttack playerAttack)
    {
        if (playerPoints >= lightAttackUpgradeCost)
        {
            playerAttack.lightAttackDamage += attackIncreaseAmount;
            playerPoints -= lightAttackUpgradeCost;
            Debug.Log("Upgraded Light Attack " + playerAttack.lightAttackDamage + " Total Points: " + playerPoints);

            return true;
        }
        return false;
    }

    public bool TryUpgradeHeavyAttack(PlayerAttack playerAttack)
    {
        if (playerPoints >= heavyAttackUpgradeCost)
        {
            playerAttack.heavyAttackDamage += attackIncreaseAmount;
            playerPoints -= heavyAttackUpgradeCost;
            Debug.Log("Upgraded Light Attack " + playerAttack.heavyAttackDamage + " Total Points: " + playerPoints);

            return true;
        }
        return false;
    }
}
