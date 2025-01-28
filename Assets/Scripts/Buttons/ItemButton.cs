using UnityEngine;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private RewardPanel panel;

    public void OnClick()
    {
        panel.gameObject.SetActive(true);
        panel.ShowItem();
    }
}
