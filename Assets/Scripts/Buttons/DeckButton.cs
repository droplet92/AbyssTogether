using UnityEngine;

public class DeckButton : BaseButton
{
    [SerializeField] private CanvasRenderer deckPanel;

    public override void OnClick()
    {
        base.OnClick();
        deckPanel.gameObject.SetActive(!deckPanel.gameObject.activeSelf);
    }
}
