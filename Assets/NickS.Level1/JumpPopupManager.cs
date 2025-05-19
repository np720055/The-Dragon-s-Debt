using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPopupManager : MonoBehaviour
{
    public GameObject popupUI;

    public void ShowPopup(float duration)
    {
        StartCoroutine(PopupRoutine(duration));
    }

    IEnumerator PopupRoutine(float duration)
    {
        popupUI.SetActive(true);
        yield return new WaitForSeconds(duration);
        popupUI.SetActive(false);
    }
}
