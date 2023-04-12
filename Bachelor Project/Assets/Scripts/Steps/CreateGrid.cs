using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : Step
{
    int[] mapSizePreferense = new int[3];
    int[] peakPreferenses = new int[4];

    private void Start()
    {
        LoadPreferences();
    }
    protected override void SetText()
    {
        question = "Well that looks cool! Do you like it or do you want me to create one for you?";
        optOne = "New one please!";
        optTwo = "Keep it!";
        optThree = "Leave me alone... Creep!!";
        base.SetText();
    }

    void LoadPreferences()
    {
        mapSizePreferense[0] = textHandler.GetPreferences("mapSizeX");
        mapSizePreferense[1] = textHandler.GetPreferences("mapSizeZ");
        mapSizePreferense[2] = textHandler.GetPreferenceAmount("mapSizeX");

        peakPreferenses[0] = textHandler.GetPreferences("peakHeight");
        peakPreferenses[1] = textHandler.GetPreferences("peakHeightRange");
        peakPreferenses[2] = textHandler.GetPreferences("peakAmount");
        peakPreferenses[3] = textHandler.GetPreferenceAmount("peakAmount");
    }

    public override void GiveTip()
    {
        base.GiveTip();
    }

    public override void DoAction(int actionIndex)
    {
        if (actionIndex == 0)
        {
            CreateNewGridWithPreferences();

            //Debug.Log("mapx " + mapSizeX);
            //Debug.Log("mapz " + mapSizeZ);
            //Debug.Log("peak height " + peakHeight);
            //Debug.Log("peak height range " + peakHeightRange);
            //Debug.Log("peak anount " + peakAmount);
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
        grid.CreateGrid();

        textHandler.SavePreferenses("mapSizeX" + grid.MapDimensionX().ToString());
        textHandler.SavePreferenses("mapSizeZ" + grid.MapDimensionZ().ToString());

        textHandler.SavePreferenses("peakHeight" + grid.PeakHeight().ToString());
        textHandler.SavePreferenses("peakHeightRange" + grid.PeakHeightRange().ToString());
        textHandler.SavePreferenses("peakAmount" + grid.PeakAmount().ToString());
    }

    public void CreateNewGridWithPreferences()
    {
        int mapSizeX = mapSizePreferense[0] / mapSizePreferense[2];
        int mapSizeZ = mapSizePreferense[1] / mapSizePreferense[2];

        int peakHeight = peakPreferenses[0] / peakPreferenses[3];
        int peakHeightRange = peakPreferenses[1] / peakPreferenses[3];
        int peakAmount = peakPreferenses[2] / peakPreferenses[3];

        grid.CreateGrid(mapSizeX, mapSizeZ, peakHeight, peakHeightRange, peakAmount);
    }
}
