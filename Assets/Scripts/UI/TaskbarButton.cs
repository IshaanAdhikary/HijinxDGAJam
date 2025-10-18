using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskbarButton : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color activeColor = new Color(0.3f, 0.3f, 0.3f);
    [SerializeField] private Color inactiveColor = new Color(0.2f, 0.2f, 0.2f);
    private Button button;
    private Image background;
    private TaskbarController taskbar;
    private WindowController window;
    private TextMeshProUGUI titleText;

    private bool isActive;

    void Awake()
    {
        titleText = transform.Find("ButtonTitleText").GetComponent<TextMeshProUGUI>();
        background = GetComponent<Image>();

        button = GetComponent<Button>();
        if (button != null)
        {
           button.onClick.AddListener(OnButtonClicked); 
        }
    }

    public void Initialize(WindowController windowController, TaskbarController taskbarManager)
    {
        window = windowController;
        taskbar = taskbarManager;

        // Set button text to window title
        if (titleText != null)
        {
            titleText.text = window.GetTitle();
        }

        SetActive(true);
    }

    private void OnButtonClicked()
    {
        if (window != null && taskbar != null)
        {
            window.MinimizeButton();
        }
    }

    public void SetActive(bool active)
    {
        if (background != null)
        {
            background.color = active ? activeColor : inactiveColor;
        }
    }
}
