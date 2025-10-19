using UnityEngine;
using UnityEngine.EventSystems;

public class PopUpButtonUI : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)

    {
        gameObject.SetActive(false);
    }
}
