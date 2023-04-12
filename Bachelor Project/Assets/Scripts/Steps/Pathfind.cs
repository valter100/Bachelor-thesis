using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfind : Step
{
    [SerializeField] Tile startTile;
    [SerializeField] Tile endTile;

    //Color startColor;
    //Color endColor;
    [SerializeField] Color roadColor;
    [SerializeField] List<Tile> pathBack = new List<Tile>();

    Tile currentTile;

    List<Tile> open = new List<Tile>();
    List<Tile> closed = new List<Tile>();

    public void PathfindStartToGoal()
    {
        if (startTile == null || endTile == null)
            return;

        StartStep();

        foreach (Tile tile in closed)
        {
            if (tile.Color() == roadColor)
                tile.SetToOldColor();
        }

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
                pathBack = RetracePath(startTile, endTile);
                grid.SetLocked(false);
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

                if (!open.Contains(neighbour) && CalculateHeightBetweenNodes(currentTile, neighbour) <= 1 && !neighbour.Impassable())
                {
                    open.Add(neighbour);
                }
            }
        }

        grid.SetLocked(false);
    }

    public List<Tile> RetracePath(Tile start, Tile end)
    {
        List<Tile> path = new List<Tile>();
        currentTile = end;

        while (currentTile != start)
        {
            path.Add(currentTile);
            currentTile = currentTile.Parent();
        }

        path.RemoveAt(0);
        path.Reverse();

        StartCoroutine(ColorPath(path));

        return path;
    }

    IEnumerator ColorPath(List<Tile> path)
    {
        foreach (Tile tile in path)
        {
            tile.SetColor(roadColor);
            tile.BumpAnimation();
            yield return new WaitForSeconds(0.1f);
        }


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
