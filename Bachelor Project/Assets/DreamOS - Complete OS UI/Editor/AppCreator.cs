using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

namespace Michsky.DreamOS
{
    public class AppCreator : EditorWindow
    {
        static AppCreator window;
        protected Texture2D bannerTexture;
        protected GUIStyle panelStyle;
        static string objectPath;

        // Editor variables
        public GameObject emptyApp;
        public GameObject emptyAppWithTabs;
        public GameObject desktopButton;
        public GameObject taskbarShortcut;
        public bool enableNavbar = true;
        public bool enableBlurManager = true;
        public bool createDesktopShortcut = true;
        public bool createTaskbarButton = true;

        // App variables
        public Sprite appIcon;
        public string appName = "App Name";
        public Transform appParent;
        public Transform shortcutParent;
        public Transform taskbarParent;

        [MenuItem("Tools/DreamOS/App Creator", false, 0)]
        public static void ShowWindow()
        {
            window = GetWindow<AppCreator>("DreamOS - App Creator");
            window.minSize = new Vector2(560, 534);
            window.maxSize = new Vector2(560, 534);
        }

        static void ShowErrorDialog()
        {
            EditorUtility.DisplayDialog("DreamOS", "Cannot create the object due to missing manager file. " +
                    "Make sure you have 'Theme Manager' file in DreamOS > Resources > Theme folder.", "Okay");
        }

