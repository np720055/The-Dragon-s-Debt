using UnityEngine;

public class TreasureChestAnimation : MonoBehaviour
{
    private Animator animator;  // Reference to Animator

    // Name of the trigger for the animation
    public string animationTrigger = "OpenChestTrigger";

    private void Start()
    {
        // Get the Animator component attached to the chest GameObject
        animator = GetComponent<Animator>();
    }

    // This is called when the player enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger zone is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Trigger the animation to open the chest
            animator.SetTrigger(animationTrigger);
        }
    }

    // Optional: You can handle other logic when the player leaves the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Optionally, trigger a "close" animation (or reset to idle)
            animator.SetTrigger("CloseChest");
        }
    }
}
