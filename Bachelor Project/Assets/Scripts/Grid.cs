using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] bool preferencePicking;
    [SerializeField] GameObject tile;
    [SerializeField] Vector3 mapDimensions;
    [SerializeField] float tileBuffer;
    Tile[,] baseGrid;
    List<Tile[,]> selectedGrids = new List<Tile[,]>();
    List<Tile[,]> subgridList = new List<Tile[,]>();
    [SerializeField] int numberOfSubgrids;
    [SerializeField] float plainPercentage;
    [SerializeField] float forestPercentage;
    [SerializeField] float seaPercentage;
    [SerializeField] float desertPercentage;
    [SerializeField] float percentageOfTilesWithObjects;
    bool locked;

    [SerializeField] Color baseTileColor;
    [SerializeField] Color highlightedTileColor;
    [SerializeField] Color subgridColor;
    [SerializeField] AI ai;

    [SerializeField] List<Vector3> peakPositions = new List<Vector3>();

    [Header("Peaks")]
    [SerializeField] int peakHeight;
    [SerializeField] int peakHeightRange;
    [SerializeField] int peakAmount;

    [SerializeField] float rotateSpeed;
    [SerializeField] float scaleSpeed;

    public delegate void GridCreation();
    public static event GridCreation OnGridCreate;


    public int MapDimensionX() => (int)mapDimensions.x;
    public int MapDimensionZ() => (int)mapDimensions.z;
    public int PeakHeight() => peakHeight;
    public int PeakAmount() => peakAmount;
    public int PeakHeightRange() => peakHeightRange;


    void Start()
    {
        subgridColor = UnityEngine.Random.ColorHSV();
    }

    private void Update()
    {
        if (!preferencePicking)
            return;

        if (Input.mouseScrollDelta.y != 0)
            ScaleWithMouseWheel();

        if (Input.GetMouseButton(0))
            RotateWithMouse();
    }

    public void CreateGrid()
    {
        if (locked)
            return;

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

        for (int i = 0; i < peakAmount; i++)
        {
            peakPositions.Add(
                new Vector3(
                    (int)UnityEngine.Random.Range(0, mapDimensions.x),
                    peakHeight + UnityEngine.Random.Range(-peakHeightRange, peakHeightRange),
                    (int)UnityEngine.Random.Range(0, mapDimensions.z
                    )));
        }

        StartCoroutine(_CreateGridRow(0));

        OnGridCreate();
    }

    public void CreateGrid(int mapsizeX, int mapSizeZ, int peakHeight, int peakHeightRange, int peakAmount)
    {
        if (locked)
            return;

        if (baseGrid != null)
        {
            foreach (Tile tile in baseGrid)
            {
                Destroy(tile.gameObject);
            }
            subgridList.Clear();
            peakPositions.Clear();
        }


        mapDimensions.x = mapsizeX;
        mapDimensions.z = mapSizeZ;
        //int tilesCreatedEachFrame = 1;

        baseGrid = new Tile[mapsizeX, mapSizeZ];

        for (int i = 0; i < peakAmount; i++)
        {
            peakPositions.Add(
                new Vector3(
                    (int)UnityEngine.Random.Range(0, mapsizeX),
                    peakHeight + UnityEngine.Random.Range(-peakHeightRange, peakHeightRange),
                    (int)UnityEngine.Random.Range(0, mapSizeZ
                    )));
        }

        StartCoroutine(_CreateGridRow(0));

        OnGridCreate();
    }

    IEnumerator _CreateGridRow(int i)
    {
        locked = true;

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

        if (i == mapDimensions.x - 1)
        {
            StartCoroutine(_PositionGridRow(i));
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(0.1f);

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
            baseGrid[x, z].CalculateDifficultyLevel();
            baseGrid[x, z].StartSpawnAnimation();

            yield return new WaitForSeconds(0.1f);
        }

        if (x == mapDimensions.x - 1)
        {
            locked = false;
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
        selectedGrids.Add(newGridSelected);
        FindObjectOfType<GridSelect>().SetUIActive(SelectedGrids().Count > 0);
        FindObjectOfType<GridSelect>().GiveTip();
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

        Debug.Log("Start: " + startCoordinates + " End: " + endCoordinates);
        Debug.Log("Size X: " + sizeX + " Size Y: " + sizeY);

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

        endX++;
        sizeX++;
        endY++;
        sizeY++;

        Tile[,] tempGrid = new Tile[sizeX, sizeY];

        if (startCoordinates == endCoordinates)
        {
            tempGrid = new Tile[1, 1];
            tempGrid[0, 0] = getTileByCoordinate((int)startCoordinates.x, (int)startCoordinates.y);
            return tempGrid;
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
        if (selectedGrids.Count == 0 || locked)
            return;

        for (int i = 0; i < SelectedGrids().Count; i++)
        {
            foreach (Tile tile in SelectedGrids()[i])
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
        if (locked)
            return;

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
        int newBiomeIndex = 0;
        for (int i = 0; i < SelectedGrids()[0].GetLength(0); i++)
        {
            for (int j = 0; j < SelectedGrids()[0].GetLength(1); j++)
            {
                if (SelectedGrids()[0][i, j] != null)
                {
                    if (SelectedGrids()[0][i, j].BiomeIndex() > 0)
                    {
                        newBiomeIndex = SelectedGrids()[0][i, j].BiomeIndex();
                    }

                    gridColor = SelectedGrids()[0][i, j].Color();
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

            if (grid[0, 0].BiomeIndex() != newBiomeIndex)
            {
                FindObjectOfType<SetBiome>().ChangeBiomeOnSpecificGrid(grid, newBiomeIndex);
            }

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

    public List<Tile> GetTilesAroundTile(Tile tile, int radius)
    {
        Vector2 coordinates = tile.GetCoordinates();
        List<Tile> tiles = new List<Tile>();

        for (int x = -radius; x < radius; x++)
        {
            for (int y = -radius; y < radius; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                tiles.Add(baseGrid[(int)coordinates.x + x, (int)coordinates.y + y]);
            }
        }

        return tiles;
    }

    public void SetLocked(bool state)
    {
        locked = state;
    }

    [ContextMenu("RecolorGrid")]
    public void Recolor()
    {
        baseGrid = new Tile[(int)mapDimensions.x, (int)mapDimensions.z];

        for (int i = 0; i < mapDimensions.x; i++)
        {
            for (int j = 0; j < mapDimensions.z; j++)
            {
                Debug.Log(i + "," + j);
                baseGrid[i, j] = GameObject.Find((i + 1) + "," + (j + 1)).GetComponent<Tile>();
            }
        }

        foreach (Tile tile in baseGrid)
        {
            tile.ReapplyColor();
        }
    }

    [ContextMenu("CalculateSubgridPercentage")]
    public void CalculateSubgridTypes()
    {
        int totalTiles = 0;
        int plainTiles = 0;
        int forestTiles = 0;
        int seaTiles = 0;
        int desertTiles = 0;
        int objectTiles = 0;

        foreach (Tile tile in baseGrid)
        {
            if (tile.BiomeIndex() == 0)
                plainTiles++;
            else if (tile.BiomeIndex() == 1)
                desertTiles++;
            else if (tile.BiomeIndex() == 2)
                seaTiles++;
            else if (tile.BiomeIndex() == 3)
                forestTiles++;

            if (tile.PlacedObject())
                objectTiles++;

            totalTiles++;
        }

        plainPercentage = (float)plainTiles / (float)totalTiles * 100;
        desertPercentage = (float)desertTiles / (float)totalTiles * 100;
        seaPercentage = (float)seaTiles / (float)totalTiles * 100;
        forestPercentage = (float)forestTiles / (float)totalTiles * 100;
        percentageOfTilesWithObjects = (float)objectTiles / (float)totalTiles * 100;
    }

    public void RotateWithMouse()
    {
        transform.Rotate(Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime * -1, 0, Space.Self);
    }

    public void ScaleWithMouseWheel()
    {
        float value = Input.mouseScrollDelta.y;
        transform.localScale += new Vector3(value, value, value) * Time.deltaTime;
    }
    public Vector2 MapDimensions() => mapDimensions;
    public bool Locked() => locked;
}
