using UnityEngine;

public class ChestTrigger2D : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collision Detected");
            animator.SetTrigger("OpenChestTrigger");
        }
    }
}
