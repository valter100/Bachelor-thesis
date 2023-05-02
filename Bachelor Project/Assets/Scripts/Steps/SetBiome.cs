using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SetBiome : Step
{
    [SerializeField] List<Color> biomeColors = new List<Color>();
    [SerializeField] List<float> heightDifference = new List<float>();
    [SerializeField] List<bool> biomeImpassable = new List<bool>();
    List<Tile[,]> selectedSubgrids = new List<Tile[,]>();
    Queue<Tile> Q = new Queue<Tile>();
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
        question = "Awesome! Now we have some cool " + biomeName + " tiles! " + extraQuestion;
        optOne = "" + extraOptOne;
        optTwo = "" + extraOptTwo;
        optThree = "" + extraOptThree;
        options.Clear();
        base.SetText();
    }

    public void SetBiomeName(int index)
    {
        grid.CalculateSubgridTypes();
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

        selectedSubgrids = grid.SelectedGrids();
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
                            if ( i < randomSubgrid.GetLength(0)) continue;
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
                Tile startpos = randomSubgrid[randomSubgrid.GetLength(0) / 2, randomSubgrid.GetLength(1) / 2];
                List<Tile> tileList = new List<Tile>();

                foreach (Tile t in randomSubgrid)
                {
                    tileList.Add(t);
                }

                bool done = false;
                int x = 0;

                newGrid.Add(startpos);

                while (!done)
                {
                    int goal = tileAmount;

                    try
                    {
                        if (goal <= 0 || x > grid.MapDimensions().x) done = true;

                        newGrid = grid.GetTilesAroundTile(startpos, x++);

                        foreach (Tile t in newGrid)
                        {
                            if (tileList.Contains(t)) continue;

                            goal--;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            Vector2 startPosition = new Vector2(Mathf.Infinity, Mathf.Infinity);
            Vector2 endPosition = new Vector2(Mathf.NegativeInfinity, Mathf.NegativeInfinity);

            foreach (Tile tile in newGrid)
            {
                if (tile.GetCoordinates().x < startPosition.x)
                {
                    if (tile.GetCoordinates().y < startPosition.y) startPosition = tile.GetCoordinates();
                }

                if (tile.GetCoordinates().x > endPosition.x)
                {
                    if (tile.GetCoordinates().y > endPosition.y) endPosition = tile.GetCoordinates();
                }
            }

            Tile[,] newSubgrid = grid.GetTilesBetween(startPosition, endPosition);

            grid.CreateSubgrid(newSubgrid);
            grid.AddSelectedGrid(newSubgrid);
            ChangeBiomeOnSpecificGrid(newSubgrid, changeIndex);
            handleObjects.PlaceObjectsAction();
        }
        if (actionIndex == 2)
        {
            ChangeBiomeNoHeight(3);
        }
    }

    public void CheckBiomePercentage(int index)
    {
        int threshold = 0;
        int a = grid.DesertPercentage();
        int b = grid.ForestPercentage();
        int c = grid.SeaPercentage();
        if (index == 0)
        {
            if (grid.DesertPercentage() < preferenceHandler.desertPercentagePref + threshold)
            {
                extraQuestion = "It looks like you have less desert tiles than you normally prefer. Would you like me to add some more?";
                offerHelp = true;
                reduce = false;
                percentageDifferece = preferenceHandler.desertPercentagePref - grid.DesertPercentage();
                changeIndex = 1;
                extraOptTwo = "Sure add some more desert";
            }
            else if (grid.DesertPercentage() > preferenceHandler.desertPercentagePref - threshold)
            {
                extraQuestion = "It looks like you have more desert tiles than you normally prefer. Would you like me to change some of them to something else?";
                offerHelp = true;
                reduce = true;
                percentageDifferece = preferenceHandler.desertPercentagePref - grid.DesertPercentage();
                changeIndex = 3;
                extraOptTwo = "Sure remove some desert";
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
                extraOptTwo = "Sure add some more sea";
            }
            else if (grid.SeaPercentage() > preferenceHandler.seaPercentagePref - threshold)
            {
                extraQuestion = "It looks like you have more sea tiles than you normally prefer. Would you like me to change some of them to something else?";
                offerHelp = true;
                reduce = true;
                percentageDifferece = preferenceHandler.seaPercentagePref - grid.SeaPercentage();
                changeIndex = 3;
                extraOptTwo = "Sure remove some sea";
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
                extraOptTwo = "Sure add some more forest";
            }
            else if (grid.ForestPercentage() > preferenceHandler.forestPercentagePref - threshold)
            {
                extraQuestion = "It looks like you have more forest tiles than you normally prefer. Would you like me to change some of them to something else?";
                offerHelp = true;
                reduce = true;
                percentageDifferece = preferenceHandler.forestPercentagePref - grid.ForestPercentage();
                changeIndex = 2;
                extraOptTwo = "Sure remove some forest";
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
