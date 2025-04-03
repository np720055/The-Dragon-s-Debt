using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    public Transform player;

    private Material material;
    private Vector2 offset;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        offset = Vector2.zero;
    }

    void Update()
    {
        offset.x = player.position.x * scrollSpeed;
        material.mainTextureOffset = offset;
    }
}
