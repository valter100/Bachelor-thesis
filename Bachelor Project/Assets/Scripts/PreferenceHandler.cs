using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceHandler : MonoBehaviour
{
    TextHandler textHandler;

    [SerializeField] public int desert, forest, sea;
    [SerializeField] public int mapSizeXPref, mapSizeZPref, numberOfMapSizePref;
    [SerializeField] public int peakHeightPref, peakHeightRangePref, peakAmountPref, numberOfPeakPref;

    void Start()
    {
        textHandler = FindObjectOfType<TextHandler>();
        LoadPreferences();
    }
    

    private void LoadPreferences()
    {
        numberOfMapSizePref = textHandler.GetPreferenceAmount("mapSizeX");
        mapSizeXPref = textHandler.GetPreferences("mapSizeX");
        mapSizeZPref = textHandler.GetPreferences("mapSizeZ");
        mapSizeXPref /= numberOfMapSizePref;
        mapSizeZPref /= numberOfMapSizePref;

        numberOfPeakPref = textHandler.GetPreferenceAmount("peakAmount");
        peakHeightPref = textHandler.GetPreferences("peakHeight");
        peakHeightPref /= numberOfPeakPref;
        peakHeightRangePref = textHandler.GetPreferences("peakHeightRange");
        peakHeightRangePref /= numberOfPeakPref;
        peakAmountPref = textHandler.GetPreferences("peakAmount");
        peakAmountPref /= numberOfPeakPref;
        

        desert = textHandler.GetPreferenceAmount("desert");
        forest = textHandler.GetPreferenceAmount("forest");
        sea = textHandler.GetPreferenceAmount("sea");
    }
}