        static void GetObjectPath()
        {
            objectPath = AssetDatabase.GetAssetPath(Resources.Load("Theme/Theme Manager"));
            objectPath = objectPath.Replace("Resources/Theme/Theme Manager.asset", "").Trim();
            objectPath = objectPath + "Prefabs/";
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

        void OnEnable()
        {
            bannerTexture = (Texture2D)Resources.Load("Editor/DAC Banner", typeof(Texture2D));
        }

        void OnGUI()
        {
            // Custom skin
            GUISkin customSkin;

            if (EditorGUIUtility.isProSkin == true)
                customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Dark");
            else
                customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Light");

            // Custom panel
            panelStyle = new GUIStyle(GUI.skin.box);
            panelStyle.normal.textColor = GUI.skin.label.normal.textColor;
            panelStyle.margin = new RectOffset(15, 15, 0, 15);
            panelStyle.padding = new RectOffset(12, 12, 12, 12);

            // Banner
            GUILayout.BeginHorizontal();
            GUILayout.Label(bannerTexture, GUILayout.Width(428), GUILayout.Height(90));

            GUILayout.BeginVertical();
            GUILayout.Space(56);

            if (GUILayout.Button("Documentation", GUILayout.Width(110), GUILayout.Height(24)))
                Application.OpenURL("https://docs.michsky.com/docs/dreamos/");

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            // Panel
            EditorGUILayout.BeginVertical(panelStyle);

            // App Information
            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("DAC Info"));
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(4);
            GUILayout.BeginHorizontal();
            GUILayout.Space(4);

            appIcon = (Sprite)EditorGUILayout.ObjectField(appIcon, typeof(Sprite), true, GUILayout.Width(65), GUILayout.Height(65));

            GUILayout.Space(4);
            GUILayout.BeginVertical();

            EditorGUILayout.LabelField(new GUIContent("App icon and name"), customSkin.FindStyle("Text"), GUILayout.Width(120), GUILayout.Height(20));
            appName = (string)EditorGUILayout.TextField(appName, GUILayout.Height(40));

            GUILayout.EndVertical();
            GUILayout.Space(6);
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
            GUILayout.EndVertical();
            GUILayout.Space(10);

            // Settings
            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("DAC Settings"));
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            enableNavbar = GUILayout.Toggle(enableNavbar, new GUIContent("Enable Navigation Bar"), customSkin.FindStyle("Toggle"));
            enableNavbar = GUILayout.Toggle(enableNavbar, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            enableBlurManager = GUILayout.Toggle(enableBlurManager, new GUIContent("Enable Blur Effects"), customSkin.FindStyle("Toggle"));
            enableBlurManager = GUILayout.Toggle(enableBlurManager, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            createDesktopShortcut = GUILayout.Toggle(createDesktopShortcut, new GUIContent("Create Desktop Shortcut"), customSkin.FindStyle("Toggle"));
            createDesktopShortcut = GUILayout.Toggle(createDesktopShortcut, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            createTaskbarButton = GUILayout.Toggle(createTaskbarButton, new GUIContent("Create Taskbar Button"), customSkin.FindStyle("Toggle"));
            createTaskbarButton = GUILayout.Toggle(createTaskbarButton, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent("App Parent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
            appParent = (Transform)EditorGUILayout.ObjectField(appParent, typeof(Transform), true);
            
            GUILayout.Space(4);

            if (GUILayout.Button("Find Automatically", GUILayout.Width(140)))
            {
                try { appParent = GameObject.Find("Apps & Windows").transform; }
                catch { }
            }

            GUILayout.EndHorizontal();

            if (createDesktopShortcut == false)
                GUI.enabled = false;

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent("Shortcut Parent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
            shortcutParent = (Transform)EditorGUILayout.ObjectField(shortcutParent, typeof(Transform), true);
           
            GUILayout.Space(4);

            if (GUILayout.Button("Find Automatically", GUILayout.Width(140)))
            {
                try { shortcutParent = GameObject.Find("Desktop List").transform; }
                catch { }
            }

            GUILayout.EndHorizontal();
            GUI.enabled = true;

            if (createTaskbarButton == false)
                GUI.enabled = false;

            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent("Taskbar Parent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
            taskbarParent = (Transform)EditorGUILayout.ObjectField(taskbarParent, typeof(Transform), true);
            
            GUILayout.Space(4);

            if (GUILayout.Button("Find Automatically", GUILayout.Width(140)))
            {
                try { taskbarParent = GameObject.Find("Task Bar/Shortcut List").transform; }
                catch { }
            }

            GUILayout.EndHorizontal();
            GUI.enabled = true;

            // Status
            GUILayout.Space(26);

            if (appParent == null || createDesktopShortcut == true && shortcutParent == null || createTaskbarButton == true && taskbarParent == null)
                GUILayout.Box(new GUIContent(""), customSkin.FindStyle("DAC Status Warning"));
            else if (appParent != null || createDesktopShortcut == true && shortcutParent != null || createTaskbarButton != true && taskbarParent == null)
                GUILayout.Box(new GUIContent(""), customSkin.FindStyle("DAC Status OK"));

            GUILayout.BeginVertical();
            GUILayout.Space(-40);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (appParent == null)
                GUI.enabled = false;

            if (createDesktopShortcut == true && shortcutParent == null)
                GUI.enabled = false;

            if (createTaskbarButton == true && taskbarParent == null)
                GUI.enabled = false;

            if (GUILayout.Button("", customSkin.FindStyle("Create App")))
            {
                GetObjectPath();
                if (enableNavbar == true) { CreateAppWithNavbar(); }
                else { CreateAppWithoutNavbar(); }
            }

            if (GUI.enabled == true)
                Repaint();

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }

        void CreateAppWithNavbar()
        {
            GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath(objectPath + "Tools/Empty App With Navbar"
               + ".prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
            clone.name = appName;
            clone.transform.SetParent(appParent, false);

            Image headerImage = clone.transform.Find("Content/Navigation Panel/Header/Icon").GetComponent<Image>();
            headerImage.sprite = appIcon;

            TextMeshProUGUI headerText = clone.transform.Find("Content/Navigation Panel/Header/Text").GetComponent<TextMeshProUGUI>();
            headerText.text = appName;

            NavDrawerExpandButton nde = clone.transform.Find("Content/Navigation Panel/Expand & Minimize").GetComponent<NavDrawerExpandButton>();
            nde.appName = appName;

            WindowManager wManager = clone.GetComponent<WindowManager>();
            CreateDesktopButton(wManager);
            CreateTaskbarButton(wManager);

            if (enableBlurManager == false)
                wManager.enableBackgroundBlur = false;

            CreateBlurMaterial(clone);

            Selection.activeObject = clone;
        }

        void CreateAppWithoutNavbar()
        {
            GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath(objectPath + "Tools/Empty App"
               + ".prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
            clone.name = appName;
            clone.transform.SetParent(appParent, false);

            Image headerImage = clone.transform.Find("Content/App Icon").GetComponent<Image>();
            headerImage.sprite = appIcon;

            TextMeshProUGUI headerText = clone.transform.Find("Content/App Title").GetComponent<TextMeshProUGUI>();
            headerText.text = appName;

            WindowManager wManager = clone.GetComponent<WindowManager>();
            CreateDesktopButton(wManager);
            CreateTaskbarButton(wManager);

            if (enableBlurManager == false)
                wManager.enableBackgroundBlur = false;

            CreateBlurMaterial(clone);

            Selection.activeObject = clone;
        }

        void CreateDesktopButton(WindowManager wManager)
        {
            GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath(objectPath + "UI Elements/Button/Desktop Button"
            + ".prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
            clone.name = appName;
            clone.transform.SetParent(shortcutParent, false);

            Image iconObj = clone.transform.Find("Icon Background/Icon").GetComponent<Image>();
            iconObj.sprite = appIcon;

            TextMeshProUGUI textObj = clone.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            textObj.text = appName;

            DoubleClickEvent dce = clone.GetComponent<DoubleClickEvent>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(dce.doubleClickEvents, new UnityEngine.Events.UnityAction(wManager.OpenWindow));
        }

        void CreateTaskbarButton(WindowManager wManager)
        {
            GameObject clone = Instantiate(AssetDatabase.LoadAssetAtPath(objectPath + "UI Elements/Button/Task Bar Button"
            + ".prefab", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
            clone.name = appName;
            clone.transform.SetParent(taskbarParent, false);

            Image aIconObj = clone.transform.Find("Icon").GetComponent<Image>();
            aIconObj.sprite = appIcon;

            Image bIconObj = clone.transform.Find("Icon Active").GetComponent<Image>();
            bIconObj.sprite = appIcon;

            TaskBarButton tbb = clone.GetComponent<TaskBarButton>();
            tbb.buttonTitle = appName;
            UnityEditor.Events.UnityEventTools.AddPersistentListener(tbb.onClickEvents, new UnityEngine.Events.UnityAction(wManager.OpenWindow));

            wManager.taskBarButton = tbb.gameObject;
        }

        void CreateBlurMaterial(GameObject clone)
        {
            if (enableBlurManager == true && GraphicsSettings.renderPipelineAsset == null)
            {
                string matPath = objectPath.Replace("/Prefabs/", "/");
                AssetDatabase.CopyAsset(matPath + "Materials/Blur New App BG.mat", matPath + "Materials/Blur " + appName + " BG" + ".mat");

                BlurManager blurManager = clone.GetComponent<BlurManager>();
                blurManager.blurMaterial = (Material)AssetDatabase.LoadAssetAtPath(matPath + "Materials/Blur " + appName + " BG" + ".mat", typeof(Material));

                Image blurImage = clone.transform.Find("Blur BG").GetComponent<Image>();
                blurImage.material = blurManager.blurMaterial;
            }

            else
            {
                BlurManager blurManager = clone.GetComponent<BlurManager>();
                blurManager.blurMaterial = null;
                blurManager.enabled = false;
            }
        }
    }
}