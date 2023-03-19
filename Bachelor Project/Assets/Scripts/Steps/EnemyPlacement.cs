using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlacement : Step
{
    [SerializeField] GameObject rangedToken;
    [SerializeField] GameObject meleeToken;

    [SerializeField] int numberOfRangedEnemies;
    [SerializeField] int numberOfMeleeEnemies;

    [SerializeField] int unitDifficulty;
    [SerializeField] int unitRange;

    public void PlaceEnemies()
    {
        StartStep();

        List<Tile[,]> selectedGrids = grid.SelectedGrids();
        Tile[,] map = grid.GetBaseGrid();

        float lowestHeight = 10000;
        float heighestHeight = 0;

        foreach(Tile tile in map)
        {
            if (tile.Height() < lowestHeight)
                lowestHeight = tile.Height();

            if (tile.Height() > heighestHeight)
                heighestHeight = tile.Height();
        }

        foreach (Tile[,] subgrid in selectedGrids)
        {
            foreach (Tile tile in subgrid)
            {
                if (tile.PlacedObject() || tile == null)
                    continue;

                int placementChance = Random.Range(0, (int)tile.Height());

                if(placementChance > heighestHeight / 2)
                {
                    //and if there are no enemies in a 1 tile radius

                    tile.InstantiateObjectOnTile(rangedToken);

                    List<Tile> nearbyTiles = grid.GetTilesAroundTile(tile, unitRange);

                    foreach(Tile otherTile in nearbyTiles)
                    {
                        otherTile.AddDifficulty(Mathf.Clamp(tile.Height() - otherTile.Height(), 0, tile.Height()) * unitDifficulty);
                    }
                }
            }
        }

    }
}
