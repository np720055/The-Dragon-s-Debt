using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    public Sprite[] idleSprites;
    public Sprite[] walkSprites;
    public Sprite[] attackSet1; // Light attack

    public float moveSpeed = 2f;
    public float attackRange = 1.5f;
    public float detectionRange = 5f;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private float lastAttackTime = -1f;
    private int currentFrame = 0;
    private bool isAnimating = false;
    private string currentAnimation = "Idle"; // Default state

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;  // Find the player by tag
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If within attack range, attack the player
        if (distanceToPlayer <= attackRange)
        {
            SetAnimationState("Attack1");  // Light attack
            AttackPlayer();
        }
        // If within detection range, move towards the player
        else if (distanceToPlayer <= detectionRange)
        {
            SetAnimationState("Walk");
            MoveTowardsPlayer();
        }
        // If outside of detection range, stay idle
        else
        {
            SetAnimationState("Idle");
        }

        // Handle sprite-based animations
        HandleAnimation();
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    private void AttackPlayer()
    {
        // Attack logic: Can add any attack effects here
        if (Time.time - lastAttackTime >= 1f)  // Prevent attacking too quickly
        {
            lastAttackTime = Time.time;
            // For now, just play the attack animation
            Debug.Log("Skeleton Attacks Player!");
        }
    }

    // Set the current animation state (Idle, Walk, Attack1, etc.)
    private void SetAnimationState(string state)
    {
        currentAnimation = state;
    }

    // Handle sprite animation switching
    private void HandleAnimation()
    {
        switch (currentAnimation)
        {
            case "Idle":
                PlayAnimation(idleSprites);
                break;
            case "Walk":
                PlayAnimation(walkSprites);
                break;
            case "Attack1":
                PlayAnimation(attackSet1);  // Light attack
                break;
        }
    }

    // Play animation by cycling through the sprites
    private void PlayAnimation(Sprite[] animationFrames)
    {
        if (animationFrames.Length == 0)
            return;

        // Switch frame every 0.1 seconds (you can tweak this for speed)
        if (!isAnimating)
        {
            isAnimating = true;
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            spriteRenderer.sprite = animationFrames[currentFrame];
            Invoke(nameof(ResetAnimationFlag), 0.1f);
        }
    }

    private void ResetAnimationFlag()
    {
        isAnimating = false;
    }
}
