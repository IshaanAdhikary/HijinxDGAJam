using System.Collections.Generic;
using UnityEngine;

public class TaskbarController : MonoBehaviour
{
    [SerializeField] private GameObject taskbarButtonPrefab;
    private Transform taskbarButtonContainer;
    private Dictionary<WindowController, TaskbarButton> windowButtons = new Dictionary<WindowController, TaskbarButton>();

    void Awake()
    {
        taskbarButtonContainer = transform.Find("TaskbarButtonContainer");
    }

    void Start()
    {
        WindowController[] windows = FindObjectsByType<WindowController>(FindObjectsSortMode.None);
        foreach (WindowController window in windows)
        {
            RegisterWindow(window);
        }
    }

    public void RegisterWindow(WindowController window)
    {
        if (windowButtons.ContainsKey(window))
            return;

        // Create taskbar button
        GameObject buttonObj = Instantiate(taskbarButtonPrefab, taskbarButtonContainer);
        TaskbarButton button = buttonObj.GetComponent<TaskbarButton>();

        if (button != null)
        {
            button.Initialize(window, this);
            windowButtons.Add(window, button);
        }

        // Listen to window events
        window.OnWindowOpened.AddListener(() => OnWindowOpened(window));
        window.OnWindowMinimized.AddListener(() => OnWindowMinimized(window));
        window.OnWindowRestored.AddListener(() => OnWindowRestored(window));
        window.OnWindowClosed.AddListener(() => OnWindowClosed(window));

        // Hide button initially if window is closed
        if (!window.IsOpen())
        {
            buttonObj.SetActive(false);
        }
    }

    public void UnregisterWindow(WindowController window)
    {
        if (windowButtons.TryGetValue(window, out TaskbarButton button))
        {
            Destroy(button.gameObject);
            windowButtons.Remove(window);
        }
    }

    private void OnWindowOpened(WindowController window)
    {
        if (windowButtons.TryGetValue(window, out TaskbarButton button))
        {
            button.gameObject.SetActive(true);
            button.SetActive(true);
        }
    }

    private void OnWindowMinimized(WindowController window)
    {
        if (windowButtons.TryGetValue(window, out TaskbarButton button))
        {
            button.SetActive(false);
        }
    }
    private void OnWindowRestored(WindowController window)
    {
        if (windowButtons.TryGetValue(window, out TaskbarButton button))
        {
            button.SetActive(true);
        }
    }

    private void OnWindowClosed(WindowController window)
    {
        if (windowButtons.TryGetValue(window, out TaskbarButton button))
        {
            button.gameObject.SetActive(false);
        }
    }

    public void BringWindowToFront(WindowController window)
    {
        window.Open();
        window.transform.SetAsLastSibling();

        // Update all button states
        foreach (var kvp in windowButtons)
        {
            kvp.Value.SetActive(kvp.Key == window);
        }

        // Bring taskbar even more front
        transform.SetAsLastSibling();
    }
}
