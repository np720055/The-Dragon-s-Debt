using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    public Tilemap tilemap;                 // Assign your Tilemap
    public TileBase openDoorTile;          // Assign in Inspector
    public Vector3Int doorTilePosition;    // Set the tile position manually
    public GameObject popupUI;             // Assign a popup UI

    private bool unlocked = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (unlocked) return;

        if (other.CompareTag("Player")) // Check if the player collides
        {
            PlayerInventory playerInv = other.GetComponent<PlayerInventory>();
            if (playerInv != null)
            {
                var popup = popupUI.GetComponent<PopupController>(); // Use the PopupController script
                if (playerInv.hasKey)
                {
                    tilemap.SetTile(doorTilePosition, openDoorTile); // Change the door tile to unlocked
                    GetComponent<Collider2D>().enabled = false; // Disable door collider
                    popup.ShowMessage("The door was unlocked!"); // Show popup
                    unlocked = true;
                }
                else
                {
                    popup.ShowMessage("You need a key to open this door."); // Show popup if no key
                }
            }
        }
    }
}