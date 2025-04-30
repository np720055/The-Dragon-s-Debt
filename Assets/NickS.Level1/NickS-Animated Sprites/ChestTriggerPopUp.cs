using UnityEngine;

public class ChestTriggerPopUp: MonoBehaviour
{
    public KeyPopupController keyPopupController;
    private bool hasBeenTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasBeenTriggered && other.CompareTag("Player"))
        {
            hasBeenTriggered = true; // prevent re-triggering

            keyPopupController.ShowKeyPopup();

            // Disable the trigger so it can't be activated again
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;
        }
    }
}