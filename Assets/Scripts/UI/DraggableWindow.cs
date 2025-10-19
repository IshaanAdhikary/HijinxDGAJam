using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float minVisibleWidth;
    [SerializeField] private float taskbarHeight;
    public UnityEvent OnWindowDragged;
    private Canvas canvas;
    private RectTransform rectTransform;
    private Vector2 offset;

    // Clamping data
    private float minX, maxX, minY, maxY;

    void Awake()
    {
        rectTransform = transform.parent.GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>(true);

        // Calculate bounds
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().rect.size;
        Vector2 windowSize = rectTransform.rect.size;
        float titleHeight = GetComponent<RectTransform>().rect.height;

        // Horizontal: allow window to go mostly off-screen
        minX = -(canvasSize.x / 2f) - (windowSize.x / 2f) + minVisibleWidth;
        maxX = (canvasSize.x / 2f) + (windowSize.x / 2f) - minVisibleWidth;

        // Vertical: keep title bar visible, allow bottom to go off
        minY = -(canvasSize.y / 2f) - (windowSize.y / 2f) + titleHeight + taskbarHeight;
        maxY = (canvasSize.y / 2f) - (windowSize.y / 2f);
    }

    private void BringToFront()
    {
        transform.parent.SetAsLastSibling();
    }

    private Vector2 clampPosition(Vector2 position)
    {
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPoint
        );
        offset = rectTransform.localPosition - (Vector3)localPoint;

        BringToFront();
        OnWindowDragged?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPoint
        );

        rectTransform.localPosition = clampPosition(localPoint + offset);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Nothing yet
    }
}