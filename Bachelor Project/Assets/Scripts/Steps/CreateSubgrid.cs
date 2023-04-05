using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSubgrid : Step
{

    protected override void SetText()
    {
        question = "Do you like subgrids just as much as I do?";
        optOne = "Sure do Clappy-o!";
        optTwo = "Pls shut up...";
        optThree = "Meh....";
        base.SetText();
    }
    public override void GiveTip()
    {
        base.GiveTip();
    }

    public override void DoAction(int actionIndex)
    {
        //grid.CreateSubgrid();
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
