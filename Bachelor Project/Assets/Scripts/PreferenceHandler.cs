using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceHandler : MonoBehaviour
{
    TextHandler textHandler;

    [SerializeField] public int desertPercentagePref, forestPercentagePref, seaPercentagePref;
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
        if (numberOfMapSizePref > 0) mapSizeXPref = textHandler.GetPreferences("mapSizeX") / numberOfMapSizePref;
        if (numberOfMapSizePref > 0) mapSizeZPref = textHandler.GetPreferences("mapSizeZ") / numberOfMapSizePref;


        numberOfPeakPref = textHandler.GetPreferenceAmount("peakAmount");
        if (numberOfPeakPref > 0) peakHeightPref = textHandler.GetPreferences("peakHeight") /numberOfPeakPref;
        if (numberOfPeakPref > 0) peakHeightRangePref = textHandler.GetPreferences("peakHeightRange") / numberOfPeakPref;
        if (numberOfPeakPref > 0) peakAmountPref = textHandler.GetPreferences("peakAmount") / numberOfPeakPref;


        if (textHandler.GetPreferenceAmount("desert") > 0) desertPercentagePref = textHandler.GetPreferences("desert") / textHandler.GetPreferenceAmount("desert");
        if (textHandler.GetPreferenceAmount("forest") > 0) forestPercentagePref = textHandler.GetPreferences("forest") / textHandler.GetPreferenceAmount("forest");
        if (textHandler.GetPreferenceAmount("sea") > 0) seaPercentagePref = textHandler.GetPreferences("sea") / textHandler.GetPreferenceAmount("sea");
    }
}
