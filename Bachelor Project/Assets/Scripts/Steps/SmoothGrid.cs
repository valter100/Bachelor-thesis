using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothGrid : Step
{
    protected override void SetText()
    {
        question = "Let's get that terrain real smoooooth";
        optOne = "I like it the way it is!";
        optTwo = "Yes Clappy make it smooth";
        optThree = "Actually, i like it rougher!";
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
        grid.SmoothGrid();
    }
}
