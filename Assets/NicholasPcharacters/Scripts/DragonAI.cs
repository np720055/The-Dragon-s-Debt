using UnityEngine;
using System.Collections;

public class DragonAI : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1.5f;
    public int damage = 1;
    public int damageFrame = 1;

    [Header("Animation Settings")]
    public float frameRate = 0.1f;

    private Sprite[] idleSprites;
    private Sprite[] walkSprites;
    private Sprite[] attack1Sprites;
    private Sprite[] attack2Sprites;
    private Sprite[] deathSprites;

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

    private bool patrolRight = true;
    private float patrolTimer = 0f;
    private float patrolDirectionChangeTime = 3f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        idleSprites = Resources.LoadAll<Sprite>("Dragon/Idle");
        walkSprites = Resources.LoadAll<Sprite>("Dragon/Walking");
        attack1Sprites = Resources.LoadAll<Sprite>("Dragon/Attack 1");
        attack2Sprites = Resources.LoadAll<Sprite>("Dragon/Attack 2");
        deathSprites = Resources.LoadAll<Sprite>("Dragon/Death");

        Debug.Log("Idle loaded: " + idleSprites.Length);
        Debug.Log("Walk loaded: " + walkSprites.Length);
        Debug.Log("Attack1 loaded: " + attack1Sprites.Length);
        Debug.Log("Attack2 loaded: " + attack2Sprites.Length);
        Debug.Log("Death loaded: " + deathSprites.Length);
    }

    void Update()
    {
        if (isDead || player == null) return;

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
        AnimateWalk(rb.velocity.x);
    }

    void Patrol()
    {
        patrolTimer += Time.deltaTime;

        float dirX = patrolRight ? 1f : -1f;
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        AnimateWalk(rb.velocity.x);

        if (patrolTimer >= patrolDirectionChangeTime)
        {
            patrolRight = !patrolRight;
            patrolTimer = 0f;
        }
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
        Sprite[] result;
        if (attackIndex == 0)
        {
            result = attack1Sprites;
        }
        else
        {
            result = attack2Sprites;
        }

        attackIndex = (attackIndex + 1) % 2;
        return result;
    }

    void AnimateAttack(Sprite[] attackSprites)
    {
        if (attackSprites.Length == 0) return;

        animationTimer += Time.deltaTime;
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

    void AnimateWalk(float velocityX)
    {
        if (walkSprites.Length == 0) return;

        animationTimer += Time.deltaTime;
        if (animationTimer >= frameRate)
        {
            currentFrame = (currentFrame + 1) % walkSprites.Length;
            sr.sprite = walkSprites[currentFrame];
            animationTimer = 0f;
        }

        if (Mathf.Abs(velocityX) > 0.01f)
        {
            sr.flipX = velocityX > 0f;

        }
    }

    public void OnDeath()
    {
        if (isDead) return;

        isDead = true;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        currentFrame = 0;
        animationTimer = 0f;

        while (currentFrame < deathSprites.Length)
        {
            sr.sprite = deathSprites[currentFrame];
            sr.flipX = (player.position.x < transform.position.x);
            currentFrame++;
            yield return new WaitForSeconds(frameRate);
        }

        GiveUpgradePoint();
        Destroy(gameObject);
    }

    private void GiveUpgradePoint()
    {
        GameObject npc = GameObject.FindWithTag("UpgradeNPC");
        if (npc != null)
        {
            StatUpgradeNPC upgradeNPC = npc.GetComponent<StatUpgradeNPC>();
            if (upgradeNPC != null)
            {
                upgradeNPC.AddPoints(30);
            }
        }
    }
}
