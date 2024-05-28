using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PrefabMaker : EditorWindow
{
    public Object go;
    public Object folder;
    string prefabName = "Enter name";
    string createAt = "";
    public bool isZeroSmoothness;
    public int additionalAnimations;
    public Object[] animations;

    [MenuItem("Tools/Prefab Maker")]

    public static void ShowWindow()
    {
        GetWindow(typeof(PrefabMaker));
    }

    void OnGUI()
    {
        GUILayout.Label("Prefab Folder", EditorStyles.boldLabel);
        folder = EditorGUILayout.ObjectField(folder, typeof(Object), true);
        createAt = AssetDatabase.GetAssetPath(folder);
        if (GUILayout.Button($"Try Auto Assets/_Prefabs"))
        {
            var autoObj = AssetDatabase.LoadAssetAtPath("Assets/_Prefabs", typeof(Object));
            if (autoObj != null)
            {
                folder = autoObj;
            }
        }
        if (GUILayout.Button($"Try Auto Assets/_Prefabs/Entities"))
        {
            var autoObj = AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Entities", typeof(Object));
            if (autoObj != null)
            {
                folder = autoObj;
            }
        }

        if (folder != null)
        {
            GUILayout.Space(20);
            GUILayout.Label("Object Main Model", EditorStyles.boldLabel);
            go = EditorGUILayout.ObjectField(go, typeof(Object), true);


            if (go != null)
            {
                var goOldName = go.name;
                GUILayout.Space(20);
                GUILayout.Label("Base Settings", EditorStyles.boldLabel);
                prefabName = EditorGUILayout.TextField("Prefab Name:", prefabName);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Is Zero Material Smoothness", EditorStyles.label);
                isZeroSmoothness = EditorGUILayout.Toggle(isZeroSmoothness);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Add Animation Models", EditorStyles.boldLabel);

                int oldCount = 0;
                if (animations != null)
                    oldCount = animations.Length;
                else
                    animations = new Object[0];

                additionalAnimations = EditorGUILayout.IntField(additionalAnimations);
                if (additionalAnimations < 0)
                    additionalAnimations = 0;
                if (additionalAnimations > 100)
                    additionalAnimations = 100;
                if (GUILayout.Button("Zero"))
                {
                    additionalAnimations = 0;
                    animations = new Object[0];
                }
                if (GUILayout.Button("Clear"))
                {
                    animations = new Object[additionalAnimations];
                }
                GUILayout.EndHorizontal();

                Object[] old = null;
                if (animations == null || additionalAnimations != animations.Length)
                {
                    old = new Object[animations.Length];
                    for (int i = 0; i < old.Length; i++)
                    {
                        old[i] = animations[i];
                    }

                    animations = new Object[additionalAnimations];
                    for (int i = 0; i < additionalAnimations && i < old.Length; i++)
                    {
                        animations[i] = old[i];
                    }
                }

                for (int i = 0; i < additionalAnimations; i++)
                {
                    animations[i] = EditorGUILayout.ObjectField(animations[i], typeof(Object), true);
                }

                string path = $"{createAt}/{prefabName}";

                bool isPrefabExist = AssetDatabase.IsValidFolder(path);
                if (!isPrefabExist)
                {
                    if (GUILayout.Button("Create"))
                    {
                        AssetDatabase.CreateFolder($"{createAt}", prefabName);
                        var modelPath = AssetDatabase.GetAssetPath(go);
                        var modelExtension = modelPath.Split('.')[1];
                        Debug.Log($"Move from {modelPath} to {path}");
                        var movedModelPath = $"{path}/{prefabName}Model.{modelExtension}";
                        AssetDatabase.MoveAsset(modelPath, movedModelPath);

                        AssetDatabase.CreateFolder($"{path}", "Materials");
                        ExtractMaterials(movedModelPath, $"{path}/Materials", prefabName);
                        AssetDatabase.CreateFolder($"{path}", "Animations");
                        ExtractAnimations(movedModelPath, $"{path}/Animations", goOldName);
                        foreach(var additionalAnim in animations)
                        {
                            if (additionalAnim != null)
                            {
                                ExtractAnimations(AssetDatabase.GetAssetPath(additionalAnim), $"{path}/Animations", additionalAnim.name, false);
                            }
                        }
                        try
                        {
                            AssetDatabase.CreateFolder($"{path}", "Textures");
                            ExtractTextures(movedModelPath, $"{path}/Textures");
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError($"Texture bug: {e}");
                        }

                        var SceneObject = Instantiate(go) as GameObject;
                        PrefabUtility.SaveAsPrefabAsset(SceneObject, $"{path}/{prefabName}.prefab");
                        DestroyImmediate(SceneObject);

                        AssetDatabase.Refresh();
                    }
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Asset already exist");
                    EditorGUILayout.Toggle(isPrefabExist);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }

    public void ExtractMaterials(string assetPath, string destinationPath, string prefix)
    {
        HashSet<string> hashSet = new HashSet<string>();
        IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
                                         where x.GetType() == typeof(Material)
                                         select x;

        int id = 0;
        foreach (Object item in enumerable)
        {
            string path = System.IO.Path.Combine(destinationPath, $"{prefix}Material{id}") + ".mat";
            id++;
            // string path = System.IO.Path.Combine(destinationPath, item.name) + ".mat";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            string value = AssetDatabase.ExtractAsset(item, path);
            if (string.IsNullOrEmpty(value))
            {
                hashSet.Add(path);
            }
        }

        AssetDatabase.WriteImportSettingsIfDirty(assetPath);
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

        foreach (string item2 in hashSet)
        {
            AssetDatabase.WriteImportSettingsIfDirty(item2);
            AssetDatabase.ImportAsset(item2, ImportAssetOptions.ForceUpdate);
            var asset = AssetDatabase.LoadAssetAtPath(item2, typeof(Material));
            if (isZeroSmoothness)
                ((Material)asset).SetFloat("_Smoothness", 0f);
        }
    }

    public void ExtractAnimations(string assetPath, string destinationPath, string defaultName = "", bool isOneF = true)
    {
        HashSet<string> hashSet = new HashSet<string>();
        IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
                                         where x.GetType() == typeof(AnimationClip)
                                         select x;

        bool isOne = enumerable.Count() <= 2 && isOneF;
        foreach (Object item in enumerable)
        {
            if (!item.name.Contains("__preview__"))
            {
                AnimationClip ac = new AnimationClip();

                EditorUtility.CopySerialized(item, ac);
                if (string.IsNullOrEmpty(defaultName))
                {
                    if (isOne)
                        AssetDatabase.CreateAsset(ac, $"{destinationPath}/{prefabName}.anim");
                    else
                        AssetDatabase.CreateAsset(ac, $"{destinationPath}/{item.name}.anim");
                }
                else
                {
                    AssetDatabase.CreateAsset(ac, $"{destinationPath}/{defaultName}.anim");
                }
            }
        }
    }

    public void ExtractTextures(string assetPath, string destinationPath)
    {
        HashSet<string> hashSet = new HashSet<string>();
        IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
                                         where x.GetType() == typeof(Texture)
                                         select x;

        foreach (Object item in enumerable)
        {
            string path = System.IO.Path.Combine(destinationPath, item.name) + $".{AssetDatabase.GetAssetPath(item).Split('.')[1]}";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            string value = AssetDatabase.ExtractAsset(item, path);
            if (string.IsNullOrEmpty(value))
            {
                hashSet.Add(assetPath);
            }
        }

        foreach (string item2 in hashSet)
        {
            AssetDatabase.WriteImportSettingsIfDirty(item2);
            AssetDatabase.ImportAsset(item2, ImportAssetOptions.ForceUpdate);
        }
    }
}