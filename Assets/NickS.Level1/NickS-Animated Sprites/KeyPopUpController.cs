using UnityEngine;
using System.Collections;

public class KeyPopupController : MonoBehaviour
{
    public GameObject keyPopup;                 // The popup UI panel
    public MonoBehaviour playerMovementScript;  // Reference to the player's movement script

    void Start()
    {
        keyPopup.SetActive(false);
    }

    public void ShowKeyPopup()
    {
        keyPopup.SetActive(true);

        // Disable player movement
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        // Automatically hide the popup after 3 seconds (optional)
        StartCoroutine(HidePopupAfterDelay(3f));
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideKeyPopup();
    }

    public void HideKeyPopup()
    {
        keyPopup.SetActive(false);

        // Re-enable player movement
        if (playerMovementScript != null)
            playerMovementScript.enabled = true;
    }
}
