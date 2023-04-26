using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBiome : Step
{
    [SerializeField] List<Color> biomeColors = new List<Color>();
    [SerializeField] List<float> heightDifference = new List<float>();
    [SerializeField] List<bool> biomeImpassable = new List<bool>();

    string biomeName = "";
    string extraQuestion = "";
    bool offerHelp = false;

    private void Start()
    {
        base.Start();
    }

    protected override void SetText()
    {
        question = "Awesome! Now we have some cool " + biomeName + " tiles!" + extraQuestion;
        optOne = "";
        optTwo = "";
        optThree = "";
        base.SetText();
    }

    public void SetBiomeName(int index)
    {
        CheckBiomePercentage(index);
        if (index == 0) biomeName = "desert";
        if (index == 1) biomeName = "sea";
        if (index == 2) biomeName = "forest";
        SetText();
        if (offerHelp) SetOptions();
        GiveTip();
    }

    private void SetOptions()
    {
        optOne = "No i like it the way it is";
        optTwo = "Sure add some more " + biomeName;
        optThree = "Actully add a lot more of " + biomeName;
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

    public void CheckBiomePercentage(int index)
    {
        int threshold = 0;

        if (index == 0)
        {
            if (grid.DesertPercentage() > preferenceHandler.desertPercentagePref + threshold)
            {
                extraQuestion = "It looks like you have less desert tiles than you normally prefer. Would you like me to add some more?";
                offerHelp = true;
            }
            else if (grid.DesertPercentage() < preferenceHandler.desertPercentagePref - threshold)
            {
                extraQuestion = "It looks like you have more desert tiles than you normally prefer. Would you like me to change some of them to something else?";
                offerHelp = true;
            }
        }
        if (index == 1)
        {
            if (grid.SeaPercentage() > preferenceHandler.seaPercentagePref + threshold)
            {
                extraQuestion = "It looks like you have less sea tiles than you normally prefer. Would you like me to add some more?";
                offerHelp = true;
            }
            else if (grid.SeaPercentage() < preferenceHandler.seaPercentagePref - threshold)
            {
                extraQuestion = "It looks like you have more sea tiles than you normally prefer. Would you like me to change some of them to something else?";
                offerHelp = true;
            }
        }
        if (index == 2)
        {
            if (grid.ForestPercentage() > preferenceHandler.forestPercentagePref + threshold)
            {
                extraQuestion = "It looks like you have less forest tiles than you normally prefer. Would you like me to add some more?";
                offerHelp = true;
            }
            else if (grid.ForestPercentage() < preferenceHandler.forestPercentagePref - threshold)
            {
                extraQuestion = "It looks like you have more forest tiles than you normally prefer. Would you like me to change some of them to something else?";
                offerHelp = true;
            }
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

                if (tile.PlacedObject())
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

                if (tile.Height() < lowestHeight)
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

                if (tile.PlacedObject())
                    Destroy(tile.PlacedObject());

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
