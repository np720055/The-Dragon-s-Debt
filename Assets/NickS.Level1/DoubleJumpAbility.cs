using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpAbility : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool canDoubleJump = false;
    private bool isGrounded = false;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private int jumpCount = 0;

    public float jumpForce = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGround();

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount = 1;
            }
            else if (canDoubleJump && jumpCount < 2)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount++;
            }
        }
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded)
            jumpCount = 0;
    }

    public void EnableDoubleJump()
    {
        canDoubleJump = true;
    }
}
