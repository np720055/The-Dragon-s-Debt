using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Sprites
    public Sprite[] idleSprites;
    public Sprite[] runSprites;
    public Sprite[] jumpSprites;
    public Sprite[] attackSet1; // Light attack
    public Sprite[] attackSet2; // Heavy attack

    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private float horizontal;
    private bool isRunning;
    private bool isJumping;
    private bool isGrounded;
    private bool isAttacking;

    // Animation state
    private float timeSinceLastFrame = 0f;
    private int idleFrame = 0;
    private int runFrame = 0;
    private int jumpFrame = 0;
    private int attackFrame = 0;

    // Attack logic
    private float attackCooldown = 0.5f;
    private float attackTimer = 0f;

    private float eKeyHeldTime = 0f;
    private float holdThreshold = 0.5f;
    private bool holdTriggered = false;

    private enum AttackType { None, Light, Heavy }
    private AttackType currentAttack = AttackType.None;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
        UpdateAnimations();
    }

    void HandleInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        isGrounded = Mathf.Abs(rb.velocity.y) < 0.1f;

        if (attackTimer > 0f) attackTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.E) && !isAttacking)
        {
            eKeyHeldTime += Time.deltaTime;
            if (eKeyHeldTime >= holdThreshold && !holdTriggered)
            {
                TriggerAttack(AttackType.Heavy);
                holdTriggered = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            eKeyHeldTime = 0f;
            holdTriggered = false;
        }

        if (Input.GetKeyUp(KeyCode.E) && !isAttacking && !holdTriggered)
        {
            TriggerAttack(AttackType.Light);
        }

        if (!isAttacking)
        {
            if (horizontal != 0)
            {
                isRunning = true;
                spriteRenderer.flipX = horizontal < 0;
            }
            else
            {
                isRunning = false;
            }

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }

            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }
    }

    void TriggerAttack(AttackType type)
    {
        currentAttack = type;
        isAttacking = true;
        attackTimer = attackCooldown;
        attackFrame = 0;
        timeSinceLastFrame = 0f;
    }

    void UpdateAnimations()
    {
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
    }

    void AnimateIdle()
    {
        timeSinceLastFrame += Time.deltaTime;
        if (idleSprites.Length == 0) return;

        if (timeSinceLastFrame >= 0.1f)
        {
            idleFrame = (idleFrame + 1) % idleSprites.Length;
            spriteRenderer.sprite = idleSprites[idleFrame];
            timeSinceLastFrame = 0f;
        }
    }

    void AnimateRun()
    {
        timeSinceLastFrame += Time.deltaTime;
        if (runSprites.Length == 0) return;

        if (timeSinceLastFrame >= 0.1f)
        {
            runFrame = (runFrame + 1) % runSprites.Length;
            spriteRenderer.sprite = runSprites[runFrame];
            timeSinceLastFrame = 0f;
        }
    }

    void AnimateJump()
    {
        if (jumpSprites.Length > 0)
        {
            spriteRenderer.sprite = jumpSprites[0];
        }
    }

    void AnimateAttack()
    {
        timeSinceLastFrame += Time.deltaTime;
        Sprite[] currentSet = currentAttack == AttackType.Heavy ? attackSet2 : attackSet1;
        if (currentSet.Length == 0) return;

        if (timeSinceLastFrame >= 0.1f)
        {
            spriteRenderer.sprite = currentSet[attackFrame];
            GetComponent<PlayerAttack>()?.DealDamage(PlayerAttack.AttackType.Light); // or .Heavy

            attackFrame++;
            timeSinceLastFrame = 0f;

            if (attackFrame >= currentSet.Length)
            {
                isAttacking = false;
                currentAttack = AttackType.None;
            }
        }
    }
}
