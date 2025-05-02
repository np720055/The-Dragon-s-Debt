using UnityEngine;

public class TiledBackground : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        TileBackground();
    }

    void TileBackground()
    {
        // Get the camera's orthographic size (half the screen height)
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;

        // Get the sprite's dimensions
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;

        // Scale the background to cover the screen width and height
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Ceil(screenWidth / spriteWidth);
        newScale.y = Mathf.Ceil(screenHeight / spriteHeight);
        transform.localScale = newScale;
    }
}
