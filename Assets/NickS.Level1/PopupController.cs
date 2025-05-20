using System.Collections;
using UnityEngine;
using TMPro;

public class PopupController : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.3f;
    public float displayDuration = 2f;

    private Coroutine popupRoutine;

    public void ShowMessage(string message)
    {
        if (popupRoutine != null)
        {
            StopCoroutine(popupRoutine);
        }

        popupRoutine = StartCoroutine(ShowPopup(message));
    }

    private IEnumerator ShowPopup(string message)
    {
        popupText.text = message;

        // Fade in
        canvasGroup.alpha = 0;
        popupText.gameObject.SetActive(true);
        yield return FadeCanvas(0, 1);

        // Wait
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        yield return FadeCanvas(1, 0);
        popupText.gameObject.SetActive(false);
    }

    private IEnumerator FadeCanvas(float start, float end)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = end;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowMessage("Test Message!");
        }
    }
}