using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private GameObject pausePanel;
    private bool isPaused = false;

    void Start()
    {
        CreatePauseUI();
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    void CreatePauseUI()
    {
        // Create Canvas
        GameObject canvasGO = new GameObject("PauseCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Create Panel as child of Canvas
        pausePanel = new GameObject("PausePanel");
        pausePanel.transform.SetParent(canvasGO.transform, false);

        Image panelImage = pausePanel.AddComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.5f); // semi-transparent black

        RectTransform panelRT = pausePanel.GetComponent<RectTransform>();
        panelRT.anchorMin = new Vector2(0, 0);
        panelRT.anchorMax = new Vector2(1, 1);
        panelRT.offsetMin = Vector2.zero;
        panelRT.offsetMax = Vector2.zero;

        // Create Play Button
        GameObject playBtnGO = CreateButton("Play", new Vector2(0, 50), pausePanel.transform);
        Button playBtn = playBtnGO.GetComponent<Button>();
        playBtn.onClick.AddListener(Resume);

        // Create Exit Button
        GameObject exitBtnGO = CreateButton("Exit", new Vector2(0, -50), pausePanel.transform);
        Button exitBtn = exitBtnGO.GetComponent<Button>();
        exitBtn.onClick.AddListener(Exit);
    }

    GameObject CreateButton(string buttonText, Vector2 anchoredPosition, Transform parent)
    {
        GameObject buttonGO = new GameObject(buttonText + "Button");
        buttonGO.transform.SetParent(parent, false);

        // Add Button components
        Button button = buttonGO.AddComponent<Button>();
        Image image = buttonGO.AddComponent<Image>();
        image.color = Color.white;

        // RectTransform setup
        RectTransform rt = buttonGO.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(160, 40);
        rt.anchoredPosition = anchoredPosition;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        // Add Text child
        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(buttonGO.transform, false);

        Text text = textGO.AddComponent<Text>();
        text.text = buttonText;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;

        RectTransform textRT = textGO.GetComponent<RectTransform>();
        textRT.anchorMin = new Vector2(0, 0);
        textRT.anchorMax = new Vector2(1, 1);
        textRT.offsetMin = Vector2.zero;
        textRT.offsetMax = Vector2.zero;

        return buttonGO;
    }

    void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
