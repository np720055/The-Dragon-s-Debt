using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;
    public GameObject blackScreen;
    public GameObject loadingGif;

    void Start()
    {
        playButton.onClick.AddListener(OnPlayButton);
        exitButton.onClick.AddListener(OnExitButton);

        if (blackScreen != null) blackScreen.SetActive(false);
        if (loadingGif != null) loadingGif.SetActive(false);
    }

    void OnPlayButton()
    {
        Debug.Log("Play button clicked");
        StartCoroutine(LoadSceneWithLoading("Castle Halls"));
    }

    void OnExitButton()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

    System.Collections.IEnumerator LoadSceneWithLoading(string sceneName)
    {
        // Show black screen + loading gif
        if (blackScreen != null) blackScreen.SetActive(true);
        if (loadingGif != null) loadingGif.SetActive(true);

        // Hide buttons
        if (playButton != null) playButton.gameObject.SetActive(false);
        if (exitButton != null) exitButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.1f); // give time for UI to update

        // Start loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
