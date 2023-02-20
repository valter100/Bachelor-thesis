using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : Step
{
    public override void DoAction()
    {
        base.DoAction();

        grid.CreateGrid();
    }
}
