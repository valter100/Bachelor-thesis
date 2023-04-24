using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferenceHandler : MonoBehaviour
{
    TextHandler textHandler;

    [SerializeField] public int desert, forest, sea;
    [SerializeField] public int mapSizeXPref, mapSizeZPref, numberOfMapSizePref;
    [SerializeField] public int peakHeightPref, peakHeightRangePref, peakAmountPref, numberOfPeakPref;

    int setMapSizeX, setMapSizeZ;


    void Start()
    {
        textHandler = gameObject.GetComponent<TextHandler>();
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

    private void CalculateClosestSizeStep()
    {
        if (mapSizeXPref > 0 && mapSizeXPref < 25) setMapSizeX = 16;
        else if (mapSizeXPref > 0 && mapSizeXPref < 25) setMapSizeX = 32;
        else if (mapSizeXPref > 0 && mapSizeXPref < 25) setMapSizeX = 32;
        else if (mapSizeXPref > 0 && mapSizeXPref < 25) setMapSizeX = 32;
        setMapSizeZ = setMapSizeX;
    }
}
