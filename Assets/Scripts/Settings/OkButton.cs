using UnityEngine;

public class OkButton : MonoBehaviour
{
    [SerializeField] private CanvasRenderer settingsPanel;

    public void OnClick()
    {
        settingsPanel.gameObject.SetActive(false);
    }
}
