#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(ThemeManager))]
    [System.Serializable]
    public class ThemeManagerEditor : Editor
    {
        GUISkin customSkin;
        protected static string buildID = "B16-20211208";
        protected static float foldoutItemSpace = 2;
        protected static float foldoutTopSpace = 5;
        protected static float foldoutBottomSpace = 2;

        protected static bool showGeneralSettings = false;
        protected static bool showColors = false;
        protected static bool showFonts = false;

        void OnEnable()
        {
            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Light"); }
        }

        public override void OnInspectorGUI()
        {
            if (customSkin == null)
            {
                EditorGUILayout.HelpBox("Editor variables are missing. You can manually fix this by deleting " +
                    "DreamOS > Resources folder and then re-import the package. \n\nIf you're still seeing this " +
                    "dialog even after the re-import, contact me with this ID: " + buildID, MessageType.Error);

                if (GUILayout.Button("Contact")) { Email(); }
                return;
            }

            // Foldout style
            GUIStyle foldoutStyle = customSkin.FindStyle("UIM Foldout");

            // UIM Header
            GUILayout.Space(8);
            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("UIM Header"));
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Colors
            var windowBGColorDark = serializedObject.FindProperty("windowBGColorDark");
            var backgroundColorDark = serializedObject.FindProperty("backgroundColorDark");
            var primaryColorDark = serializedObject.FindProperty("primaryColorDark");
            var secondaryColorDark = serializedObject.FindProperty("secondaryColorDark");
            var highlightedColorDark = serializedObject.FindProperty("highlightedColorDark");
            var highlightedColorSecondaryDark = serializedObject.FindProperty("highlightedColorSecondaryDark");
            var taskBarColorDark = serializedObject.FindProperty("taskBarColorDark");
            var highlightedColorCustom = serializedObject.FindProperty("highlightedColorCustom");
            var highlightedColorSecondaryCustom = serializedObject.FindProperty("highlightedColorSecondaryCustom");

            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showColors = EditorGUILayout.Foldout(showColors, "Colors", true, foldoutStyle);
            showColors = GUILayout.Toggle(showColors, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showColors)
            {
                GUILayout.Label("System Theme", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Accent Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(highlightedColorDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Accent Reversed"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(highlightedColorSecondaryDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Primary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(primaryColorDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Secondary Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(secondaryColorDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Window BG Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(windowBGColorDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Background Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(backgroundColorDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Task Bar Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(taskBarColorDark, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.Space(12);
                GUILayout.Label("Custom Theme", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Accent Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(highlightedColorCustom, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Accent Reversed Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(highlightedColorSecondaryCustom, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
            GUILayout.Space(foldoutItemSpace);
            GUILayout.BeginVertical(EditorStyles.helpBox);

            // Fonts
            var systemFontThin = serializedObject.FindProperty("systemFontThin");
            var systemFontLight = serializedObject.FindProperty("systemFontLight");
            var systemFontRegular = serializedObject.FindProperty("systemFontRegular");
            var systemFontSemiBold = serializedObject.FindProperty("systemFontSemiBold");
            var systemFontBold = serializedObject.FindProperty("systemFontBold");

            GUILayout.Space(foldoutTopSpace);
            GUILayout.BeginHorizontal();
            showFonts = EditorGUILayout.Foldout(showFonts, "Button", true, foldoutStyle);
            showFonts = GUILayout.Toggle(showFonts, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));
            GUILayout.EndHorizontal();
            GUILayout.Space(foldoutBottomSpace);

            if (showFonts)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font Thin"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(systemFontThin, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font Light"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(systemFontLight, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font Regular"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(systemFontRegular, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font Semibold"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(systemFontSemiBold, new GUIContent(""));

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(EditorStyles.helpBox);

                EditorGUILayout.LabelField(new GUIContent("Font Bold"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                EditorGUILayout.PropertyField(systemFontBold, new GUIContent(""));

                GUILayout.EndHorizontal();
            }

            // Settings
            GUILayout.EndVertical();
            GUILayout.Space(14);
            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Options Header"));

            var enableDynamicUpdate = serializedObject.FindProperty("enableDynamicUpdate");

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            enableDynamicUpdate.boolValue = GUILayout.Toggle(enableDynamicUpdate.boolValue, new GUIContent("Update Values"), customSkin.FindStyle("Toggle"));
            enableDynamicUpdate.boolValue = GUILayout.Toggle(enableDynamicUpdate.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();

            var enableExtendedColorPicker = serializedObject.FindProperty("enableExtendedColorPicker");

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            enableExtendedColorPicker.boolValue = GUILayout.Toggle(enableExtendedColorPicker.boolValue, new GUIContent("Extended Color Picker"), customSkin.FindStyle("Toggle"));
            enableExtendedColorPicker.boolValue = GUILayout.Toggle(enableExtendedColorPicker.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();

            if (enableExtendedColorPicker.boolValue == true) { EditorPrefs.SetInt("UIManager.EnableExtendedColorPicker", 1); }
            else { EditorPrefs.SetInt("UIManager.EnableExtendedColorPicker", 0); }

            var editorHints = serializedObject.FindProperty("editorHints");

            GUILayout.BeginVertical(EditorStyles.helpBox);

            editorHints.boolValue = GUILayout.Toggle(editorHints.boolValue, new GUIContent("Theme Manager Hints"), customSkin.FindStyle("Toggle"));

            if (editorHints.boolValue == true)
            {
                EditorGUILayout.HelpBox("These values are universal and affect all objects containing 'Theme Manager' component.", MessageType.Info);
                EditorGUILayout.HelpBox("If want to assign unique values, remove 'Theme Manager Element' component from the object.", MessageType.Info);
            }

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
            Repaint();

            GUILayout.Space(12);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Reset to defaults", customSkin.button))
                ResetToDefaults();

            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // Support
            GUILayout.Space(14);
            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Support Header"));
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Need help? Contact me via:", customSkin.FindStyle("Text"));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Discord", customSkin.button)) { Discord(); }
            if (GUILayout.Button("Twitter", customSkin.button)) { Twitter(); }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("E-mail", customSkin.button)) { Email(); }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("ID: " + buildID);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(6);
        }

        void Discord() { Application.OpenURL("https://discord.gg/VXpHyUt"); }
        void Email() { Application.OpenURL("https://www.michsky.com/contact/"); }
        void Twitter() { Application.OpenURL("https://www.twitter.com/michskyHQ"); }

        void ResetToDefaults()
        {
            if (EditorUtility.DisplayDialog("Reset to defaults", "Are you sure you want to reset Theme Manager values to default?", "Yes", "Cancel"))
            {
                try
                {
                    if (EditorPrefs.HasKey("DreamOS.PipelineUpgrader"))
                    {
                        Preset defaultPreset = Resources.Load<Preset>("Theme Manager Presets/SRP Default");
                        defaultPreset.ApplyTo(Resources.Load("Theme/Theme Manager"));
                    }

                    else
                    {
                        Preset defaultPreset = Resources.Load<Preset>("Theme Manager Presets/Default");
                        defaultPreset.ApplyTo(Resources.Load("Theme/Theme Manager"));
                    }

                    Selection.activeObject = null;
                    Debug.Log("<b>[Theme Manager]</b> Resetting successful.");
                }

                catch { Debug.LogWarning("<b>[Theme Manager]</b> Resetting failed. Default preset seems to be missing."); }
            }
        }
    }
}
#endif