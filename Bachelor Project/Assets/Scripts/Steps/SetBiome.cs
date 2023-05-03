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
                        newGrid = grid.GetTilesAroundTile(startpos, x++);

                        foreach (Tile t in newGrid)
                        {
                            if (tileList.Contains(t)) continue;

                            goal--;
                        }

                        if (goal <= 0 || x > grid.MapDimensions().x) done = true;
                    }
                    catch
                    {
                        if (x > grid.MapDimensions().x) done = true;
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

        }
    }

    public void ChangeBiomeOfGrid(List<Tile> gridList, int biomeIndex)
    {
        StartStep();

        if (biomeIndex == 2)
        {
            float waterHeight = CalculateWaterHeight();
            heightDifference[biomeIndex] = waterHeight;
        }

        foreach (Tile tile in gridList)
        {
            if (tile.PlacedObject())
                Destroy(tile.PlacedObject());

            tile.SetBiome(biomeIndex, true);
            tile.SetColor(biomeColors[biomeIndex]);
            tile.SetHeight(heightDifference[biomeIndex]);
            tile.SetImpassable(biomeImpassable[biomeIndex]);
        }
    }

    public void ChangeBiomeOfTile(Tile tile, int biomeIndex)
    {
        if (tile.PlacedObject())
            Destroy(tile.PlacedObject());

        tile.SetBiome(biomeIndex, true);
        tile.SetColor(biomeColors[biomeIndex]);
        tile.SetHeight(heightDifference[biomeIndex]);
        tile.SetImpassable(biomeImpassable[biomeIndex]);
    }

    public void ChangeBiomeOfTileNoHeight(Tile tile, int biomeIndex)
    {
        if (tile.PlacedObject())
            Destroy(tile.PlacedObject());

        tile.SetBiome(biomeIndex, true);
        tile.SetColor(biomeColors[biomeIndex]);
        tile.SetImpassable(biomeImpassable[biomeIndex]);
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
