using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTools : Step
{
    [SerializeField] GameObject tool;

    public void CreateTool()
    {
        GameObject[] tools = GameObject.FindGameObjectsWithTag("Tool");

        for(int i = 0; i < tools.Length; i++)
        {
            Destroy(tools[i]);
        }

        StartStep();

        Instantiate(tool);
    }
}
