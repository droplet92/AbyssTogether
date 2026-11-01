using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class PaletteColor : MonoBehaviour
{
    [SerializeField] private ColorToken token;
    [SerializeField, Range(0f, 1f)] private float alpha = 1f;

    public void Configure(ColorToken token, float alpha)
    {
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
        if (token == ColorToken.None) return;
        GetComponent<Graphic>().color = Palette.Get(token, alpha);
    }
}
