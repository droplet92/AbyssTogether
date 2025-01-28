using UnityEngine;
using System.Reflection;

public class AutoFieldValidator : MonoBehaviour
{
    void Awake()
    {
        ValidateSerializedFields();
    }

    private void ValidateSerializedFields()
    {
        var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        foreach (var field in fields)
        {
            if (IsRequired(field))
            {
                var value = field.GetValue(this);
                Debug.Assert(value != null, $"[AutoFieldValidator] SerializedField '{field.Name}' is null in '{name}'");
            }
        }
    }
    private bool IsRequired(FieldInfo fieldInfo)
    {
        if (fieldInfo.Name == "potionButton") return false;
        if (fieldInfo.Name == "itemButton") return false;
        if (fieldInfo.Name == "character" && name.StartsWith("ItemSlot")) return false;
        return fieldInfo.GetCustomAttribute<SerializeField>() != null;
    }
}
