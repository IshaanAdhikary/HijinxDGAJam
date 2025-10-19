using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpButtonUI : MonoBehaviour
{
    public Sprite[] imagePool;

    Image img;
    Button closeButton;
    bool closing;

    void Awake()
    {
        img = GetComponent<Image>();
        if (img != null) img.raycastTarget = true;     // parent blocks, but does not act on, clicks

        closeButton = GetComponentInChildren<Button>(true);
        if (closeButton != null)
        {
            // Hard reset: remove BOTH persistent (Inspector) and runtime listeners
            closeButton.onClick = new Button.ButtonClickedEvent();
            closeButton.onClick.AddListener(ClosePopup);
        }
    }

    void OnEnable()
    {
        if (img != null && imagePool != null && imagePool.Length > 0)
            img.sprite = imagePool[Random.Range(0, imagePool.Length)];
    }

    public void ClosePopup()
    {
        if (!closing) StartCoroutine(CloseNextFrame());
    }

    IEnumerator CloseNextFrame()
    {
        closing = true;
        yield return null;              // let the click finish so it doesn't affect others
        gameObject.SetActive(false);
        closing = false;
    }
}
