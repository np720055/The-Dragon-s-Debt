using UnityEngine;

public class CharacterAI : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;
    public int damage = 1;
    public int damageFrame = 1;

    [Header("Animations")]
    public Sprite[] idleSprites;
    public Sprite[] walkSprites;
    public Sprite[] attack1Sprites;
    public Sprite[] attack2Sprites;
    public Sprite[] attack3Sprites;
    public float frameRate = 0.1f;

    [Header("Patrol")]
    public float patrolDistance = 3f;
    public float patrolSpeed = 1.5f;
    public float patrolWaitTime = 1.5f;

    private Transform player;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private float animationTimer;
    private int currentFrame;
    private float lastAttackTime;
    private bool isDead = false;
    private bool isAttacking = false;
    private bool damageDealt = false;

    private int attackIndex = 0;
    private Sprite[] currentAttackSprites;

    // Patrol state
    private Vector2 patrolLeftEdge;
    private Vector2 patrolRightEdge;
    private int patrolDirection = 1; // 1 = right, -1 = left
    private float waitTimer = 0f;
    private bool waiting = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Setup patrol range
        patrolLeftEdge = transform.position - Vector3.right * patrolDistance;
        patrolRightEdge = transform.position + Vector3.right * patrolDistance;
    }

    void Update()
    {
        if (isDead || player == null) return;

        var health = GetComponent<HealthSystem>();
        if (health != null && health.IsDead()) return;



        float dist = Vector2.Distance(transform.position, player.position);

        if (isAttacking)
        {
            AnimateAttack(currentAttackSprites);
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
            Patrol();
        }
    }

    void MoveTowardPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        AnimateWalk(direction.x);
    }

    void Patrol()
    {
        if (waiting)
        {
            waitTimer += Time.deltaTime;
            rb.velocity = Vector2.zero;
            AnimateIdle();

            if (waitTimer >= patrolWaitTime)
            {
                waiting = false;
                patrolDirection *= -1;
                waitTimer = 0f;
            }
            return;
        }

        float nextX = transform.position.x + patrolDirection * patrolSpeed * Time.deltaTime;

        if (patrolDirection == 1 && nextX >= patrolRightEdge.x ||
            patrolDirection == -1 && nextX <= patrolLeftEdge.x)
        {
            waiting = true;
            rb.velocity = Vector2.zero;
            return;
        }

        rb.velocity = new Vector2(patrolDirection * patrolSpeed, rb.velocity.y);
        AnimateWalk(patrolDirection);
    }

    void StartAttack()
    {
        isAttacking = true;
        animationTimer = 0f;
        currentFrame = 0;
        damageDealt = false;
        lastAttackTime = Time.time;

        currentAttackSprites = GetNextAttack();
    }

    Sprite[] GetNextAttack()
    {
        Sprite[] result = attack1Sprites;

        switch (attackIndex)
        {
            case 0: result = attack1Sprites; break;
            case 1: result = attack2Sprites; break;
            case 2: result = attack3Sprites; break;
        }

        attackIndex = (attackIndex + 1) % 3;
        return result;
    }

    void AnimateAttack(Sprite[] attackSprites)
    {
        animationTimer += Time.deltaTime;
        if (attackSprites.Length == 0) return;

        if (animationTimer >= frameRate)
        {
            if (currentFrame < attackSprites.Length)
            {
                sr.sprite = attackSprites[currentFrame];
                sr.flipX = (player.position.x < transform.position.x);

                if (!damageDealt && currentFrame == damageFrame)
                {
                    float dist = Vector2.Distance(transform.position, player.position);
                    if (dist <= attackRange)
                    {
                        var playerHealth = player.GetComponent<HealthSystem>();
                        if (playerHealth != null)
                            playerHealth.TakeDamage(damage);
                    }
                    damageDealt = true;
                }

                currentFrame++;
                animationTimer = 0f;
            }
            else
            {
                isAttacking = false;
            }
        }
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

    public void OnDeath()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;

        GiveUpgradePoint();
    }

    private void GiveUpgradePoint()
    {
        GameObject npc = GameObject.FindWithTag("UpgradeNPC");
        if (npc != null)
        {
            StatUpgradeNPC upgradeNPC = npc.GetComponent<StatUpgradeNPC>();
            if (upgradeNPC != null)
            {
                upgradeNPC.AddPoints(50);
            }
        }
    }
}
