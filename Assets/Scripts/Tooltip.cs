using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private Image frameImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;

    private LocalizedString localizedName;
    private LocalizedString localizedDescription;

    void Start()
    {
        // localizedName.StringChanged += OnNameChanged;
        // localizedDescription.StringChanged += OnDescriptionChanged;

        // localizedName.RefreshString();
        // localizedDescription.RefreshString();
    }
    void OnDestroy()
    {
        localizedName.StringChanged -= OnNameChanged;
        localizedDescription.StringChanged -= OnDescriptionChanged;
    }

    public void SetTooltipData(LocalizedString name, LocalizedString description)
    {
        localizedName = name;
        localizedDescription = description;
        localizedName.StringChanged += OnNameChanged;
        localizedDescription.StringChanged += OnDescriptionChanged;

        localizedName.RefreshString();
        localizedDescription.RefreshString();
    }

    private void OnNameChanged(string value)
    {
        itemName.text = value;
    }
    private void OnDescriptionChanged(string value)
    {
        itemDescription.text = value;
    }
}