using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MovementType { Horizontal, Vertical, Both }
    public MovementType movementType = MovementType.Both; // Set this in the Inspector for each platform

    public float amplitudeX = 2f;  // Horizontal movement distance
    public float amplitudeY = 2f;  // Vertical movement distance
    public float speed = 2f;       // Speed of movement
    public float curveFrequency = 1f; // Frequency of the sine wave curve

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Default offsets
        float offsetX = 0f;
        float offsetY = 0f;

        // Calculate the curve motion for each movement type
        switch (movementType)
        {
            case MovementType.Horizontal:
                offsetX = Mathf.Sin(Time.time * speed) * amplitudeX;  // Horizontal sine wave movement
                break;

            case MovementType.Vertical:
                offsetY = Mathf.Sin(Time.time * speed) * amplitudeY;  // Vertical sine wave movement
                break;

            case MovementType.Both:
                // Both horizontal and vertical sine wave movement
                offsetX = Mathf.Sin(Time.time * speed) * amplitudeX; // Horizontal sine wave movement
                offsetY = Mathf.Sin(Time.time * speed * curveFrequency) * amplitudeY; // Vertical sine wave (with curve frequency)
                break;
        }

        // Apply the calculated offsets to create curving movement
        transform.position = startPos + new Vector3(offsetX, offsetY, 0);
    }
}
