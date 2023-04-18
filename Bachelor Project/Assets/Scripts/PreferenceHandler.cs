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
        textHandler = gameObject.GetComponent<TextHandler>();
        LoadPreferences();
    }
    

    void LoadPreferences()
    {
        mapSizeXPref = textHandler.GetPreferences("mapSizeX");
        mapSizeZPref = textHandler.GetPreferences("mapSizeZ");
        numberOfMapSizePref = textHandler.GetPreferenceAmount("mapSizeX");

        peakHeightPref = textHandler.GetPreferences("peakHeight");
        peakHeightRangePref = textHandler.GetPreferences("peakHeightRange");
        peakAmountPref = textHandler.GetPreferences("peakAmount");
        numberOfPeakPref = textHandler.GetPreferenceAmount("peakAmount");

        desert = textHandler.GetPreferenceAmount("desert");
        forest = textHandler.GetPreferenceAmount("forest");
        sea = textHandler.GetPreferenceAmount("sea");
    }
}
