using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBiome : Step
{
    [SerializeField] List<Color> biomeColors = new List<Color>();
    [SerializeField] List<int> heightDifference = new List<int>();
    [SerializeField] List<bool> biomeImpassable = new List<bool>();

    public override void DoAction()
    {
        base.DoAction();
    }

    public void ChangeBiome(int biomeIndex)
    {
        foreach (Tile[,] subgrid in grid.SelectedGrids())
        {
            foreach (Tile tile in subgrid)
            {
                if (tile == null)
                    continue;

                if(tile.PlacedObject())
                    Destroy(tile.PlacedObject());

                tile.SetBiome(biomeIndex);
                tile.SetColor(biomeColors[biomeIndex]);
                tile.SetHeight(heightDifference[biomeIndex]);
                tile.SetImpassable(biomeImpassable[biomeIndex]);
            }
        }
    }

    public void ChangeBiomeOnSpecificGrid(Tile[,] subgrid, int biomeIndex)
    {
        foreach (Tile tile in subgrid)
        {
            if (tile == null)
                continue;

            if (tile.PlacedObject())
                Destroy(tile.PlacedObject());

            tile.SetBiome(biomeIndex);
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

                tile.SetColor(biomeColors[biomeIndex]);
            }
        }
    }

    public List<bool> GetImpassableList()
    {
        return biomeImpassable;
    }
}
