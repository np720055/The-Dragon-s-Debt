using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player movement and sprite control
    public Sprite[] idleSprites; // Array for idle animation sprites
    public Sprite[] runSprites;  // Array for run animation sprites
    public Sprite[] jumpSprites; // Array for jump animation sprites
    public Sprite[] attackSet1; // Array for attack set 1 animation sprites

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isJumping;
    private bool isRunning;
    private bool isGrounded;
    private bool isAttacking;

    private float horizontal;
    private float timeSinceLastFrame = 0f;
    private int idleFrame = 0;
    private int runFrame = 0;
    private int jumpFrame = 0;
    private int attackFrame = 0;

    private float attackCooldown = 0.5f; // Time between attacks (in seconds)
    private float attackTimer = 0f; // Timer to track cooldown

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        // Update attack cooldown timer
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        // Check if grounded (player is touching the ground)
        isGrounded = Mathf.Abs(rb.velocity.y) < 0.1f;

        // Handle movement and jumping
        if (isGrounded)
        {
            if (horizontal != 0)
            {
                isRunning = true;
                // Flip sprite based on direction
                if (horizontal < 0)
                {
                    spriteRenderer.flipX = true;  // Flip sprite when moving left
                }
                else
                {
                    spriteRenderer.flipX = false; // Keep sprite facing right when moving right
                }
            }
            else
            {
                isRunning = false;
            }

            // Jumping
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Apply jump force
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }
        }

        // Attack logic (press 'E' key)
        if (Input.GetKeyDown(KeyCode.E) && attackTimer <= 0f) // Press 'E' key and cooldown check
        {
            isAttacking = true;
            attackTimer = attackCooldown; // Reset attack cooldown
            Debug.Log("Attack triggered"); // Debugging log for attack trigger
        }
        else
        {
            isAttacking = false;
        }

        // Update sprite based on state
        if (isAttacking)
        {
            AnimateAttack();
        }
        else if (isJumping)
        {
            AnimateJump();
        }
        else if (isRunning)
        {
            AnimateRun();
        }
        else
        {
            AnimateIdle();
        }

        // Apply horizontal movement
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    void AnimateIdle()
    {
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= 0.1f) // Change sprite every 0.1 seconds
        {
            idleFrame = (idleFrame + 1) % idleSprites.Length;
            spriteRenderer.sprite = idleSprites[idleFrame];
            timeSinceLastFrame = 0f;
        }
    }

    void AnimateRun()
    {
        timeSinceLastFrame += Time.deltaTime;
        if (timeSinceLastFrame >= 0.1f) // Change sprite every 0.1 seconds
        {
            runFrame = (runFrame + 1) % runSprites.Length;
            spriteRenderer.sprite = runSprites[runFrame];
            timeSinceLastFrame = 0f;
        }
    }

    void AnimateJump()
    {
        // You can add logic here for jumping animation if needed.
        // This example assumes you only display the jump sprite.
        spriteRenderer.sprite = jumpSprites[0];
    }

    void AnimateAttack()
    {
        // Log to check if we enter this function
        Debug.Log("Animating attack");

        timeSinceLastFrame += Time.deltaTime;

        // Slowing down the animation to 0.2 seconds per frame
        if (timeSinceLastFrame >= 0.2f)
        {
            if (attackSet1.Length > 0)
            {
                attackFrame = (attackFrame + 1) % attackSet1.Length; // Cycle through attackSet1 sprites
                spriteRenderer.sprite = attackSet1[attackFrame];

                // Log to check which sprite is being used
                Debug.Log("Current Attack Sprite: " + attackSet1[attackFrame].name);
            }

            timeSinceLastFrame = 0f;
        }
    }

    void Attack()
    {
        // This function can be used to handle attack-specific logic, 
        // like dealing damage, playing sounds, etc.
        // Animation will be handled by AnimateAttack().
    }
}
