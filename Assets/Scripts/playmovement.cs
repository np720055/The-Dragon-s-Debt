using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite[] idleSprites;
    public Sprite[] runSprites;
    public Sprite[] jumpSprites;
    public float speed = 5f;
    public float jumpForce = 7f;
    public float animationSpeed = 0.15f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector3 direction;
    private int spriteIndex;
    private string currentState = "Idle"; // Tracks current animation state
    private bool isJumping = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Set gravity scale to 0 to prevent automatic falling (we will control gravity manually)
        rb.gravityScale = 0;

        // Set player size via Transform scale
        transform.localScale = new Vector3(3.3f, 3.3f, 3.3f); // Set the player size as needed
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), animationSpeed, animationSpeed);
    }

    private void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        // Move character
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        // Flip character based on direction
        if (move != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);
        }

        // Change animation based on movement
        if (rb.velocity.y > 0.1f)
        {
            SetAnimation("Jump");
        }
        else if (move != 0)
        {
            SetAnimation("Run");
        }
        else
        {
            SetAnimation("Idle");
        }

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.gravityScale = 1; // Enable gravity when jumping
            isJumping = true;
        }

        // If the player is in the air and not jumping anymore, apply gravity effect
        if (isJumping && rb.velocity.y < 0)
        {
            rb.gravityScale = 1; // Enable gravity to make the player fall
        }
        else if (isJumping && rb.velocity.y > 0)
        {
            rb.gravityScale = 0.5f; // Apply lighter gravity while going up (optional, can be adjusted)
        }
    }

    private void SetAnimation(string newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        spriteIndex = 0;
    }

    private void AnimateSprite()
    {
        Sprite[] currentSprites = idleSprites;

        if (currentState == "Run") currentSprites = runSprites;
        if (currentState == "Jump") currentSprites = jumpSprites;

        if (currentSprites.Length > 0)
        {
            spriteIndex = (spriteIndex + 1) % currentSprites.Length;
            spriteRenderer.sprite = currentSprites[spriteIndex];
        }
    }

    private bool IsGrounded()
    {
        // Use a small raycast downward from the player's feet to check if grounded
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        return hit.collider != null;
    }
}
