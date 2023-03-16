using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleObjects : Step
{
    [SerializeField] float placementChance;
    [SerializeField] List<GameObject> forestObjects = new List<GameObject>();

    public void PlaceObjectsAction()
    {
        if (grid.Locked())
            return;

        StartCoroutine(PlaceObjectsOverTime());
    }

    public void RemoveObjectsAction()
    {
        if (grid.Locked())
            return;

        StartCoroutine(RemoveObjectsOverTime());
    }

    IEnumerator PlaceObjectsOverTime()
    {
        grid.SetLocked(true);

        foreach (Tile[,] subgrid in grid.SelectedGrids())
        {
            foreach (Tile tile in subgrid)
            {
                if (tile == null)
                    continue;

                if (tile.PlacedObject())
                {
                    Destroy(tile.PlacedObject());

                    if (!FindObjectOfType<SetBiome>().GetImpassableList()[tile.BiomeIndex()])
                        tile.SetImpassable(false);
                }

                float random = Random.Range(0, 100);

                if (random < placementChance)
                {
                    int objectIndex = Random.Range(0, forestObjects.Count);
                    tile.PlaceObjectOnTile(forestObjects[objectIndex]);
                    yield return new WaitForSeconds(0.025f);
                }
            }
        }

        grid.SetLocked(false);

        yield return null;
    }

    IEnumerator RemoveObjectsOverTime()
    {
        grid.SetLocked(true);

        foreach (Tile[,] subgrid in grid.SelectedGrids())
        {
            foreach (Tile tile in subgrid)
            {
                if (tile == null)
                    continue;

                if (tile.PlacedObject())
                {
                    Destroy(tile.PlacedObject());

                    if (!FindObjectOfType<SetBiome>().GetImpassableList()[tile.BiomeIndex()])
                        tile.SetImpassable(false);

                    yield return new WaitForSeconds(0.025f);
                }
            }
        }

        grid.SetLocked(false);

        yield return null;
    }

}
