using UnityEngine;

public class DeckButton : MonoBehaviour
{
    [SerializeField] private CanvasRenderer deckPanel;

    public void OnClick()
    {
        deckPanel.gameObject.SetActive(!deckPanel.gameObject.activeSelf);
    }
}
