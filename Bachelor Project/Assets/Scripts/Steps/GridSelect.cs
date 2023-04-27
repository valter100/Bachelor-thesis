using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridSelect : Step
{
    SetBiome setBiome;
    HandleObjects handleObjects;

    private void Start()
    {
        base.Start();
        setBiome = FindObjectOfType<SetBiome>();
        handleObjects = FindObjectOfType<HandleObjects>();
    }

    protected override void SetText()
    {
        question = "I see you selected a subgrid, want me to help you with it?";
        optOne = "Make it Desert";
        optTwo = "Make it Sea";
        optThree = "Make it Forest";
        base.SetText();
    }
    public override void GiveTip()
    {
        base.GiveTip();
    }

    public override void DoAction(int actionIndex)
    {
        int totalTiles = 0;
        foreach (Tile[,] grid in grid.SelectedGrids())
        {
            totalTiles += grid.Length;
        }

        int selectedGridPercentage = (int)((float)totalTiles * 100 / ((float)grid.MapDimensionX() * (float)grid.MapDimensionZ()));
        

        if (actionIndex == 0)
        {
            textHandler.SavePreferenses("desert" + selectedGridPercentage.ToString());
            setBiome.ChangeBiome(1);
            setBiome.SetBiomeName(0);
        }
        if (actionIndex == 1)
        {
            textHandler.SavePreferenses("sea" + selectedGridPercentage.ToString());
            setBiome.ChangeBiome(2);
            setBiome.SetBiomeName(1);
        }
        if (actionIndex == 2)
        {
            textHandler.SavePreferenses("forest" + selectedGridPercentage.ToString());
            setBiome.ChangeBiomeNoHeight(3);
            setBiome.SetBiomeName(2);
        }

        clappy.SetStep(3);
        handleObjects.PlaceObjectsAction();
        grid.CalculateSubgridTypes();
    }
}
