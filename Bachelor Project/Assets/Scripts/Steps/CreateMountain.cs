using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMountain : Step
{
    Vector3 peakLocation;
    float peakHeight;

    public void RandomizePeakLocation()
    {
        peakHeight = Random.Range(1, 5);
        peakLocation = new Vector3(Random.Range(0, grid.GetBaseGrid().GetLength(0)), 0, Random.Range(0, grid.GetBaseGrid().GetLength(1)));
    }

    public void AddPeak()
    {
        RandomizePeakLocation();

        List<Tile> mountainTiles = new List<Tile>();

        foreach(Tile tile in grid.GetBaseGrid())
        {
            Tile mountainTile = tile.SetHeightWithNewPeak(new Vector2(peakLocation.x, peakLocation.z), peakHeight);

            if(mountainTile)
            {
                mountainTiles.Add(mountainTile);
            }
        }
        grid.CreateSubgridFromList(mountainTiles, false);
    }
}
