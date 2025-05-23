using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    public enum BossState { Idle, Fly, Landing, Walk, Attack, Death }
    private BossState currentState = BossState.Idle;

    [Header("Stats")]
    public float moveSpeed = 2f;
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Player Reference")]
    public Transform player;

    [Header("Animations")]
    public Sprite[] idleSprites, flySprites, landingSprites, walkSprites,
                    sliceSprites, tailSprites, thrustSprites,
                    firePrepSprites, fireShootSprites, verticalAttackSprites, deathSprites;
    public float frameRate = 0.1f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float animationTimer;
    private int frameIndex;
    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(BossBehaviorLoop());
    }

    private void Update()
    {
        if (isDead) return;
        sr.flipX = (player.position.x < transform.position.x);
    }

    IEnumerator BossBehaviorLoop()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(1f);

            int action = Random.Range(0, 5);

            switch (action)
            {
                case 0: yield return StartCoroutine(DoSliceTailCombo()); break;
                case 1: yield return StartCoroutine(DoThrustAttack()); break;
                case 2: yield return StartCoroutine(DoFireBreath()); break;
                case 3: yield return StartCoroutine(DoVerticalAttack()); break;
                case 4: yield return StartCoroutine(WalkTowardPlayer(2f)); break;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator WalkTowardPlayer(float duration)
    {
        currentState = BossState.Walk;
        frameIndex = 0;
        float timer = 0f;

        while (timer < duration)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
            Animate(walkSprites);
            timer += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
    }

    IEnumerator DoSliceTailCombo()
    {
        currentState = BossState.Attack;
        yield return AnimateAttack(sliceSprites);
        yield return AnimateAttack(tailSprites);
    }

    IEnumerator DoThrustAttack()
    {
        currentState = BossState.Attack;
        yield return AnimateAttack(thrustSprites);
    }

    IEnumerator DoFireBreath()
    {
        currentState = BossState.Attack;
        yield return AnimateAttack(firePrepSprites);
        yield return AnimateAttack(fireShootSprites);
    }

    IEnumerator DoVerticalAttack()
    {
        currentState = BossState.Attack;
        yield return AnimateAttack(verticalAttackSprites);
    }

    IEnumerator AnimateAttack(Sprite[] attackSprites)
    {
        frameIndex = 0;
        animationTimer = 0f;

        while (frameIndex < attackSprites.Length)
        {
            sr.sprite = attackSprites[frameIndex];
            animationTimer += Time.deltaTime;

            if (animationTimer >= frameRate)
            {
                frameIndex++;
                animationTimer = 0f;
            }

            yield return null;
        }
    }

    void Animate(Sprite[] sprites)
    {
        if (sprites.Length == 0) return;

        animationTimer += Time.deltaTime;
        if (animationTimer >= frameRate)
        {
            frameIndex = (frameIndex + 1) % sprites.Length;
            sr.sprite = sprites[frameIndex];
            animationTimer = 0f;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        isDead = true;
        currentState = BossState.Death;
        rb.velocity = Vector2.zero;
        frameIndex = 0;

        foreach (var sprite in deathSprites)
        {
            sr.sprite = sprite;
            yield return new WaitForSeconds(frameRate);
        }

        // Optional: trigger event or remove object
        Destroy(gameObject);
    }
}
