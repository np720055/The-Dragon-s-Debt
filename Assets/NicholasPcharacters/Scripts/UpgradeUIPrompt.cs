using UnityEngine;

public class UpgradeUIPrompt : MonoBehaviour
{
    public GameObject upgradePanel; // Assign in Inspector

    private void Start()
    {
        // Make sure it's hidden on start
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && upgradePanel != null)
            upgradePanel.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && upgradePanel != null)
            upgradePanel.SetActive(false);
    }
}
