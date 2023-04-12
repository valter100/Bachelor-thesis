using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothGrid : Step
{
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
