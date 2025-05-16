using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTrigger : MonoBehaviour
{
    public HealthSystem playerHealth;
    public PlayerAttack playerAttack;
    private StatUpgradeNPC upgradeNPC;
    private bool inRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            upgradeNPC = GetComponent<StatUpgradeNPC>();
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    private void Update()
    {
        if (!inRange || upgradeNPC == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            upgradeNPC.TryUpgradeHealth(playerHealth);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            upgradeNPC.TryUpgradeLightAttack(playerAttack);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            upgradeNPC.TryUpgradeHeavyAttack(playerAttack);
    }
}
