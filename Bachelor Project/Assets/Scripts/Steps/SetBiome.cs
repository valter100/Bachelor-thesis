using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetBiome : Step
{
    [SerializeField] List<Color> biomeColors = new List<Color>();
    [SerializeField] List<float> heightDifference = new List<float>();
    [SerializeField] List<bool> biomeImpassable = new List<bool>();
    HandleObjects handleObjects;

    string biomeName = "";
    string extraQuestion = "";
    string extraOptOne, extraOptTwo, extraOptThree;
    bool offerHelp = false;
    bool reduce = false;
    float percentageDifferece;
    int changeIndex;

    private void Start()
    {
        handleObjects = FindObjectOfType<HandleObjects>();
        base.Start();
    }

    protected override void SetText()
    {
        question = "Awesome! Now we have some cool " + biomeName + " tiles!" + extraQuestion;
        optOne = "" + extraOptOne;
        optTwo = "" + extraOptTwo;
        optThree = "" + extraOptThree;
        options.Clear();
        base.SetText();
    }

    public void SetBiomeName(int index)
    {
        CheckBiomePercentage(index);
        if (index == 0) biomeName = "desert";
        if (index == 1) biomeName = "sea";
        if (index == 2) biomeName = "forest";
        if (offerHelp)
        {
            extraOptOne = "No i like it the way it is";
            extraOptThree = "Actully remove the subgrid with " + biomeName;
        }
        SetText();
        GiveTip();
    }

    public override void GiveTip()
    {
        base.GiveTip();
    }

    public override void DoAction(int actionIndex)
    {
        if (!offerHelp) return;

        List<Tile[,]> selectedSubgrids = grid.SelectedGrids();
        Tile[,] randomSubgrid = selectedSubgrids[Random.Range(0, selectedSubgrids.Count)];


        float percentagePerTile = 100 / (float)(grid.MapDimensionX() * grid.MapDimensionZ());
        int tileAmount = (int)(Mathf.Abs(percentageDifferece) / percentagePerTile);


        if (actionIndex == 0)
        {

        }
        if (actionIndex == 1)
        {
            List<Tile> newGrid = new List<Tile>();

            if (reduce)
            {
                int x = 0;
                int z = 0;
                for (int i = tileAmount; i > 0; i--)
                {
                    try
                    {
                        newGrid.Add(randomSubgrid[x, z]);
                        x++;
                        if (x >= randomSubgrid.GetLength(0))
                        {
                            x = 0;
                            z++;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                Vector2 startpos = randomSubgrid[randomSubgrid.GetLength(0) - 1, randomSubgrid.GetLength(1) - 1].GetCoordinates();
                Tile[,] basegrid = grid.GetBaseGrid();
                int x = 0;
                int z = 0;
                for (int i = tileAmount; i > 0; i--)
                {
                    try
                    {
                        if ((int)startpos.x + x < basegrid.GetLength(0) && (int)startpos.y + z < basegrid.GetLength(1))
                            newGrid.Add(basegrid[(int)startpos.x + x, (int)startpos.y + z]);
                        x++;
                        if (x >= randomSubgrid.GetLength(0))
                        {
                            x = 0;
                            z++;
                        }
                        tileAmount--;
                    }
                    catch
                    {
                    }
                }
            }

            if (changeIndex != 3) ChangeBiomeOnSpecificGrid(newGrid, changeIndex);
            else ChangeBiomeNoHeight(newGrid, 3);
            handleObjects.PlaceObjectsAction();
        }
        if (actionIndex == 2)
        {
            if (reduce)
            {
                ChangeBiomeNoHeight(3);
            }
            else
            {

            }
        }
    }

    public void CheckBiomePercentage(int index)
    {
        int threshold = 0;
        int p = grid.DesertPercentage();
        if (index == 0)
        {
            if (grid.DesertPercentage() < preferenceHandler.desertPercentagePref + threshold)
            {
                extraQuestion = "It looks like you have less desert tiles than you normally prefer. Would you like me to add some more?";
                offerHelp = true;
                reduce = false;
                percentageDifferece = preferenceHandler.desertPercentagePref - grid.DesertPercentage();
                changeIndex = 1;
                extraOptTwo = "Sure add some more " + biomeName;
                extraOptThree = "Actully add a lot more of " + biomeName;
            }
            else if (grid.DesertPercentage() > preferenceHandler.desertPercentagePref - threshold)
            {
                extraQuestion = "It looks like you have more desert tiles than you normally prefer. Would you like me to change some of them to something else?";
                offerHelp = true;
                reduce = true;
                percentageDifferece = preferenceHandler.desertPercentagePref - grid.DesertPercentage();
                changeIndex = 3;
                extraOptTwo = "Sure remove some " + biomeName;
            }
        }
        if (index == 1)
        {
            if (grid.SeaPercentage() < preferenceHandler.seaPercentagePref + threshold)
            {
                extraQuestion = "It looks like you have less sea tiles than you normally prefer. Would you like me to add some more?";
                offerHelp = true;
                reduce = false;
                percentageDifferece = preferenceHandler.seaPercentagePref - grid.SeaPercentage();
                changeIndex = 2;
                extraOptTwo = "Sure add some more " + biomeName;
                extraOptThree = "Actully add a lot more of " + biomeName;
            }
            else if (grid.SeaPercentage() > preferenceHandler.seaPercentagePref - threshold)
            {
                extraQuestion = "It looks like you have more sea tiles than you normally prefer. Would you like me to change some of them to something else?";
                offerHelp = true;
                reduce = true;
                percentageDifferece = preferenceHandler.seaPercentagePref - grid.SeaPercentage();
                changeIndex = 3;
                extraOptTwo = "Sure remove some " + biomeName;
            }
        }
        if (index == 2)
        {
            if (grid.ForestPercentage() < preferenceHandler.forestPercentagePref + threshold)
            {
                extraQuestion = "It looks like you have less forest tiles than you normally prefer. Would you like me to add some more?";
                offerHelp = true;
                reduce = false;
                percentageDifferece = preferenceHandler.forestPercentagePref - grid.ForestPercentage();
                changeIndex = 3;
                extraOptTwo = "Sure add some more " + biomeName;
                extraOptThree = "Actully add a lot more of " + biomeName;
            }
            else if (grid.ForestPercentage() > preferenceHandler.forestPercentagePref - threshold)
            {
                extraQuestion = "It looks like you have more forest tiles than you normally prefer. Would you like me to change some of them to something else?";
                offerHelp = true;
                reduce = true;
                percentageDifferece = preferenceHandler.forestPercentagePref - grid.ForestPercentage();
                changeIndex = 2;
                extraOptTwo = "Sure remove some " + biomeName;
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

    public void ChangeBiomeOnSpecificGrid(List<Tile> subgrid, int biomeIndex)
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

    public void ChangeBiomeNoHeight(List<Tile> subgrid, int biomeIndex)
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

    public List<bool> GetImpassableList()
    {
        return biomeImpassable;
    }
}
