#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Michsky.DreamOS
{
    public class CreateMenu : Editor
    {
        static string objectPath;

        static void GetObjectPath()
        {
            objectPath = AssetDatabase.GetAssetPath(Resources.Load("Theme/Theme Manager"));
            objectPath = objectPath.Replace("Resources/Theme/Theme Manager.asset", "").Trim();
            objectPath = objectPath + "Prefabs/";
        }

        static void MakeSceneDirty(GameObject source, string sourceName)
        {
            if (Application.isPlaying == false)
            {
                Undo.RegisterCreatedObjectUndo(source, sourceName);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

        static void ShowErrorDialog()
        {
            EditorUtility.DisplayDialog("DreamOS", "Cannot create the object due to missing manager file. " +
                    "Make sure you have 'Theme Manager' file in DreamOS > Resources > Theme folder.", "Okay");
        }

        static void UpdateCustomEditorPath()
        {
            string mainPath = AssetDatabase.GetAssetPath(Resources.Load("Theme Manager"));
            mainPath = mainPath.Replace("Resources/Theme/Theme Manager.asset", "").Trim();
            string darkPath = mainPath + "Editor/Glass Skin Dark.guiskin";
            string lightPath = mainPath + "Editor/Glass Skin Light.guiskin";

            EditorPrefs.SetString("DreamOS.CustomEditorDark", darkPath);
            EditorPrefs.SetString("DreamOS.CustomEditorLight", lightPath);
        }

        static void CreateObject(string resourcePath)
        {
            try
            {
                GetObjectPath();
                UpdateCustomEditorPath();
                GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath(objectPath + resourcePath + ".prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;

                try
                {
                    if (Selection.activeGameObject == null)
                    {
                        var canvas = (Canvas)GameObject.FindObjectsOfType(typeof(Canvas))[0];
                        clone.transform.SetParent(canvas.transform, false);
                    }

                    else { clone.transform.SetParent(Selection.activeGameObject.transform, false); }

                    clone.name = clone.name.Replace("(Clone)", "").Trim();
                    MakeSceneDirty(clone, clone.name);
                }

                catch
                {
                    CreateCanvas();
                    var canvas = (Canvas)GameObject.FindObjectsOfType(typeof(Canvas))[0];
                    clone.transform.SetParent(canvas.transform, false);
                    clone.name = clone.name.Replace("(Clone)", "").Trim();
                    MakeSceneDirty(clone, clone.name);
                }

                Selection.activeObject = clone;
            }

            catch { ShowErrorDialog(); }
        }

        [MenuItem("GameObject/DreamOS/Canvas", false, -1)]
        static void CreateCanvas()
        {
            try
            {
                GetObjectPath();
                UpdateCustomEditorPath();
                GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath(objectPath + "UI Elements/Other/Canvas.prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                clone.name = clone.name.Replace("(Clone)", "").Trim();
                Selection.activeObject = clone;
                MakeSceneDirty(clone, clone.name);
            }

            catch { ShowErrorDialog(); }
        }

        [MenuItem("Tools/DreamOS/Create World Space Resources", false, 12)]
        static void CreateWorldSpaceResources()
        {
            try
            {
                GetObjectPath();
                GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath(objectPath + "World Space/World Space Resources.prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                Undo.RegisterCreatedObjectUndo(clone, "Created an object");
                clone.name = clone.name.Replace("(Clone)", "").Trim();

                if (Application.isPlaying == false) { EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene()); }
            }

            catch { ShowErrorDialog(); }
        }

        static void ShowManager()
        {
            Selection.activeObject = Resources.Load("Theme/Theme Manager");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Theme Manager'. Make sure you have 'Theme Manager' asset in Resources folder. " +
                    "You can create a new Theme Manager asset or re-import the pack if you can't see the file.");
        }

        [MenuItem("Tools/DreamOS/Show Theme Manager")]
        static void ShowManagerTools()
        {
            Selection.activeObject = Resources.Load("Theme/Theme Manager");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Theme Manager'. Make sure you have 'Theme Manager' asset in Resources folder. " +
                    "You can create a new Theme Manager asset or re-import the pack if you can't see the file.");
        }

        [MenuItem("Tools/DreamOS/Show Chat List")]
        static void ShowChatList()
        {
            Selection.activeObject = Resources.Load("Chats/Example Chat");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Example Chat'. Make sure you have 'Example Chat' asset in Resources/Chats folder.");
        }

        [MenuItem("Tools/DreamOS/Show Game Hub List")]
        static void ShowGameHubList()
        {
            Selection.activeObject = Resources.Load("Game Hub/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Game Hub folder.");
        }

        [MenuItem("Tools/DreamOS/Show Music Playlists")]
        static void ShowMusicLibrary()
        {
            Selection.activeObject = Resources.Load("Music Player/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Music Player folder.");
        }

        [MenuItem("Tools/DreamOS/Show Notepad Library")]
        static void ShowNotepadLibrary()
        {
            Selection.activeObject = Resources.Load("Notepad/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Notepad folder.");
        }

        [MenuItem("Tools/DreamOS/Show Photo Library")]
        static void ShowPhotoLibrary()
        {
            Selection.activeObject = Resources.Load("Photo Gallery/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Gallery folder.");
        }

        [MenuItem("Tools/DreamOS/Show Video Library")]
        static void ShowVideoLibrary()
        {
            Selection.activeObject = Resources.Load("Video Player/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Video Player folder.");
        }

        [MenuItem("Tools/DreamOS/Show Web Library")]
        static void ShowWebLibrary()
        {
            Selection.activeObject = Resources.Load("Web Browser/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Web Browser folder.");
        }

        [MenuItem("Tools/DreamOS/Show Widget Library")]
        static void ShowWidgetLibrary()
        {
            Selection.activeObject = Resources.Load("Widgets/Library");

            if (Selection.activeObject == null)
                Debug.Log("Can't find an asset named 'Library'. Make sure you have 'Library' asset in Resources/Widgets folder.");
        }

        #region Button
        [MenuItem("GameObject/DreamOS/Button/Desktop Button", false, 0)]
        static void DesktopButton()
        {
            CreateObject("UI Elements/Button/Desktop Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Main Button", false, 0)]
        static void MainButton()
        {
            CreateObject("UI Elements/Button/Main Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Main Button (Only Icon)", false, 0)]
        static void MainButtonOnlyIcon()
        {
            CreateObject("UI Elements/Button/Main Button (Only Icon)");
        }

        [MenuItem("GameObject/DreamOS/Button/Main Button (With Icon)", false, 0)]
        static void MainButtonWithIcon()
        {
            CreateObject("UI Elements/Button/Main Button (With Icon)");
        }

        [MenuItem("GameObject/DreamOS/Button/Nav Drawer Button", false, 0)]
        static void NavDrawerButton()
        {
            CreateObject("UI Elements/Button/Nav Drawer Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Picture Selection Button", false, 0)]
        static void PictureSelectionButton()
        {
            CreateObject("UI Elements/Button/Picture Selection Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Quick Center App Button", false, 0)]
        static void QuickCenterAppButton()
        {
            CreateObject("UI Elements/Button/Quick Center App Button");
        }

        [MenuItem("GameObject/DreamOS/Button/Task Bar Button", false, 0)]
        static void TaskBarButton()
        {
            CreateObject("UI Elements/Button/Task Bar Button");
        }
        #endregion

        #region Horizontal Selector
        [MenuItem("GameObject/DreamOS/Horizontal Selector/Standard", false, 0)]
        static void HorizontalSelector()
        {
            CreateObject("UI Elements/Horizontal Selector/Horizontal Selector");
        }
        #endregion

        #region Input Field
        [MenuItem("GameObject/DreamOS/Input Field/Fading Input Field", false, 0)]
        static void FadingInputField()
        {
            CreateObject("UI Elements/Input Field/Fading Input Field");
        }

        [MenuItem("GameObject/DreamOS/Input Field/Standard Input Field", false, 0)]
        static void StandardInputField()
        {
            CreateObject("UI Elements/Input Field/Standard Input Field");
        }
        #endregion

        #region Loader
        [MenuItem("GameObject/DreamOS/Loader/Material Spinner", false, 0)]
        static void LoaderMaterial()
        {
            CreateObject("UI Elements/Loader/Material Spinner");
        }
        #endregion

        #region Modal Window
        [MenuItem("GameObject/DreamOS/Modal Window/Standard", false, 0)]
        static void ModalWindow()
        {
            CreateObject("UI Elements/Modal Window/Standard Modal Window");
        }
        #endregion

        #region Scrollbar
        [MenuItem("GameObject/DreamOS/Scrollbar/Standard", false, 0)]
        static void Scrollbar()
        {
            CreateObject("UI Elements/Scrollbar/Scrollbar");
        }
        #endregion

        #region Slider
        [MenuItem("GameObject/DreamOS/Slider/Standard", false, 0)]
        static void Slider()
        {
            CreateObject("UI Elements/Slider/Slider");
        }
        #endregion

        #region Switch
        [MenuItem("GameObject/DreamOS/Switch/Standard", false, 0)]
        static void Switch()
        {
            CreateObject("UI Elements/Switch/Switch");
        }
        #endregion

        #region Vertical Selector
        [MenuItem("GameObject/DreamOS/Vertical Selector/Standard", false, 0)]
        static void VerticalSelector()
        {
            CreateObject("UI Elements/Vertical Selector/Vertical Selector");
        }
        #endregion
    }
}
#endif