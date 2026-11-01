using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class PaletteSelectable : MonoBehaviour
{
    [Serializable]
    public struct State
    {
        public ColorToken token;
        [Range(0f, 1f)] public float alpha;

        public State(ColorToken token, float alpha)
        {
            this.token = token;
            this.alpha = alpha;
        }

        public Color Resolve(Color current) => token == ColorToken.None ? current : Palette.Get(token, alpha);
    }

    [SerializeField] private State normal;
    [SerializeField] private State highlighted;
    [SerializeField] private State pressed;
    [SerializeField] private State selected;
    [SerializeField] private State disabled;

    public void Configure(State normal, State highlighted, State pressed, State selected, State disabled)
    {
        this.normal = normal;
        this.highlighted = highlighted;
        this.pressed = pressed;
        this.selected = selected;
        this.disabled = disabled;
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
        var selectable = GetComponent<Selectable>();
        var colors = selectable.colors;
        colors.normalColor = normal.Resolve(colors.normalColor);
        colors.highlightedColor = highlighted.Resolve(colors.highlightedColor);
        colors.pressedColor = pressed.Resolve(colors.pressedColor);
        colors.selectedColor = selected.Resolve(colors.selectedColor);
        colors.disabledColor = disabled.Resolve(colors.disabledColor);
        selectable.colors = colors;
    }
}
