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
            if (field.GetCustomAttribute<SerializeField>() != null)
            {
                var value = field.GetValue(this);
                Debug.Assert(value != null, $"[AutoFieldValidator] SerializedField '{field.Name}' is null in '{name}'");
            }
        }
    }
}
