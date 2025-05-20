using System.Collections;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    public TextMeshProUGUI popupText;

    public void ShowMessage(string message)
    {
        StopAllCoroutines(); // Cancel any running popups
        StartCoroutine(Show(message));
    }

    private IEnumerator Show(string message)
    {
        popupText.text = message;
        popupText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        popupText.gameObject.SetActive(false);
    }
}