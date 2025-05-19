using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chalice : MonoBehaviour
{
    public GameObject popupUI; // Assign the popup UI object in inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        DoubleJumpAbility doubleJump = other.GetComponent<DoubleJumpAbility>();
        if (doubleJump != null)
        {
            doubleJump.EnableDoubleJump();

            if (popupUI != null)
            {
                FindObjectOfType<JumpPopupManager>().ShowPopup(3f);
            }

            Destroy(gameObject); // Remove chalice
        }
    }

    IEnumerator HidePopup()
    {
        yield return new WaitForSeconds(3f);
        if (popupUI != null)
            popupUI.SetActive(false);
    }

}
