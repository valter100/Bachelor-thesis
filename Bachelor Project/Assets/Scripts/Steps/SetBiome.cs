using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBiome : Step
{
    [SerializeField] List<Color> biomeColors = new List<Color>();
    [SerializeField] List<float> heightDifference = new List<float>();
    [SerializeField] List<bool> biomeImpassable = new List<bool>();

    string biomeName = "";

    private void Start()
    {
        base.Start();
    }

    protected override void SetText()
    {
        question = "Awesome! Now we have some cool " + biomeName + " tiles!";
        optOne = "Yeah i like them!!";
        optTwo = "Nah i liked it better before";
        optThree = "Meh... they are okay i guess..";
        base.SetText();
    }

    public void SetBiomeName(int index)
    {
        if (index == 0) biomeName = "Desert";
        if (index == 1) biomeName = "Sea";
        if (index == 2) biomeName = "Forest";
        SetText();
        GiveTip();
    }

    public override void GiveTip()
    {
        base.GiveTip();
    }

    public override void DoAction(int actionIndex)
    {
        if (actionIndex == 0)
        {
            //textHandler.
        }
        if (actionIndex == 1)
        {

        }
        if (actionIndex == 2)
        {
            
        }
    }

    public void ChangeBiome(int biomeIndex)
    {
        StartStep();

        if (biomeIndex == 2)
        {
            float waterHeight = CalculateWaterHeight();
            heightDifference[biomeIndex] = waterHeight - 0.5f;
        }

        foreach (Tile[,] subgrid in grid.SelectedGrids())
        {
            foreach (Tile tile in subgrid)
            {
                if (tile == null)
                    continue;

                if(tile.PlacedObject())
                    Destroy(tile.PlacedObject());

                tile.SetBiome(biomeIndex, true);
                tile.SetColor(biomeColors[biomeIndex]);
                tile.SetHeight(heightDifference[biomeIndex]);
                tile.SetImpassable(biomeImpassable[biomeIndex]);
            }
        }
    }

    public int CalculateWaterHeight()
    {
        float lowestHeight = float.MaxValue;

        foreach (Tile[,] subgrid in grid.SelectedGrids())
        {
            foreach (Tile tile in subgrid)
            {
                if (tile == null)
                    continue;

                if(tile.Height() < lowestHeight)
                {
                    lowestHeight = tile.Height();
                }
            }
        }

        return (int)lowestHeight;
    }

    public void ChangeBiomeOnSpecificGrid(Tile[,] subgrid, int biomeIndex)
    {
        foreach (Tile tile in subgrid)
        {
            if (tile == null)
                continue;

            if (tile.PlacedObject())
                Destroy(tile.PlacedObject());

            tile.SetBiome(biomeIndex, true);
            tile.SetColor(biomeColors[biomeIndex]);
            tile.SetHeight(heightDifference[biomeIndex]);
            tile.SetImpassable(biomeImpassable[biomeIndex]);
        }
    }

    public void ChangeBiomeNoHeight(int biomeIndex)
    {
        foreach (Tile[,] subgrid in grid.SelectedGrids())
        {
            foreach (Tile tile in subgrid)
            {
                if (tile == null)
                    continue;

                tile.SetBiome(biomeIndex, false);
                tile.SetColor(biomeColors[biomeIndex]);
                tile.SetImpassable(biomeImpassable[biomeIndex]);
            }
        }
    }

    public List<bool> GetImpassableList()
    {
        return biomeImpassable;
    }
}
