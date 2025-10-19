using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpButtonUITest : MonoBehaviour, IPointerDownHandler
{
    // This MUST be public or [SerializeField] to appear in the Inspector
    public Sprite[] imagePool;

    private Image imageComponent;

    void Awake()
    {
        imageComponent = GetComponent<Image>();
    }

    void OnEnable()
    {
        if (imageComponent != null && imagePool != null && imagePool.Length > 0)
        {
            int i = Random.Range(0, imagePool.Length);
            imageComponent.sprite = imagePool[i];
        }
    }

    public void OnPointerDown(PointerEventData _)
    {
        gameObject.SetActive(false);
    }
}
