using UnityEngine;
using System.Collections;

public class SkeletonAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;
    public int damage = 1;
    public int damageFrame = 1;

    public Sprite[] idleSprites;
    public Sprite[] walkSprites;
    public Sprite[] attackSprites;
    public Sprite[] hurtSprites;
    public Sprite[] deathSprites;

    public float frameRate = 0.1f;

    private Transform player;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private float lastAttackTime;
    private float lastDamageTime; // Time when the skeleton last took damage
    private float animationTimer;
    private int currentFrame;
    private bool isDead = false;
    private bool isAttacking = false;
    private bool damageDealt = false;
    private bool isCooldown = false;  // Flag to prevent immediate attacks after damage

    public int maxHealth = 50;
    public int currentHealth;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentHealth = maxHealth;
    }

    private void Awake()
    {
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogError("No Collider2D found on " + name);
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }


        if (isAttacking)
        {
            AnimateAttack();
        }
        else if (dist <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            StartAttack();
        }
        else if (dist <= detectionRange)
        {
            MoveTowardPlayer();
        }
        else
        {
            Wander();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Skeleton took damage. Current health: " + currentHealth);

        lastDamageTime = Time.time; // Track when damage was taken
        isCooldown = true;  // Activate cooldown

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(HurtRoutine());
        }
    }

    void Die()
    {
        if (isDead) return;

        Debug.Log("Skeleton died. Starting death animation.");
        isDead = true;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static; // Prevent falling
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        int frame = 0;
        Debug.Log("Playing death animation...");

        while (frame < deathSprites.Length)
        {
            sr.sprite = deathSprites[frame];
            frame++;
            yield return new WaitForSeconds(frameRate);
        }

        yield return new WaitForSeconds(0.2f); // Optional delay before disappearing
        Destroy(gameObject);
    }

    IEnumerator HurtRoutine()
    {
        int frame = 0;
        while (frame < hurtSprites.Length)
        {
            sr.sprite = hurtSprites[frame];
            frame++;
            yield return new WaitForSeconds(frameRate);
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        animationTimer = 0f;
        currentFrame = 0;
        damageDealt = false;
        lastAttackTime = Time.time;
    }

    void AnimateAttack()
    {
        animationTimer += Time.deltaTime;

        if (animationTimer >= frameRate && currentFrame < attackSprites.Length)
        {
            sr.sprite = attackSprites[currentFrame];

            if (player.position.x < transform.position.x)
                sr.flipX = true;
            else
                sr.flipX = false;

            if (!damageDealt && currentFrame == damageFrame)
            {
                float dist = Vector2.Distance(transform.position, player.position);
                if (dist <= attackRange)
                {
                    HealthSystem playerHealth = player.GetComponent<HealthSystem>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damage);
                        Debug.Log("Skeleton hit player!");
                    }
                }
                damageDealt = true;
            }

            currentFrame++;
            animationTimer = 0f;

            if (currentFrame >= attackSprites.Length)
            {
                isAttacking = false;
            }
        }
    }

    void MoveTowardPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        AnimateWalk(direction.x);
    }

    void Wander()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
        AnimateIdle();
    }

    void AnimateWalk(float directionX)
    {
        animationTimer += Time.deltaTime;
        if (walkSprites.Length == 0) return;

        if (animationTimer >= frameRate)
        {
            currentFrame = (currentFrame + 1) % walkSprites.Length;
            sr.sprite = walkSprites[currentFrame];
            animationTimer = 0f;
        }

        sr.flipX = directionX < 0;
    }

    void AnimateIdle()
    {
        animationTimer += Time.deltaTime;
        if (idleSprites.Length == 0) return;

        if (animationTimer >= frameRate)
        {
            currentFrame = (currentFrame + 1) % idleSprites.Length;
            sr.sprite = idleSprites[currentFrame];
            animationTimer = 0f;
        }
    }
}
