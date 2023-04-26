using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateGrid : Step
{
    [SerializeField] GameObject areYouSureWindow;
    [SerializeField] GameObject gridSizeWindow;
    [SerializeField] TMP_InputField xInput;
    [SerializeField] TMP_InputField zInput;
    [SerializeField] TMP_InputField peakAmountInput;
    [SerializeField] TMP_InputField peakHeightInput;
    [SerializeField] TMP_InputField peakheightRangeInput;

    protected override void SetText()
    {
        question = "Well that looks cool! Do you like it or do you want me to createa new one for you?";
        optOne = "New one please!";
        optTwo = "Keep it!";
        optThree = "Leave me alone... Creep!!";
        base.SetText();
    }


    public override void GiveTip()
    {
        base.GiveTip();
    }

    public void ActivateSizeWindow()
    {
        xInput.text = preferenceHandler.mapSizeXPref.ToString();
        zInput.text = preferenceHandler.mapSizeZPref.ToString();
        peakAmountInput.text = preferenceHandler.peakAmountPref.ToString();
        peakHeightInput.text = preferenceHandler.peakHeightPref.ToString();
        peakheightRangeInput.text = preferenceHandler.peakHeightRangePref.ToString();

        gridSizeWindow.SetActive(true);
    }
    public override void DoAction(int actionIndex)
    {
        if (grid.Locked())
            return;

        if (actionIndex == 0)
        {
            CreateNewGridWithPreferences();
            return;
        }
        if (actionIndex == 1)
        {
            return;
        }
        if (actionIndex == 2)
        {
            clappy.SetInactive();
            return;
        }

        if (grid.GetBaseGrid() != null)
        {
            areYouSureWindow.SetActive(true);
            return;
        }

        ActivateSizeWindow();
    }

    public void CreateNewGrid()
    {
        //clappy.setNextStep();
        SetUIActive(true);
        FindObjectOfType<CreateSubgrid>().SetUIActive(true);
        grid.CreateGrid(
            int.Parse(xInput.text), 
            int.Parse(zInput.text),
            int.Parse(peakHeightInput.text),
            int.Parse(peakheightRangeInput.text),
            int.Parse(peakAmountInput.text)
           );

        textHandler.SavePreferenses("mapSizeX" + grid.MapDimensionX().ToString());
        textHandler.SavePreferenses("mapSizeZ" + grid.MapDimensionZ().ToString());

        textHandler.SavePreferenses("peakHeight" + grid.PeakHeight().ToString());
        textHandler.SavePreferenses("peakHeightRange" + grid.PeakHeightRange().ToString());
        textHandler.SavePreferenses("peakAmount" + grid.PeakAmount().ToString());
    }

    public void CreateNewGridWithPreferences()
    {
        int mapSizeX = preferenceHandler.mapSizeXPref;
        int mapSizeZ = preferenceHandler.mapSizeZPref;

        int peakHeight = preferenceHandler.peakHeightPref;
        int peakHeightRange = preferenceHandler.peakHeightRangePref;
        int peakAmount = preferenceHandler.peakAmountPref;

        grid.CreateGrid(mapSizeX, mapSizeZ, peakHeight, peakHeightRange, peakAmount);
    }
}
