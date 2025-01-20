using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour, ICardTarget
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

    public int CurrentHealth { get; set; } = 0;
    public int Attack { get => attack; }
    public int Defense { get => defense; }

    public delegate void OnAttackChanged();
    public event OnAttackChanged attackChanged = () => {};

    void Awake()
    {
        CurrentHealth = PlayerPrefs.GetInt($"Hp{gameObject.name}", maxHealth);
        
        if (isDied())
            gameObject.SetActive(false);
        else
            healthBar.SetHealth(CurrentHealth, maxHealth);
    }
    public bool CanApply(string targetType)
    {
        if (targetType == "Character") return true;
        if (targetType == "CharacterAll") return true;
        return false;
    }
    public void SetHighlight(bool isActive)
    {
        if (bodyHighlight != null)
            bodyHighlight.SetActive(isActive);
    }
    public bool isDied()
    {
        return CurrentHealth <= 0;
    }

    public void UpdateHealth(int delta)
    {
        int value = math.min(delta, maxHealth);
        CurrentHealth = math.min(CurrentHealth + value, maxHealth);

        hpDiff.gameObject.SetActive(true);
        hpDiff.text = value.ToString();

        healthBar.SetHealth(CurrentHealth, maxHealth);

        if (value < 0)
        {
            transform.DOShakePosition
            (
                duration: 0.5f,
                strength: 10f,
                vibrato: 100,
                randomness: 90,
                fadeOut: true
            );
            hpDiff.color = new Color32(0xBC, 0x01, 0x19, 0xFF);
        }
        else if (value > 0)
            hpDiff.color = new Color32(0x31, 0x62, 0x57, 0xFF);
        else
            hpDiff.color = Color.white;

        var localY = hpDiff.transform.localPosition.y;

        DOTween.Sequence()
            .Join(hpDiff.transform.DOLocalMoveY(localY + 30f, 0.5f))
            .Join(hpDiff.DOFade(0, 0.5f))
            .OnComplete(() =>
            {
                hpDiff.transform.DOLocalMoveY(localY, 0);
                hpDiff.gameObject.SetActive(false);
            });
    }
    public void UpdateAttack(int delta)
    {
        attack = math.max(attack + delta, 0);
        attackChanged();
        buffs.UpdateContents(delta, 0);
    }
    public void UpdateDefense(int delta)
    {
        defense = math.max(defense + delta, 0);
        buffs.UpdateContents(0, delta);
    }

    public void AnimateAttack()
    {
        var rectTransform = body.GetComponent<RectTransform>();

        rectTransform.DOPunchAnchorPos(new Vector2(100f, 0), 0.5f, 6)
            .SetEase(Ease.InExpo);
    }
    public void AnimateSpell()
    {
        var rectTransform = GetComponent<RectTransform>();

        rectTransform.DOJump(rectTransform.position, 10f, 3, 0.5f)
            .SetEase(Ease.InQuad);
    }
    public void AnimateHit()
    {
        var rectTransform = body.GetComponent<RectTransform>();

        rectTransform.DOPunchAnchorPos(new Vector2(-60f, 0), 0.5f, 5)
            .SetEase(Ease.InQuart);
    }
}
