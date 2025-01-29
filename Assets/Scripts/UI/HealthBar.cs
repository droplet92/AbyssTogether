using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : AutoFieldValidator
{
    [SerializeField] private GameObject cardTarget;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text value;

    public void Initialize(int maxHealth)
    {
        UpdateText(maxHealth, maxHealth);
    }
    public void SetHealth(float currentHealth, float maxHealth)
    {
        currentHealth = math.max(currentHealth, 0);
        UpdateText(currentHealth, maxHealth);

        slider.DOValue(currentHealth / maxHealth, .5f)
            .OnComplete(() => 
            {
                if (cardTarget.GetComponent<ITarget>().IsDead())
                {
                    cardTarget.GetComponent<CanvasGroup>()
                        .DOFade(0f, 0.5f)
                        .OnComplete(() => cardTarget.SetActive(false));
                }
            });
    }

    private void UpdateText(float currentHealth, float maxHealth)
    {
        value.text = $"{currentHealth}/{maxHealth}";
    }
}
