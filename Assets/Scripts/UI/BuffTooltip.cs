using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat;
using UnityEngine.UI;

public class BuffTooltip : AutoFieldValidator
{
    [SerializeField] private Image frameImage;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text defenseText;
    [SerializeField] private LocalizedString localizedAttack;
    [SerializeField] private LocalizedString localizedDefense;

    private int attack = 0;
    private int defense = 0;

    static public BuffTooltip FromPrefab(GameObject prefab, Transform parent, Vector3 localPosition)
    {
        var obj = Instantiate(prefab, parent);
        obj.transform.localPosition = localPosition + new Vector3(225f, 135f, 0);
        obj.SetActive(false);
        return obj.GetComponent<BuffTooltip>();
    }

    void Awake()
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

    public void SetTooltipData(int atk, int dfs)
    {
        attack = atk;
        defense = dfs;
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