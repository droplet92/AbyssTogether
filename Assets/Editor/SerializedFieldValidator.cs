using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SerializedFieldValidator : IProcessSceneWithReport
{
    public int callbackOrder => 0;

    private const BindingFlags DeclaredInstanceFields =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

    public void OnProcessScene(Scene scene, BuildReport report)
    {
        var issues = ValidateScene(scene).ToList();
        if (issues.Count == 0) return;

        if (report == null)
        {
            foreach (var issue in issues)
                Debug.LogError(issue);
            return;
        }
        throw new BuildFailedException($"Unassigned serialized fields:\n{string.Join("\n", issues)}");
    }

    private static IEnumerable<string> ValidateScene(Scene scene)
    {
        return scene.GetRootGameObjects()
            .SelectMany(root => root.GetComponentsInChildren<MonoBehaviour>(true))
            .SelectMany(component => ValidateComponent(component, scene.path));
    }

    private static IEnumerable<string> ValidateComponent(MonoBehaviour component, string context)
    {
        if (component == null || !IsProjectScript(component.GetType()))
            return Enumerable.Empty<string>();

        var location = $"{context}: {GetPath(component.transform)}";
        var typeName = component.GetType().Name;
        return UnassignedFields(component)
            .Select(name => $"{location} ({typeName}.{name}) is not assigned");
    }

    private static bool IsProjectScript(System.Type type)
    {
        return type.Assembly.GetName().Name == "Assembly-CSharp";
    }

    // Field names, or "name[i]" for list elements, that are left empty.
    private static IEnumerable<string> UnassignedFields(MonoBehaviour component)
    {
        return RequiredFields(component.GetType())
            .SelectMany(field => UnassignedSlots(field, field.GetValue(component)));
    }

    private static IEnumerable<FieldInfo> RequiredFields(System.Type type)
    {
        return TypeHierarchy(type)
            .SelectMany(t => t.GetFields(DeclaredInstanceFields))
            .Where(field => IsSerialized(field) && !field.IsDefined(typeof(OptionalAttribute)));
    }

    private static IEnumerable<System.Type> TypeHierarchy(System.Type type)
    {
        for (var t = type; t != typeof(MonoBehaviour); t = t.BaseType)
            yield return t;
    }

    private static IEnumerable<string> UnassignedSlots(FieldInfo field, object value)
    {
        if (typeof(Object).IsAssignableFrom(field.FieldType))
        {
            if ((Object)value == null)
                yield return field.Name;
            yield break;
        }
        if (value is not IList list || !IsObjectList(field.FieldType))
            yield break;

        for (int i = 0; i < list.Count; i++)
        {
            if ((Object)list[i] == null)
                yield return $"{field.Name}[{i}]";
        }
    }

    private static bool IsSerialized(FieldInfo field)
    {
        if (field.IsDefined(typeof(System.NonSerializedAttribute))) return false;
        return field.IsPublic || field.IsDefined(typeof(SerializeField));
    }

    private static bool IsObjectList(System.Type type)
    {
        var element = type.IsArray ? type.GetElementType() : type.IsGenericType ? type.GetGenericArguments()[0] : null;
        return element != null && typeof(Object).IsAssignableFrom(element);
    }

    private static string GetPath(Transform transform)
    {
        return transform.parent == null ? transform.name : $"{GetPath(transform.parent)}/{transform.name}";
    }
}
