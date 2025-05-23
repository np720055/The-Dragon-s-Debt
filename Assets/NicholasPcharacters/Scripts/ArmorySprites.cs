
using UnityEngine;

public class NPCSpriteAnimator : MonoBehaviour
{
    public Sprite[] sprites;        // Add all 7 blacksmith sprites here in order
    public float frameRate = 6f;    // How fast the animation plays

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sprites.Length > 0)
            spriteRenderer.sprite = sprites[0];
    }

    void Update()
    {
        AnimateSprite();
    }

    void AnimateSprite()
    {
        if (sprites.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            currentFrame = (currentFrame + 1) % sprites.Length;
            spriteRenderer.sprite = sprites[currentFrame];
            timer = 0f;
        }
    }
}
