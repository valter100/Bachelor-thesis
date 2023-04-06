using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSelect : Step
{

    protected override void SetText()
    {
        question = "I see you selected a subgrid, want me to help you with it?";
        optOne = "Make it Desert";
        optTwo = "Make it Forest";
        optThree = "Make it Sea";
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
    }
}
