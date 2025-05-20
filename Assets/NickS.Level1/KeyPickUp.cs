using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public GameObject popupUI; // Assign in Inspector

    private bool opened = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!opened && other.CompareTag("Player"))
        {
            PlayerInventory playerInv = other.GetComponent<PlayerInventory>();
            if (playerInv != null)
            {
                playerInv.hasKey = true;
               
                opened = true;
            }
        }
    }
}
