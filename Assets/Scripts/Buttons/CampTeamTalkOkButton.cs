using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class CampTeamTalkOkButton : CampExitButton
{
    [SerializeField] private GameObject teamTalkPanel;
    [SerializeField] private TMP_Text label;
    [SerializeField] private LocalizedString removeString;
    [SerializeField] private LocalizedString copyString;

    private CardUI selected;
    private bool isRemoveCardPhase = true;
    private int removeIndex = -1;
    private int copyIndex = -1;

    void Awake()
    {
        removeString.StringChanged += OnLabelChanged;
        removeString.RefreshString();
    }
    void OnDestroy()
    {
        copyString.StringChanged -= OnLabelChanged;
    }

    public override void OnClick()
    {
        if (isRemoveCardPhase && removeIndex >= 0)
        {
            BaseOnClick();
            selected = null;
            isRemoveCardPhase = false;
            removeString.StringChanged -= OnLabelChanged;
            copyString.StringChanged += OnLabelChanged;
            copyString.RefreshString();
        }
        else if (copyIndex >= 0)
        {
            var data = PlayerPrefs.GetString("Deck");

            if (data.Length > 0)
            {
                var deck = JsonUtility.FromJson<DeckDataJson>(data);
                var copy = deck.cards[copyIndex];
                deck.cards.RemoveAt(removeIndex);
                deck.cards.Insert(removeIndex, copy);
                PlayerPrefs.SetString("Deck", JsonUtility.ToJson(deck));
            }
            teamTalkPanel.SetActive(false);
            base.OnClick();
        }
    }
    public void SelectCard(CardUI card, int index)
    {
        if (selected != null)
            selected.SetOX(!isRemoveCardPhase, false);

        selected = card;
        selected.SetOX(!isRemoveCardPhase, true);

        if (isRemoveCardPhase)
            removeIndex = index;
        else
            copyIndex = index;
    }

    private void OnLabelChanged(string value)
    {
        label.text = value;
    }
}
