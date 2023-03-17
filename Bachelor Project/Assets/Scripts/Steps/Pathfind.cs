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

    Tile currentTile;

    List<Tile> open = new List<Tile>();
    List<Tile> closed = new List<Tile>();

    public void PathfindStartToGoal()
    {
        foreach(Tile tile in closed)
        {
            tile.SetToOldColor();
        }

        open.Clear();
        closed.Clear();

        //Tile[,] map = grid.GetBaseGrid();

        startTile.SetH(CalculateDistanceBetweenNodes(startTile, endTile));
        startTile.SetF(startTile.G() + startTile.H());
        startTile.SetParent(null);

        open.Add(startTile);

        while(open.Count > 0)
        {
            currentTile = open[0];

            for(int i = 1; i < open.Count; i++)
            {
                if (open[i].F() < currentTile.F() || 
                    open[i].F() == currentTile.F() && open[i].H() < currentTile.H())
                {
                    currentTile = open[i];
                }
            }

            open.Remove(currentTile);
            closed.Add(currentTile);

            if(currentTile == endTile)
            {
                RetracePath(startTile, endTile);
                grid.SetLocked(false);
                return;
            }

            foreach(Tile neighbour in currentTile.AdjacentTiles())
            {
                if (closed.Contains(neighbour))
                    continue;

                if(CalculateDistanceBetweenNodes(currentTile, neighbour) + currentTile.G() < neighbour.G()||
                    !open.Contains(neighbour))
                {
                    neighbour.SetG(CalculateDistanceBetweenNodes(currentTile, neighbour) + currentTile.G());
                    neighbour.SetH(CalculateDistanceBetweenNodes(neighbour, endTile));
                    neighbour.SetParent(currentTile);
                }

                if(!open.Contains(neighbour) && CalculateHeightBetweenNodes(currentTile, neighbour) <= 1 && !neighbour.Impassable())
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
        currentTile.SetColor(roadColor);

        while (currentTile != start)
        {
            path.Add(currentTile);

            if(currentTile != end)
            {
                currentTile.SetColor(roadColor);
            }

            currentTile = currentTile.Parent();
        }
        currentTile.SetColor(roadColor);
        path.Reverse();

        return path;
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
        return Mathf.Abs(goal.GetCoordinates().x - start.GetCoordinates().x) + Mathf.Abs(goal.GetCoordinates().y - start.GetCoordinates().y);
    }

    public float CalculateHeightBetweenNodes(Tile start, Tile goal)
    {
        return Mathf.Abs(start.Height() - goal.Height());
    }
}
