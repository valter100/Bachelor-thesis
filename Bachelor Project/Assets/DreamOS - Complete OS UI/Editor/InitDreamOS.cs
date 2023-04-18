using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine.Rendering;

public class InitGlassOS : MonoBehaviour
{
    [InitializeOnLoad]
	public class InitOnLoad
	{
		static InitOnLoad()
		{
			if (!EditorPrefs.HasKey("DreamOSv1.Installed"))
			{
				EditorPrefs.SetInt("DreamOSv1.Installed", 1);
				EditorUtility.DisplayDialog("Hello there!", "Thank you for purchasing DreamOS." +
					"\r\rIf you need help, feel free to contact us through our support channels or Discord.", "Got it");
			}

			if (!EditorPrefs.HasKey("DreamOS.HasCustomEditorData"))
			{
				EditorPrefs.SetInt("DreamOS.HasCustomEditorData", 1);

				string mainPath = AssetDatabase.GetAssetPath(Resources.Load("Theme/Theme Manager"));
				mainPath = mainPath.Replace("Resources/Theme/Theme Manager.asset", "").Trim();
				string darkPath = mainPath + "Editor/Glass Skin Dark.guiskin";
				string lightPath = mainPath + "Editor/Glass Skin Light.guiskin";

				EditorPrefs.SetString("DreamOS.CustomEditorDark", darkPath);
				EditorPrefs.SetString("DreamOS.CustomEditorLight", lightPath);
			}

			if (!EditorPrefs.HasKey("DreamOS.PipelineUpgrader") && GraphicsSettings.renderPipelineAsset != null)
			{
				EditorPrefs.SetInt("DreamOS.PipelineUpgrader", 1);
				
				if (EditorUtility.DisplayDialog("DreamOS SRP Upgrader", "It looks like your project is using URP/HDRP rendering pipeline, " +
					"would you like to upgrade DreamOS Theme Manager for your project?" +
					"\r\rNote that the blur shader is not currently compatible with URP/HDRP.", "Yes", "No"))
                {
					try
					{
						Preset defaultPreset = Resources.Load<Preset>("Theme Manager Presets/SRP Default");
						defaultPreset.ApplyTo(Resources.Load("Theme/Theme Manager"));
					}

					catch { Debug.LogWarning("<b>[DreamOS]</b> Something went wrong while loading the SRP preset."); }
				}
			}
		}
	}
}