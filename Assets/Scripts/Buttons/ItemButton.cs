using UnityEngine;

public class ItemButton : BaseButton
{
    [SerializeField] private RewardPanel panel;

    public override void OnClick()
    {
        base.OnClick();
        panel.gameObject.SetActive(true);
        panel.ShowItem();
    }
}
