using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceHandler : MonoBehaviour
{
    TextHandler textHandler;

    [SerializeField] public int desertPercenteagePref, forestPerentagePref, seaPercentagePref;
    [SerializeField] public int mapSizeXPref, mapSizeZPref, numberOfMapSizePref;
    [SerializeField] public int peakHeightPref, peakHeightRangePref, peakAmountPref, numberOfPeakPref;

    void Start()
    {
        textHandler = gameObject.GetComponent<TextHandler>();
        LoadPreferences();
    }


    private void LoadPreferences()
    {
        numberOfMapSizePref = textHandler.GetPreferenceAmount("mapSizeX");
        if (numberOfMapSizePref > 0) mapSizeXPref = textHandler.GetPreferences("mapSizeX") / numberOfMapSizePref;
        if (numberOfMapSizePref > 0) mapSizeZPref = textHandler.GetPreferences("mapSizeZ") / numberOfMapSizePref;


        numberOfPeakPref = textHandler.GetPreferenceAmount("peakAmount");
        if (numberOfPeakPref > 0) peakHeightPref = textHandler.GetPreferences("peakHeight");
        if (numberOfPeakPref > 0) peakHeightRangePref = textHandler.GetPreferences("peakHeightRange");
        if (numberOfPeakPref > 0) peakAmountPref = textHandler.GetPreferences("peakAmount");


        if (textHandler.GetPreferenceAmount("desert") > 0) desertPercenteagePref = textHandler.GetPreferences("desert") / textHandler.GetPreferenceAmount("desert");
        if (textHandler.GetPreferenceAmount("forest") > 0) forestPerentagePref = textHandler.GetPreferences("forest") / textHandler.GetPreferenceAmount("forest");
        if (textHandler.GetPreferenceAmount("sea") > 0) seaPercentagePref = textHandler.GetPreferences("sea") / textHandler.GetPreferenceAmount("sea");
    }
}
