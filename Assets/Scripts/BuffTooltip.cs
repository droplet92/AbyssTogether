using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat;
using UnityEngine.UI;

public class BuffTooltip : MonoBehaviour
{
    [SerializeField] private Image frameImage;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text defenseText;
    [SerializeField] private LocalizedString localizedAttack;
    [SerializeField] private LocalizedString localizedDefense;

    private int attack = 0;
    private int defense = 0;

    void Start()
    {
        localizedAttack.StringChanged += OnAttackChanged;
        localizedDefense.StringChanged += OnDefenseChanged;
        
        localizedAttack.RefreshString();
        localizedDefense.RefreshString();
    }
    void OnDestroy()
    {
        localizedAttack.StringChanged -= OnAttackChanged;
        localizedDefense.StringChanged -= OnDefenseChanged;
    }

    public void SetTooltipData(int attack, int defense)
    {
        this.attack = attack;
        this.defense = defense;

        localizedAttack.RefreshString();
        localizedDefense.RefreshString();
    }

    private void OnAttackChanged(string value)
    {
        attackText.text = value.FormatSmart(attack);
    }
    private void OnDefenseChanged(string value)
    {
        defenseText.text = value.FormatSmart(defense);
    }
}