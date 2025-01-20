using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject cardTarget;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text value;

    public void Initialize(int maxHealth)
    {
        value.text = $"{maxHealth}/{maxHealth}";
    }
    public void SetHealth(float currentHealth, float maxHealth)
    {
        currentHealth = math.max(currentHealth, 0);
        value.text = $"{currentHealth}/{maxHealth}";

        slider.DOValue(currentHealth / maxHealth, .5f)
            .OnComplete(() => 
            {
                if (cardTarget.GetComponent<ICardTarget>().isDied())
                {
                    cardTarget.GetComponent<CanvasGroup>()
                        .DOFade(0f, 0.5f).OnComplete(() => cardTarget.SetActive(false));
                }
            });
    }
}
