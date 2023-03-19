using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTools : Step
{
    [SerializeField] GameObject tool;

    public void CreateTool()
    {
        StartStep();

        Instantiate(tool);
    }
}
