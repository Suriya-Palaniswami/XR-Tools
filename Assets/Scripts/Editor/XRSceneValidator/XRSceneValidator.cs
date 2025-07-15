using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class XRSceneValidator : EditorWindow
{
    [MenuItem("Window/XR Tools/Scene Validator")]
    public static void ShowWindow()
    {
        GetWindow<XRSceneValidator>("XR Scene Validator");
    }

    private Vector2 scroll;
    private string report = "";

    void OnGUI()
    {
        if (GUILayout.Button("Run Scene Validation"))
        {
            RunValidation();
        }

        GUILayout.Space(10);

        scroll = EditorGUILayout.BeginScrollView(scroll);
        EditorGUILayout.TextArea(report, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
    }

    void RunValidation()
    {
        report = "XR Scene Validation Report:\n";

        // Check for XR Rig (any GameObject with XR-related components)
        var xrRigs = FindObjectsOfType<MonoBehaviour>()
            .Where(comp => comp.GetType().Name.Contains("XR") || comp.GetType().Name.Contains("Rig"))
            .Select(comp => comp.gameObject)
            .Distinct()
            .ToList();

        if (xrRigs.Count == 0)
            report += "❌ No XR Rig found in scene.\n";
        else
            report += $"✅ XR Rig(s) found: {xrRigs.Count}\n";

        // Check for Main Camera
        var cam = Camera.main;
        report += cam == null ? "❌ No Camera tagged as 'MainCamera'.\n" : "✅ Main Camera found.\n";

        // Check for EventSystem
        var eventSystem = GameObject.FindObjectOfType<EventSystem>();
        report += eventSystem == null ? "❌ No EventSystem found in scene.\n" : "✅ EventSystem found.\n";

        #if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            report += "✅ Input System Package is active.\n";
        #elif ENABLE_INPUT_SYSTEM && ENABLE_LEGACY_INPUT_MANAGER
                report += "⚠️ Both Input System and Legacy Input are enabled.\n";
        #elif !ENABLE_INPUT_SYSTEM && ENABLE_LEGACY_INPUT_MANAGER
            report += "⚠️ Legacy Input Manager is active (consider upgrading).\n";
        #else
            report += "❌ No input system enabled.\n";
        #endif


        // Check for recommended layers (Hands, UI, Interactable, etc.)
        var recommendedLayers = new[] { "UI", "XR", "Hands", "Interactable" };
        foreach (var layer in recommendedLayers)
        {
            if (LayerMask.NameToLayer(layer) == -1)
                report += $"⚠️ Layer '{layer}' is missing.\n";
            else
                report += $"✅ Layer '{layer}' exists.\n";
        }

        report += "\nDone.";
    }
}
