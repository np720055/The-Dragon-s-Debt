using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the player moves

    private Rigidbody2D rb;       // Reference to the Rigidbody2D component
    private Vector2 moveDirection; // To store the direction of movement

    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from horizontal and vertical axes (WASD or Arrow keys)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Store the movement direction as a Vector2
        moveDirection = new Vector2(moveX, moveY).normalized; // Normalizing to ensure consistent movement speed
    }

    void FixedUpdate()
    {
        // Move the player based on input, adjusting by the movement speed
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}
