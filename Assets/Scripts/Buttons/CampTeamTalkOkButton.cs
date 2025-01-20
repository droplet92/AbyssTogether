using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class CampTeamTalkOkButton : CampExitButton
{
    [SerializeField] private GameObject teamTalkPanel;
    [SerializeField] private TMP_Text label;
    [SerializeField] private LocalizedString removeString;
    [SerializeField] private LocalizedString copyString;

    private Image selected;
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
            var path = Path.Combine(Application.dataPath, "Deck.json");

            if (File.Exists(path))
            {
                DeckDataJson temp = new DeckDataJson()
                {
                    cards = new List<string>()
                    {
                        "Attack", "Attack", "AttackAll",
                        "Shield", "Shield", "ShieldAll",
                        "Heal", "Heal", "HealAll",
                        "Buff", "Debuff", "Skill"
                    }
                };
                var copy = temp.cards[copyIndex];
                temp.cards.RemoveAt(removeIndex);
                temp.cards.Insert(removeIndex, copy);
                File.WriteAllText(path, JsonUtility.ToJson(temp));
            }
            teamTalkPanel.SetActive(false);
            base.OnClick();
        }
    }
    public void SelectCard(CardUI card, int index)
    {
        if (selected != null)
            selected.color = Color.white;

        selected = card.GetComponentInChildren<Image>();
        selected.color = Color.black;

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
