using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public HealthSystem playerHealthSystem; // assign in inspector
    public Image fadeImage; // assign in inspector
    public float fadeDuration = 1f;
    private bool isRestarting = false;

    void Start()
    {
        // Start fully black, then fade in
        fadeImage.color = new Color(0, 0, 0, 1);
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (playerHealthSystem != null && playerHealthSystem.currentHealth <= 0 && !isRestarting)
        {
            isRestarting = true;
            StartCoroutine(RestartWithFade());
        }
    }

    IEnumerator RestartWithFade()
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        float t = fadeDuration;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0); // fully transparent at end
    }
}
