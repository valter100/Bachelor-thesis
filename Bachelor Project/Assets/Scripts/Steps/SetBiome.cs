using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBiome : Step
{
    [SerializeField] List<Color> biomeColors = new List<Color>();
    [SerializeField] List<int> heightDifference = new List<int>();

    public override void DoAction()
    {
        base.DoAction();
    }

    public void ChangeBiome(int biomeIndex)
    {
        Tile[,] subgrid = grid.SelectedGrid();

        foreach (Tile tile in subgrid)
        {
            tile.SetColor(biomeColors[biomeIndex]);
            tile.SetHeight(false, heightDifference[biomeIndex]);
        }
    }
}
