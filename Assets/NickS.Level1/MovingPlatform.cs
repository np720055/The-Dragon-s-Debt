using UnityEngine;

public class MovingPlatformWithTriggers : MonoBehaviour
{
    public float speed = 3f;          // Speed at which the platform moves
    public Transform pointA;          // Position A
    public Transform pointB;          // Position B
    private Vector3 targetPosition;   // The current target position
    private bool movingToB = true;    // Flag to determine the current direction

    private Rigidbody2D rb;           // Reference to the Rigidbody2D component

    void Start()
    {
        // Get the Rigidbody2D component (if applicable)
        rb = GetComponent<Rigidbody2D>();

        // Ensure the Rigidbody2D is set to Kinematic (it won't be affected by forces)
        rb.bodyType = RigidbodyType2D.Kinematic;

        // Initially set the target position to pointB or pointA
        targetPosition = pointB.position; // Start moving to pointB
    }

    void FixedUpdate()
    {
        // Smoothly move the platform along the X-axis while maintaining the fixed Y position
        Vector3 fixedPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        rb.MovePosition(Vector3.MoveTowards(transform.position, fixedPosition, speed * Time.deltaTime));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Log when colliding with PointA or PointB
        if (other.CompareTag("PointA") && !movingToB)
        {
            Debug.Log("Collided with PointA, moving to PointB.");
            movingToB = true; // Switch direction to move to pointB
            targetPosition = pointB.position; // Set target to pointB
        }
        else if (other.CompareTag("PointB") && movingToB)
        {
            Debug.Log("Collided with PointB, moving to PointA.");
            movingToB = false; // Switch direction to move to pointA
            targetPosition = pointA.position; // Set target to pointA
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Optional: Log when the platform stays within the trigger collider
        if (other.CompareTag("PointA") || other.CompareTag("PointB"))
        {
            Debug.Log("Platform is inside the trigger: " + other.gameObject.name);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Optional: Log when the platform exits a trigger
        if (other.CompareTag("PointA") || other.CompareTag("PointB"))
        {
            Debug.Log("Platform exited the trigger: " + other.gameObject.name);
        }
    }
}
