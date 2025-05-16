using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformStick : MonoBehaviour
{
    public float moveSpeed = 5f; // Player movement speed
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Regular player movement when not on platform
        if (transform.parent == null)
        {
            float moveInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // If player touches the moving platform, parent the player to it
        if (col.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(col.transform);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        // If player leaves the platform, remove the parent
        if (col.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }
}
