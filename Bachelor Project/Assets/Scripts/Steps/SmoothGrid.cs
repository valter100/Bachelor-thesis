using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothGrid : Step
{
    public override void DoAction()
    {
        base.DoAction();

        grid.SmoothGrid();
    }
}
