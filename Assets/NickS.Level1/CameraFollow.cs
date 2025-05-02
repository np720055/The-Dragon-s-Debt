using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;      // Reference to the player's transform
    public Vector3 offset;       // Offset distance between the camera and the player
    public float smoothSpeed = 0.125f; // Smooth factor to make the camera movement smoother

    void Start()
    {
        // If no player reference is assigned, try to find the player object automatically
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    void LateUpdate()
    {
        // Desired position of the camera (keeping the offset from the player)
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate between current camera position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position
        transform.position = smoothedPosition;
    }
}
