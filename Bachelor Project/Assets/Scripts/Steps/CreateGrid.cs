using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : Step
{

    [SerializeField] GameObject areYouSureWindow;

    protected override void SetText()
    {
        question = "Well that looks cool! Do you like it or do you want me to create a new one for you?";
        optOne = "New one please!";
        optTwo = "Keep it!";
        optThree = "Leave me alone... Creep!!";
        base.SetText();
    }


    public override void GiveTip()
    {
        base.GiveTip();
    }

    public override void DoAction(int actionIndex)
    {
        if (grid.Locked())
            return;

        if (actionIndex == 0)
        {
            CreateNewGridWithPreferences();
        }
        if (actionIndex == 1)
        {

        }
        if (actionIndex == 2)
        {
            clappy.SetInactive();
        }
    }


    public void CreateNewGrid()
    {
        if (grid.GetBaseGrid() != null)
        {
            areYouSureWindow.SetActive(true);
            return;
        }

        clappy.setNextStep();
        FindObjectOfType<CreateSubgrid>().SetUIActive(true);
        grid.CreateGrid();

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
