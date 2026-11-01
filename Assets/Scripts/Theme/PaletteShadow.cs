using UnityEngine;
using UnityEngine.UI;

public class PaletteShadow : MonoBehaviour
{
    [SerializeField] private Shadow effect;
    [SerializeField] private ColorToken token;
    [SerializeField, Range(0f, 1f)] private float alpha = 1f;

    public void Configure(Shadow effect, ColorToken token, float alpha)
    {
        this.effect = effect;
        this.token = token;
        this.alpha = alpha;
        Apply();
    }

    void Awake()
    {
        Apply();
    }

    void OnValidate()
    {
        Apply();
    }

    private void Apply()
    {
        if (effect == null || token == ColorToken.None) return;
        effect.effectColor = Palette.Get(token, alpha);
    }
}
