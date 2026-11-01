using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour, ITarget
{
    [Header("캐릭터 기본 설정")]
    [SerializeField] private Image body;
    [SerializeField] private GameObject bodyHighlight;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TMP_Text hpDiff;
    [SerializeField] public Buffs buffs;
    [SerializeField] private int maxHealth = 0;
    [SerializeField] private int attack = 0;
    [SerializeField] private int defense = 0;

    public delegate void OnAttackChanged();
    public event OnAttackChanged attackChanged = () => {};

    public int CurrentHealth { get => unit?.CurrentHealth ?? 0; set => unit.CurrentHealth = value; }
    public int Attack => unit?.Attack ?? attack;
    public int Defense => unit?.Defense ?? defense;

    private Unit unit;

    public void Initialize()
    {
        buffs.Initialize();
        unit = new Unit(maxHealth, SaveData.GetHealth(gameObject.name, maxHealth), attack, defense);
        
        if (IsDead()) gameObject.SetActive(false);
        else healthBar.SetHealth(CurrentHealth, maxHealth);
    }
    public bool CanApply(string targetType)
    {
        if (IsDead()) return false;
        if (targetType == "Character") return true;
        if (targetType == "CharacterAll") return true;
        return false;
    }
    public void SetHighlight(bool isActive)
    {
        if (IsDead()) return;
        bodyHighlight?.SetActive(isActive);
    }
    public bool IsDead()
    {
        return unit == null || unit.IsDead;
    }

    public void UpdateHealth(int delta)
    {
        int? applied = unit?.ChangeHealth(delta);
        if (applied == null) return;

        int deltaHp = applied.Value;
        healthBar.SetHealth(CurrentHealth, maxHealth);

        hpDiff.gameObject.SetActive(true);
        hpDiff.color = GetHpDiffColor(deltaHp);
        hpDiff.text = deltaHp.ToString();

        if (deltaHp < 0)
            transform.DOShakePosition(0.5f, 10f, 100);

        float hpDiffBeginY = hpDiff.transform.localPosition.y;
        DOTween.Sequence()
            .Join(hpDiff.transform.DOLocalMoveY(hpDiffBeginY + 30f, 0.5f))
            .Join(hpDiff.DOFade(0, 0.5f))
            .OnComplete(() =>
            {
                hpDiff.transform.DOLocalMoveY(hpDiffBeginY, 0);
                hpDiff.gameObject.SetActive(false);
            });
    }
    public void UpdateAttack(int delta)
    {
        int? change = unit?.ChangeAttack(delta);
        if (change == null) return;

        buffs.UpdateContents(change.Value, 0);
        attackChanged();
    }
    public void UpdateDefense(int delta)
    {
        int? change = unit?.ChangeDefense(delta);
        if (change == null) return;

        buffs.UpdateContents(0, change.Value);
    }

    public void AnimateAttack()
    {
        if (IsDead()) return;
        body.GetComponent<RectTransform>()
            .DOPunchAnchorPos(new Vector2(100f, 0), 0.5f, 6)
            .SetEase(Ease.InExpo);
    }
    public void AnimateSpell()
    {
        if (IsDead()) return;
        transform.DOJump(transform.position, 10f, 3, 0.5f)
            .SetEase(Ease.InQuad);
    }
    public void AnimateHit()
    {
        if (IsDead()) return;
        body.GetComponent<RectTransform>()
            .DOPunchAnchorPos(new Vector2(-60f, 0), 0.5f, 5)
            .SetEase(Ease.InQuart);
    }
    
    private Color32 GetHpDiffColor(int delta)
    {
        if (delta < 0) return new Color32(0xBC, 0x01, 0x19, 0xFF);  // red
        if (delta > 0) return new Color32(0x31, 0x62, 0x57, 0xFF);  // green
        return Color.white;
    }
}
