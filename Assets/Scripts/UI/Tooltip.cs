using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class Tooltip : AutoFieldValidator
{
    [SerializeField] private Image frameImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;

    private LocalizedString localizedName;
    private LocalizedString localizedDescription;

    static public Tooltip FromPrefab(GameObject prefab, Transform parent, Vector3 localPosition)
    {
        var obj = Instantiate(prefab, parent);
        obj.transform.localPosition = localPosition + new Vector3(305f, -40f, 0);
        obj.SetActive(false);
        return obj.GetComponent<Tooltip>();
    }

    void OnDestroy()
    {
        if (localizedName != null)
            localizedName.StringChanged -= OnNameChanged;
        
        if (localizedDescription != null)
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