using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject tile;
    [SerializeField] Vector3 mapDimensions;
    [SerializeField] float tileBuffer;
    Tile[,] baseGrid;
    List<Tile[,]> selectedGrids = new List<Tile[,]>();
    List<Tile[,]> subgridList = new List<Tile[,]>();
    [SerializeField] int numberOfSubgrids;


    [SerializeField] Color baseTileColor;
    [SerializeField] Color highlightedTileColor;
    [SerializeField] Color subgridColor;
    [SerializeField] AI ai;

    [SerializeField] List<Vector3> peakPositions = new List<Vector3>();

    [Header("Peaks")]
    [SerializeField] int peakHeight;
    [SerializeField] int peakHeightRange;
    [SerializeField] int peakAmount;
    // Start is called before the first frame update
    void Start()
    {
        subgridColor = UnityEngine.Random.ColorHSV();
    }

    private void Update()
    {

    }

    public void CreateGrid()
    {
        if (baseGrid != null)
        {
            foreach (Tile tile in baseGrid)
            {
                Destroy(tile.gameObject);
            }
            subgridList.Clear();
            peakPositions.Clear();
        }

        //int tilesCreatedEachFrame = 1;

        baseGrid = new Tile[(int)mapDimensions.x, (int)mapDimensions.z];

        for(int i = 0; i < peakAmount; i++)
        {
            peakPositions.Add(
                new Vector3(
                    (int)UnityEngine.Random.Range(0, mapDimensions.x),
                    peakHeight + UnityEngine.Random.Range(-peakHeightRange, peakHeightRange),
                    (int)UnityEngine.Random.Range(0, mapDimensions.z
                    )));
        }

        //for (int i = 0; i < mapDimensions.x; i++)
        //{
        //    for (int j = 0; j < mapDimensions.z; j++)
        //    {
        //        baseGrid[i, j] = Instantiate(tile, new Vector3(i, 0, j) + new Vector3(i * tileBuffer, 0, j * tileBuffer), Quaternion.identity).GetComponent<Tile>(); //Add Height later
        //        baseGrid[i, j].transform.parent = gameObject.transform;
        //        baseGrid[i, j].gameObject.name = (i + 1) + "," + (j + 1);
        //        baseGrid[i, j].SetCoordinates(i, j);
        //        baseGrid[i, j].SetColors(baseTileColor, highlightedTileColor);
        //    }
        //}

        StartCoroutine(_CreateGridRow(0));
    }

    IEnumerator _CreateGridRow(int i)
    {
        for (int j = 0; j < mapDimensions.z; j++)
        {
            baseGrid[i, j] = Instantiate(tile, Vector3.zero, Quaternion.identity).GetComponent<Tile>();
            baseGrid[i, j].SetCoordinates(i, j);
            baseGrid[i, j].transform.parent = gameObject.transform;
            baseGrid[i, j].gameObject.name = (i + 1) + "," + (j + 1);
            baseGrid[i, j].SetColors(baseTileColor, highlightedTileColor);
            baseGrid[i, j].SetAdjacentTiles(this);
            baseGrid[i, j].FindClosestPeak(peakPositions);
        }

        if (i == mapDimensions.x -1)
        {
            StartCoroutine(_PositionGridRow(i));
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(0.05f);

            StartCoroutine(_PositionGridRow(i));
            StartCoroutine(_CreateGridRow(i + 1));
        }
    }

    IEnumerator _PositionGridRow(int x)
    {
        for (int z = 0; z < mapDimensions.z; z++)
        {
            baseGrid[x, z].gameObject.transform.position = new Vector3(baseGrid[x, z].GetCoordinates().x, 0, baseGrid[x, z].GetCoordinates().y) + new Vector3(x * tileBuffer, 0, z * tileBuffer);
            baseGrid[x, z].SetHeightWithSmoothing(0);
        }

        yield return null;
    }

    public void CreateSubgrid(Tile[,] subgrid)
    {
        foreach (Tile tile in subgrid)
        {
            if (tile.PartOfSubgrid())
            {
                tile.RemoveFromSubgrid();
            }

            tile.SetColor(subgridColor);
            tile.SetPartOfSubgrid(subgrid);
        }

        subgridColor = UnityEngine.Random.ColorHSV();

        subgridList.Add(subgrid);

        RemoveEmptySubgrids();

        numberOfSubgrids = subgridList.Count;
    }

    public Tile[,] GetSubgrid(int index)
    {
        if (subgridList.Count < index)
        {
            Debug.Log("Index is out of bounds. No subgrid to return");
            return null;
        }

        return subgridList[index];
    }

    public Tile[,] GetBaseGrid() => baseGrid;
    public Tile getTileByCoordinate(int x, int y) => baseGrid[x, y];

    public void SetGridDimension(int x, int y, int z)
    {
        mapDimensions = new Vector3(x, y, z);
    }

    public void AddSelectedGrid(Tile[,] newGridSelected)
    {
        FindObjectOfType<GridSelect>().SetUIActive(selectedGrids.Count != 0);

        selectedGrids.Add(newGridSelected);
        FindObjectOfType<GridSelect>().DoAction();

    }

    public List<Tile[,]> SelectedGrids()
    {
        List<Tile[,]> tempList = selectedGrids;

        tempList.Remove(baseGrid);

        return tempList;
    }

    public void UpdateGridAmount()
    {
        numberOfSubgrids = subgridList.Count;
    }

    public Tile[,] GetTilesBetween(Vector2 startCoordinates, Vector2 endCoordinates)
    {
        int sizeX, sizeY;
        sizeX = (int)Mathf.Abs(startCoordinates.x - endCoordinates.x);
        sizeY = (int)Mathf.Abs(startCoordinates.y - endCoordinates.y);

        Tile[,] tempGrid = new Tile[sizeX, sizeY];

        int i = 0;
        int j = 0;

        int startX = (int)startCoordinates.x;
        int endX = (int)endCoordinates.x;
        int startY = (int)startCoordinates.y;
        int endY = (int)endCoordinates.y;

        if (startX > endX)
        {
            (startX, endX) = (endX, startX);
        }

        if (startY > endY)
        {
            (startY, endY) = (endY, startY);
        }

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                tempGrid[i, j] = baseGrid[x, y];
                j++;
            }
            i++;
            j = 0;
        }

        return tempGrid;
    }

    public void RemoveEmptySubgrids()
    {
        for (int i = 0; i < subgridList.Count; i++)
        {
            bool containsTile = false;

            foreach (Tile tile in subgridList[i])
            {
                if (tile != null)
                    containsTile = true;
            }

            if (!containsTile)
            {
                Debug.Log("Subgrid removed");

                subgridList.RemoveAt(i);
                i--;
            }

        }

        UpdateGridAmount();
    }

    public void SmoothGrid()
    {
        foreach (Tile[,] subgrid in selectedGrids)
        {
            foreach (Tile tile in subgrid)
                tile.SetHeightWithSmoothing(0);
        }
    }

    public void DeselectSubgrids()
    {
        if (selectedGrids.Count == 0)
            return;

        for (int i = 0; i < selectedGrids.Count; i++)
        {
            foreach (Tile tile in selectedGrids[i])
            {
                if (tile != null)
                    tile.Deselect();
            }
            selectedGrids.RemoveAt(i--);
        }

        FindObjectOfType<GridSelect>().SetUIActive(false);

        AddSelectedGrid(baseGrid);
    }

    public void SelectSubgrid(Tile[,] subgrid)
    {
        AddSelectedGrid(subgrid);

        foreach (Tile tile in subgrid)
        {
            if (tile != null)
                tile.Select();
        }

    }

    public void SetAllTilesToHeight(float height, Tile[,] targetedGrid)
    {
        foreach (Tile tile in targetedGrid)
        {
            tile.SetHeight(height);
        }
    }

    public void MergeSubgrids()
    {
        //Step 1: Find the color of the first selected subgrid

        Color gridColor = Color.white;
        bool foundColor = false;
        for (int i = 0; i < SelectedGrids()[0].GetLength(0); i++)
        {
            for (int j = 0; j < SelectedGrids()[0].GetLength(1); j++)
            {
                if (SelectedGrids()[0][i, j] != null)
                {
                    gridColor = SelectedGrids()[0][0, 0].Color();
                    foundColor = true;
                    break;
                }
            }
            if (foundColor)
                break;
        }

        //step 2: calculate the size of the new Tile[,]

        int smallestX = 10000;
        int smallestY = 10000;
        int largestX = 0;
        int largestY = 0;

        foreach (Tile[,] grid in SelectedGrids())
        {
            foreach (Tile tile in grid)
            {
                if (tile == null)
                    continue;

                if (tile.GetCoordinates().x < smallestX)
                    smallestX = (int)tile.GetCoordinates().x;
                if (tile.GetCoordinates().y < smallestY)
                    smallestY = (int)tile.GetCoordinates().y;

                if (tile.GetCoordinates().x > largestX)
                    largestX = (int)tile.GetCoordinates().x;
                if (tile.GetCoordinates().y > largestY)
                    largestY = (int)tile.GetCoordinates().y;
            }
        }


        //step 3: create a new Tile[,] with every tile from all the selected subgrids
        int subgridXSize = largestX - smallestX;
        int subgridYSize = largestY - smallestY;

        Tile[,] newSubgrid = new Tile[subgridXSize, subgridYSize];

        int tilesBeforeMerge = 0;

        int currentWidthIndex = 0;
        int currentHeightIndex = 0;

        Debug.Log("Subgrid Size: X: " + subgridXSize + " Y: " + subgridYSize);

        List<Tile> selectedTiles = new List<Tile>();

        foreach (Tile[,] grid in SelectedGrids())
        {
            tilesBeforeMerge += grid.Length;

            foreach (Tile tile in grid)
            {
                if (selectedTiles.Contains(tile))
                    continue;
                selectedTiles.Add(tile);

                try
                {
                    newSubgrid[currentWidthIndex++, currentHeightIndex] = tile;
                }
                catch (Exception ex)
                {
                    Debug.Log(ex);
                    Debug.Log("X: " + currentWidthIndex + " Y: " + currentHeightIndex);
                }

                if (currentWidthIndex == subgridXSize)
                {
                    currentWidthIndex = 0;
                    currentHeightIndex++;
                }

            }
        }

        Debug.Log("Number of tiles before Merge: " + tilesBeforeMerge);
        Debug.Log("Number of Tiles in new subgrid: " + newSubgrid.Length);


        //step 4: Remove all the selected subgrids from the subgrid list
        for (int i = 0; i < subgridList.Count; i++)
        {
            for (int j = 0; j < SelectedGrids().Count; j++)
            {
                if (subgridList[i] == SelectedGrids()[j])
                {
                    subgridList.Remove(SelectedGrids()[j]);
                    i--;
                    break;
                }
            }
        }

        //step 5: clear the selectedGrids list
        SelectedGrids().Clear();

        //Step 6: Add the new subgrid to the subgrid list and selected list
        subgridList.Add(newSubgrid);
        selectedGrids.Add(newSubgrid);
        UpdateGridAmount();

        //Step 7: Apply that color to every tile in the new Tile[,]
        foreach (Tile tile in newSubgrid)
        {
            if (tile != null)
            {
                tile.SetColor(gridColor);
                tile.SetPartOfSubgrid(newSubgrid);
            }
        }


    }

    public Vector2 MapDimensions() => mapDimensions;
}
