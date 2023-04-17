using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : Step
{
    protected override void SetText()
    {
        question = "Well that looks cool! Do you like it or do you want a new one?";
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
        if (actionIndex == 0)
        {

        }
        if (actionIndex == 1)
        {

        }
        if (actionIndex == 2)
        {

        }
        CreateNewGrid();
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
}
