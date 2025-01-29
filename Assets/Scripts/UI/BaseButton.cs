using UnityEngine;
using UnityEngine.EventSystems;

public class BaseButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        SfxManager.Instance.PlayFocusSound();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        SfxManager.Instance.PlayClickSound();
    }
}
