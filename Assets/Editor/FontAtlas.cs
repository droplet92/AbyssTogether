using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.TextCore.LowLevel;

public static class FontAtlas
{
    static readonly string[][] FallbackChains =
    {
        new[] { "Assets/Resources/Fonts/WavvePADO-Regular.asset", "Assets/Resources/Fonts/MPLUS2-Bold.asset" },
        new[] { "Assets/Resources/Fonts/WavvePADO-Regular-SDF.asset", "Assets/Resources/Fonts/MPLUS2-Bold-SDF.asset" },
    };

    [MenuItem("Tools/Rebuild Font Atlases")]
    public static void Rebuild()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        var setup = EditorSceneManager.GetSceneManagerSetup();
        string characters;
        try
        {
            characters = CollectCharacters(includeScenes: true);
        }
        finally
        {
            if (setup.Length > 0) EditorSceneManager.RestoreSceneManagerSetup(setup);
        }

        foreach (var chain in FallbackChains)
        {
            string remaining = characters;
            foreach (var path in chain)
                remaining = Rebuild(AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path), remaining);
            if (remaining.Length > 0)
                Debug.LogError($"No font in {chain[0]} chain has: {remaining}");
        }
        AssetDatabase.SaveAssets();
    }

    public static string CollectCharacters(bool includeScenes)
    {
        var set = new HashSet<char>();
        void Add(string s)
        {
            if (s == null) return;
            foreach (var c in Regex.Replace(s, "<[^>]*>", ""))
                if (!char.IsControl(c)) set.Add(c);
        }
        void AddSerializedStrings(Object o)
        {
            var it = new SerializedObject(o).GetIterator();
            while (it.Next(true))
                if (it.propertyType == SerializedPropertyType.String) Add(it.stringValue);
        }

        for (char c = ' '; c <= '~'; c++) set.Add(c);

        foreach (var guid in AssetDatabase.FindAssets("t:StringTable"))
            foreach (var entry in AssetDatabase.LoadAssetAtPath<StringTable>(AssetDatabase.GUIDToAssetPath(guid)).Values)
                Add(entry.Value);

        foreach (var guid in AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/GameDatabase" }))
            foreach (var o in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(guid)))
                if (o is ScriptableObject) AddSerializedStrings(o);

        if (includeScenes)
        {
            foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
                foreach (var root in EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single).GetRootGameObjects())
                    foreach (var component in root.GetComponentsInChildren<MonoBehaviour>(true))
                        if (component != null) AddSerializedStrings(component);

            foreach (var guid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" }))
                foreach (var component in AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid)).GetComponentsInChildren<MonoBehaviour>(true))
                    if (component != null) AddSerializedStrings(component);
        }
        return new string(set.OrderBy(c => c).ToArray());
    }

    // Keeps the asset's material object so texts that reference it stay connected.
    static string Rebuild(TMP_FontAsset asset, string characters)
    {
        var font = SourceFont(asset);
        int pointSize = (int)asset.faceInfo.pointSize;
        string lacking = new string(characters.Where(c => !HasGlyph(font, pointSize, c)).ToArray());
        string wanted = new string(characters.Except(lacking).ToArray());

        var generated = Generate(font, pointSize, asset.atlasPadding, asset.atlasRenderMode, wanted);
        var oldTexture = asset.atlasTexture;
        var texture = new Texture2D(generated.atlasWidth, generated.atlasHeight, TextureFormat.Alpha8, false)
        {
            name = oldTexture.name,
            filterMode = oldTexture.filterMode,
        };
        texture.LoadRawTextureData(generated.atlasTexture.GetRawTextureData());
        texture.Apply(false, false);
        AssetDatabase.AddObjectToAsset(texture, asset);

        var target = new SerializedObject(asset);
        var source = new SerializedObject(generated);
        foreach (var name in new[] { "m_GlyphTable", "m_CharacterTable", "m_UsedGlyphRects", "m_FreeGlyphRects", "m_AtlasWidth", "m_AtlasHeight" })
            target.CopyFromSerializedProperty(source.FindProperty(name));
        target.ApplyModifiedPropertiesWithoutUndo();

        asset.atlasTextures = new[] { texture };
        asset.material.SetTexture(ShaderUtilities.ID_MainTex, texture);
        asset.material.SetFloat(ShaderUtilities.ID_TextureWidth, texture.width);
        asset.material.SetFloat(ShaderUtilities.ID_TextureHeight, texture.height);
        Object.DestroyImmediate(oldTexture, true);
        asset.ReadFontAssetDefinition();
        EditorUtility.SetDirty(asset);

        Debug.Log($"{asset.name}: {asset.characterTable.Count} glyphs, atlas {texture.width}x{texture.height}");
        return lacking;
    }

    static Font SourceFont(TMP_FontAsset asset)
    {
        var guid = new SerializedObject(asset).FindProperty("m_SourceFontFileGUID").stringValue;
        return AssetDatabase.LoadAssetAtPath<Font>(AssetDatabase.GUIDToAssetPath(guid));
    }

    static TMP_FontAsset Generate(Font font, int pointSize, int padding, GlyphRenderMode mode, string characters)
    {
        foreach (var size in new[] { 256, 512, 1024, 2048, 4096 })
        {
            var generated = TMP_FontAsset.CreateFontAsset(font, pointSize, padding, mode, size, size, AtlasPopulationMode.Dynamic, false);
            if (generated.TryAddCharacters(characters, out var missing) || string.IsNullOrEmpty(missing))
                return generated;
        }
        throw new System.InvalidOperationException($"{font.name}: characters do not fit in a 4096 atlas");
    }

    static bool HasGlyph(Font font, int pointSize, char c)
    {
        FontEngine.LoadFontFace(font, pointSize);
        return FontEngine.TryGetGlyphIndex(c, out var index) && index != 0;
    }

    public static IEnumerable<string> FindMissing(string characters)
    {
        foreach (var chain in FallbackChains)
        {
            var assets = chain.Select(AssetDatabase.LoadAssetAtPath<TMP_FontAsset>).ToList();
            var missing = characters.Where(c => !assets.Any(a => a.characterLookupTable.ContainsKey(c))).ToArray();
            if (missing.Length > 0)
                yield return $"{assets[0].name}: {new string(missing)}";
        }
    }
}

public class FontCoverageCheck : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        var missing = FontAtlas.FindMissing(FontAtlas.CollectCharacters(includeScenes: false)).ToList();
        if (missing.Count > 0)
            throw new BuildFailedException("Characters missing from font assets. Run Tools/Rebuild Font Atlases.\n" + string.Join("\n", missing));
    }
}
