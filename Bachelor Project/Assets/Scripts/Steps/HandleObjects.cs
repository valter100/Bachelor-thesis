using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleObjects : Step
{
    [SerializeField] float placementChance;

    List<List<GameObject>> biomeObjects = new List<List<GameObject>>();
    [SerializeField] List<GameObject> standardObjects = new List<GameObject>();
    [SerializeField] List<GameObject> forestObjects = new List<GameObject>();
    [SerializeField] List<GameObject> seaObjects = new List<GameObject>();
    [SerializeField] List<GameObject> desertObjects = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        biomeObjects.Add(standardObjects);
        biomeObjects.Add(desertObjects);
        biomeObjects.Add(seaObjects);
        biomeObjects.Add(forestObjects);
    }

    public void PlaceObjectsAction()
    {
        if (grid.Locked())
            return;

        StartStep();

        StartCoroutine(PlaceObjectsOverTime());
    }

    public void RemoveObjectsAction()
    {
        if (grid.Locked())
            return;

        StartStep();

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
                    int listIndex = tile.BiomeIndex();
                    List<GameObject> list = biomeObjects[listIndex];
                    int objectIndex = Random.Range(0, list.Count);
                    tile.InstantiateObjectOnTile(list[objectIndex]);
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
