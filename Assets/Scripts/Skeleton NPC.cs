using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    public float moveSpeed = 2f;          // Speed at which the skeleton moves
    public float attackRange = 1.5f;      // Range at which the skeleton will attack the player
    public float attackCooldown = 1.5f;   // Time between consecutive attacks
    public int damage = 1;                // Amount of damage the skeleton does

    // Arrays to hold the animation frames for different actions
    public Sprite[] idleSprites;
    public Sprite[] walkSprites;
    public Sprite[] attackSprites;

    private Transform player;             // Reference to the player's transform
    private SpriteRenderer spriteRenderer;
    private float attackTimer;
    private float frameTimer;
    private int animFrame;

    private enum State { Idle, Walk, Attack }
    private State currentState = State.Idle;

    void Start()
    {
        // Find the player in the scene by tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Only proceed if the player exists
        if (player == null) return;

        // Get the distance from the player
        float distance = Vector2.Distance(transform.position, player.position);

        // If we're in attack range
        if (distance <= attackRange)
        {
            currentState = State.Attack;

            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
        else
        {
            // Change state to walk if outside attack range
            currentState = State.Walk;

            // Attempt to move the skeleton towards the player
            Vector3 target = player.position;
            Vector3 newPos = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            // Debug: Show target position
            Debug.Log("Moving towards: " + target);
            Debug.DrawLine(transform.position, newPos, Color.red, 0.1f);  // Visualize movement line in Scene view

            transform.position = newPos;  // Move the skeleton

            // Flip the sprite based on direction
            spriteRenderer.flipX = (player.position.x < transform.position.x);
        }

        // Reduce attack cooldown
        attackTimer -= Time.deltaTime;

        // Update animations
        Animate();
    }

    // Function to handle the attack logic (deals damage to the player)
    void Attack()
    {
        Debug.Log("Skeleton attacks!");

        // Assuming the player has a PlayerManager script with a TakeDamage method
        PlayerManager pm = player.GetComponent<PlayerManager>();
        if (pm != null)
        {
            pm.TakeDamage(damage);
        }
    }

    // Function to animate the skeleton based on the current state
    void Animate()
    {
        frameTimer += Time.deltaTime;

        // Change the frame every 0.1 seconds
        if (frameTimer >= 0.1f)
        {
            frameTimer = 0f;
            animFrame++;

            // Get the current animation frames based on the state
            Sprite[] currentSet = GetCurrentAnimation();

            if (currentSet.Length > 0)
            {
                animFrame %= currentSet.Length;
                spriteRenderer.sprite = currentSet[animFrame];
            }
        }
    }

    // Function to return the appropriate animation frames based on the current state
    Sprite[] GetCurrentAnimation()
    {
        switch (currentState)
        {
            case State.Walk:
                return walkSprites;
            case State.Attack:
                return attackSprites;
            default:
                return idleSprites;
        }
    }
}
