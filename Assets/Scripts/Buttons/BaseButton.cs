using UnityEngine;

public class BaseButton : MonoBehaviour
{
    public virtual void OnClick()
    {
        SfxManager.Instance.PlayClickSound();
    }

    public virtual void OnFocus()
    {
        SfxManager.Instance.PlayFocusSound();
    }
}
