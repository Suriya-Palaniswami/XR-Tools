using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

public class BuildPresetSaver : EditorWindow
{
    [System.Serializable]
    public class BuildPreset
    {
        public BuildTarget target;
        public string scriptingBackend;
        public string companyName;
        public string productName;
        public string outputPath;
    }

    private const string PresetFolder = "Assets/Editor/BuildPresets/";
    private string presetName = "";
    private Vector2 scroll;
    private List<string> savedPresets = new();

    [MenuItem("Window/XR Tools/Build Preset Saver")]
    public static void ShowWindow()
    {
        GetWindow<BuildPresetSaver>("Build Preset Saver");
    }

    private void OnEnable()
    {
        if (!Directory.Exists(PresetFolder))
            Directory.CreateDirectory(PresetFolder);

        RefreshPresetList();
    }

    private void OnGUI()
    {
        GUILayout.Label("Save Current Build Settings as Preset", EditorStyles.boldLabel);
        presetName = EditorGUILayout.TextField("Preset Name", presetName);

        if (GUILayout.Button("Save Preset") && !string.IsNullOrEmpty(presetName))
            SaveCurrentPreset();

        GUILayout.Space(10);
        GUILayout.Label("Available Presets", EditorStyles.boldLabel);
        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var preset in savedPresets)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(preset);
            if (GUILayout.Button("Load", GUILayout.Width(50)))
                LoadPreset(preset);
            if (GUILayout.Button("Open Folder", GUILayout.Width(100)))
                OpenPresetFolder(preset);
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    void RefreshPresetList()
    {
        savedPresets.Clear();
        if (!Directory.Exists(PresetFolder)) return;

        foreach (var file in Directory.GetFiles(PresetFolder, "*.json"))
        {
            savedPresets.Add(Path.GetFileNameWithoutExtension(file));
        }
    }

    void SaveCurrentPreset()
    {
        var preset = new BuildPreset
        {
            target = EditorUserBuildSettings.activeBuildTarget,
            scriptingBackend = PlayerSettings.GetScriptingBackend(BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget)).ToString(),
            companyName = PlayerSettings.companyName,
            productName = PlayerSettings.productName,
            outputPath = EditorUserBuildSettings.GetBuildLocation(EditorUserBuildSettings.activeBuildTarget)
        };

        var json = JsonUtility.ToJson(preset, true);
        File.WriteAllText(Path.Combine(PresetFolder, presetName + ".json"), json);
        RefreshPresetList();
    }

    void LoadPreset(string name)
    {
        var path = Path.Combine(PresetFolder, name + ".json");
        if (!File.Exists(path)) return;

        var json = File.ReadAllText(path);
        var preset = JsonUtility.FromJson<BuildPreset>(json);

        var targetGroup = BuildPipeline.GetBuildTargetGroup(preset.target);

        if (targetGroup == BuildTargetGroup.Unknown)
        {
            UnityEngine.Debug.LogWarning($"Unsupported target group for build target: {preset.target}");
            return;
        }

        EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, preset.target);
        PlayerSettings.SetScriptingBackend(targetGroup, (ScriptingImplementation)System.Enum.Parse(typeof(ScriptingImplementation), preset.scriptingBackend));
        PlayerSettings.companyName = preset.companyName;
        PlayerSettings.productName = preset.productName;
        EditorUserBuildSettings.SetBuildLocation(preset.target, preset.outputPath);

        UnityEngine.Debug.Log($"✅ Loaded Preset: {name}\nPlatform: {preset.target}\nScripting: {preset.scriptingBackend}\nOutput: {preset.outputPath}");
    }

    void OpenPresetFolder(string name)
    {
        var path = Path.Combine(PresetFolder, name + ".json");
        if (!File.Exists(path)) return;

        EditorUtility.RevealInFinder(path);
    }
}