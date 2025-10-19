using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using TMPro;

public class WindowController : MonoBehaviour
{
    [Header("Window Settings")]
    [SerializeField] private string windowTitle = "Window";
    [SerializeField] private bool startOpen = false;
    [SerializeField] private bool canClose = true;
    [SerializeField] private bool canMinimize = true;

    [Header("UI References")]
    [SerializeField] private GameObject windowContent;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button minimizeButton;
    [SerializeField] private TaskbarController taskbar;

    [Header("Animation")]
    [SerializeField] private float animDuration = 0.2f;
    [SerializeField] private AnimationCurve openCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Events")]
    public UnityEvent OnWindowOpened;
    public UnityEvent OnWindowClosed;
    public UnityEvent OnWindowMinimized;
    public UnityEvent OnWindowRestored;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI minimizeButtonText;
    private bool isOpen;
    private bool isMinimized;
    private bool isAnimating;
    private Vector3 originalScale;
    private Coroutine currentAnimation;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;

        // Add canvas group for fade effects
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Setup buttons
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(Close);
            closeButton.gameObject.SetActive(canClose);
        }

        if (minimizeButton != null)
        {
            minimizeButton.onClick.AddListener(MinimizeButton);
            minimizeButton.gameObject.SetActive(canMinimize);
            minimizeButtonText = minimizeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
    }

    void Start()
    {
        if (startOpen)
            Open();
        else
        {
            gameObject.SetActive(false);
            isOpen = false;
        }
        if (taskbar != null)
        {
            taskbar.RegisterWindow(this);
        }
    }

    public void Open()
    {
        if (isOpen || isAnimating) return;

        gameObject.SetActive(true);
        isOpen = true;

        // Bring to front
        transform.SetAsLastSibling();

        // Animate open
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(AnimateOpen());

        OnWindowOpened?.Invoke();
    }

    public void Close()
    {
        if (!canClose || !isOpen || isAnimating) return;

        isOpen = false;

        // Animate close
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }
        currentAnimation = StartCoroutine(AnimateClose());

        OnWindowClosed?.Invoke();
    }

    public void MinimizeButton()
    {
        if (!isMinimized)
        {
            Minimize();
        }
        else
        {
            Restore();
        }
    }

    private void Minimize()
    {
        if (!canMinimize || !isOpen) return;

        // Simple minimize - just hide content
        if (windowContent != null)
        {
            windowContent.SetActive(false);
        }


        // Adjust minimize button
        if (minimizeButtonText != null)
        {
            minimizeButtonText.text = "+";
            minimizeButtonText.fontSize = 24;
            minimizeButtonText.rectTransform.offsetMax = new Vector2(0, 0);
        }

        isMinimized = true;
        OnWindowMinimized?.Invoke();
    }

    private void Restore()
    {
        if (windowContent != null)
        {
            windowContent.SetActive(true);
        }

        if (minimizeButtonText != null)
        {
            minimizeButtonText.text = "-";
            minimizeButtonText.fontSize = 60;
            minimizeButtonText.rectTransform.offsetMax = new Vector2(0, 10);
        }

        isMinimized = false;
        OnWindowRestored?.Invoke();
    }

    IEnumerator AnimateOpen()
    {
        isAnimating = true;

        // Start from zero scale and fade
        rectTransform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;

        float elapsed = 0f;

        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animDuration;
            float curveValue = openCurve.Evaluate(t);

            // Scale up
            rectTransform.localScale = Vector3.Lerp(Vector3.zero, originalScale, curveValue);

            // Fade in
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        // Ensure final values
        rectTransform.localScale = originalScale;
        canvasGroup.alpha = 1f;

        isAnimating = false;
    }

    IEnumerator AnimateClose()
    {
        isAnimating = true;

        float elapsed = 0f;

        while (elapsed < animDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animDuration;
            float curveValue = openCurve.Evaluate(1f - t); // Reverse curve

            // Scale down
            rectTransform.localScale = Vector3.Lerp(Vector3.zero, originalScale, curveValue);

            // Fade out
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, 1f - t);

            yield return null;
        }

        // Hide window
        gameObject.SetActive(false);
        rectTransform.localScale = originalScale;
        canvasGroup.alpha = 1f;

        isAnimating = false;
    }

    // Public getters
    public bool IsOpen() => isOpen;
    public bool IsMinimized() => isMinimized;
    public string GetTitle() => windowTitle;
    // Public setters
    public void SetTitle(string title) => windowTitle = title;
}