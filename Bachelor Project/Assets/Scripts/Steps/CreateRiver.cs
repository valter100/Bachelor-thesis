using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRiver : Step
{
    [SerializeField] Tile startTile;
    [SerializeField] Tile endTile;

    [SerializeField] Color riverColor;
    [SerializeField] float riverWidth;
    [SerializeField] float riverHeight;

    [SerializeField] Tile[,] wholeMap;

    Tile currentTile;

    List<Tile> open = new List<Tile>();
    List<Tile> closed = new List<Tile>();
    List<Tile> riverList = new List<Tile>();

    bool randomPlacementAlongX;

    protected override void Start()
    {
        base.Start();
    }

    public void MakeRiver()
    {
        if (grid.Locked())
            return;

        wholeMap = grid.GetBaseGrid();
        grid.SetLocked(true);
        bool foundPath = false;
        //Step 1. Identify start and end tile around the maps borders
        int xStart, yStart, xEnd, yEnd;

        while (!foundPath)
        {
            randomPlacementAlongX = Random.value < 0.5f ? true : false;

            if (randomPlacementAlongX)
            {
                xStart = Random.Range(0, wholeMap.GetLength(0) - 1);
                yStart = Random.value < 0.5f ? 0 : wholeMap.GetLength(1) - 1;

                xEnd = Random.Range(0, wholeMap.GetLength(0) - 1);
                yEnd = yStart == 0 ? wholeMap.GetLength(1) - 1 : 0;
            }
            else
            {
                yStart = Random.Range(0, wholeMap.GetLength(1) - 1);
                xStart = Random.value < 0.5f ? 0 : wholeMap.GetLength(0) - 1;

                yEnd = Random.Range(0, wholeMap.GetLength(1) - 1);
                xEnd = xStart == 0 ? wholeMap.GetLength(0) - 1 : 0;
            }
            Debug.Log("Start X: " + xStart + ", Y: " + yStart);
            Debug.Log("End X: " + xEnd + ", Y: " + yEnd);

            startTile = grid.GetTileByCoordinate(xStart, yStart);
            endTile = grid.GetTileByCoordinate(xEnd, yEnd);

            StartStep();

            open.Clear();
            closed.Clear();

            startTile.SetH(CalculateDistanceBetweenNodes(startTile, endTile));
            startTile.SetF(0);
            startTile.SetParent(null);

            open.Add(startTile);

            while (open.Count > 0)
            {
                currentTile = open[0];

                for (int i = 1; i < open.Count; i++)
                {
                    if (open[i].F() < currentTile.F() ||
                        open[i].F() == currentTile.F() && open[i].H() < currentTile.H())
                    {
                        currentTile = open[i];
                    }
                }

                open.Remove(currentTile);
                closed.Add(currentTile);

                if (currentTile == endTile)
                {
                    RetracePath(startTile, endTile);
                    foundPath = true;
                    return;
                }

                foreach (Tile neighbour in currentTile.AdjacentTiles())
                {
                    if (closed.Contains(neighbour))
                        continue;


                    if (CalculateDistanceBetweenNodes(currentTile, neighbour) + currentTile.G() < neighbour.G() ||
                        !open.Contains(neighbour))
                    {
                        neighbour.SetG(CalculateDistanceBetweenNodes(neighbour, currentTile) + currentTile.G());
                        neighbour.SetH(CalculateDistanceBetweenNodes(neighbour, endTile));
                        neighbour.SetF(neighbour.G() + neighbour.H());
                        neighbour.SetParent(currentTile);
                    }

                    if (!open.Contains(neighbour) && CalculateHeightBetweenNodes(currentTile, neighbour) <= 2)
                    {
                        open.Add(neighbour);
                    }
                }
            }

        }
    }

    public List<Tile> RetracePath(Tile start, Tile end)
    {
        List<Tile> path = new List<Tile>();
        riverList.Clear();
        currentTile = end;

        while (currentTile != start)
        {
            path.Add(currentTile);
            riverList.Add(currentTile);
            currentTile = currentTile.Parent();
        }

        riverList.Add(currentTile);
        path.Add(currentTile);

        path.Reverse();

        StartCoroutine(ColorPath(path));

        return path;
    }

    IEnumerator ColorPath(List<Tile> path)
    {
        foreach (Tile tile in path)
        {
            List<Tile> adjacentTiles = new List<Tile>();
            if (randomPlacementAlongX)
            {
                if (tile.GetCoordinates().x > 0)
                {
                    adjacentTiles.Add(grid.GetTileByCoordinate((int)tile.GetCoordinates().x - 1, (int)tile.GetCoordinates().y));
                }

                if (tile.GetCoordinates().x < wholeMap.GetLength(0))
                {
                    adjacentTiles.Add(grid.GetTileByCoordinate((int)tile.GetCoordinates().x + 1, (int)tile.GetCoordinates().y));
                }

            }
            else
            {
                if (tile.GetCoordinates().y > 0)
                {
                    adjacentTiles.Add(grid.GetTileByCoordinate((int)tile.GetCoordinates().x, (int)tile.GetCoordinates().y - 1));
                }

                if (tile.GetCoordinates().y < wholeMap.GetLength(1))
                {
                    adjacentTiles.Add(grid.GetTileByCoordinate((int)tile.GetCoordinates().x, (int)tile.GetCoordinates().y + 1));
                }
            }

            foreach (Tile adjacentTile in adjacentTiles)
            {
                if (riverList.Contains(adjacentTile))
                    continue;

                riverList.Add(adjacentTile);
                FindObjectOfType<SetBiome>().ChangeBiomeOfTile(adjacentTile, 2);
                adjacentTile.BumpAnimation();
            }

            FindObjectOfType<SetBiome>().ChangeBiomeOfTile(tile, 2);
            tile.BumpAnimation();

            yield return new WaitForSeconds(0.05f);
        }

        grid.CreateSubgridFromList(riverList, false);

        grid.SetLocked(false);
        yield return null;
    }

    public void SetStartTile(Tile tile)
    {
        startTile = tile;
    }

    public void SetEndTile(Tile tile)
    {
        endTile = tile;
    }

    public float CalculateDistanceBetweenNodes(Tile start, Tile goal)
    {
        return Mathf.Abs(goal.GetCoordinates().x - start.GetCoordinates().x) + Mathf.Abs(goal.GetCoordinates().y - start.GetCoordinates().y) + Mathf.Abs(goal.Height() - start.Height());
    }

    public float CalculateHeightBetweenNodes(Tile start, Tile goal)
    {
        return Mathf.Abs(start.Height() - goal.Height());
    }
}
