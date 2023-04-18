#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(NotepadManager))]
    public class NotepadManagerEditor : Editor
    {
        private NotepadManager notepadTarget;
        private int currentTab;

        private void OnEnable()
        {
            notepadTarget = (NotepadManager)target;
        }

        public override void OnInspectorGUI()
        {
            GUISkin customSkin;
            Color defaultColor = GUI.color;

            if (EditorGUIUtility.isProSkin == true)
                customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Dark");
            else
                customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Light");

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = defaultColor;

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Notepad Top Header"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-42);

            GUIContent[] toolbarTabs = new GUIContent[2];
            toolbarTabs[0] = new GUIContent("Resources");
            toolbarTabs[1] = new GUIContent("Settings");

            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            currentTab = GUILayout.Toolbar(currentTab, toolbarTabs, customSkin.FindStyle("Tab Indicator"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-40);
            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 1;

            GUILayout.EndHorizontal();

            var libraryAsset = serializedObject.FindProperty("libraryAsset");
            var noteLibraryParent = serializedObject.FindProperty("noteLibraryParent");
            var noteLibraryButton = serializedObject.FindProperty("noteLibraryButton");
            var notepadWindow = serializedObject.FindProperty("notepadWindow");
            var viewerAnimator = serializedObject.FindProperty("viewerAnimator");
            var viewerTitle = serializedObject.FindProperty("viewerTitle");
            var viewerContent = serializedObject.FindProperty("viewerContent");
            var deleteButton = serializedObject.FindProperty("deleteButton");
            var sortListByName = serializedObject.FindProperty("sortListByName");
            var notepadStoring = serializedObject.FindProperty("notepadStoring");
            var saveCustomNotes = serializedObject.FindProperty("saveCustomNotes");

            switch (currentTab)
            {             
                case 0:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Core Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Library Asset"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(libraryAsset, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Library Parent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(noteLibraryParent, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Window"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(notepadWindow, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Viewer Animator"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(viewerAnimator, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Viewer Title"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(viewerTitle, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Viewer Content"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(viewerContent, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Delete Button"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(deleteButton, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    break;

                case 1:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Options Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    sortListByName.boolValue = GUILayout.Toggle(sortListByName.boolValue, new GUIContent("Sort Note List By Name"), customSkin.FindStyle("Toggle"));
                    sortListByName.boolValue = GUILayout.Toggle(sortListByName.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    saveCustomNotes.boolValue = GUILayout.Toggle(saveCustomNotes.boolValue, new GUIContent("Save Custom Notes"), customSkin.FindStyle("Toggle"));
                    saveCustomNotes.boolValue = GUILayout.Toggle(saveCustomNotes.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (saveCustomNotes.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Notepad Storing"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(notepadStoring, new GUIContent(""));

                        GUILayout.EndHorizontal();

                        if (notepadTarget.notepadStoring == null)
                        {
                            EditorGUILayout.HelpBox("'Save Custom Notes' is enabled but 'Notepad Storing' is not assigned. " +
                                "Please add and/or assign 'Notepad Storing' component.", MessageType.Error);

                            if (GUILayout.Button("+  Create Notepad Storing", customSkin.button))
                            {
                                NotepadStoring tempNS = notepadTarget.gameObject.AddComponent<NotepadStoring>();
                                notepadTarget.notepadStoring = tempNS;
                                tempNS.notepadManager = notepadTarget;
                            }
                        }
                    }

                    break;            
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif