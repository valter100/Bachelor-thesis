using System.Collections;
using System.Collections.Generic;
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
    bool perlin;


    [SerializeField] Color baseTileColor;
    [SerializeField] Color highlightedTileColor;
    [SerializeField] Color subgridColor;
    [SerializeField] AI ai;
    // Start is called before the first frame update
    void Start()
    {
        subgridColor = Random.ColorHSV();
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
                subgridList.Clear();
                Destroy(tile.gameObject);
            }
        }

        baseGrid = new Tile[(int)mapDimensions.x, (int)mapDimensions.z];

        for (int i = 0; i < mapDimensions.x; i++)
        {
            for (int j = 0; j < mapDimensions.z; j++)
            {
                baseGrid[i, j] = Instantiate(tile, new Vector3(i, 0, j) + new Vector3(i * tileBuffer, 0, j * tileBuffer), Quaternion.identity).GetComponent<Tile>(); //Add Height later
                baseGrid[i, j].transform.parent = gameObject.transform;
                baseGrid[i, j].gameObject.name = (i + 1) + "," + (j + 1);
                baseGrid[i, j].SetCoordinates(i, j);
                baseGrid[i, j].SetColors(baseTileColor, highlightedTileColor);
            }
        }

        foreach (Tile tile in baseGrid)
        {
            tile.SetAdjacentTiles(this);
            tile.SetHeightWithSmoothing(perlin, 0);
        }


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

        subgridColor = Random.ColorHSV();

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
        FindObjectOfType<GridSelect>().GiveTip();

    }

    public List<Tile[,]> SelectedGrids()
    {
        List<Tile[,]> tempList = selectedGrids;

        tempList.Remove(baseGrid);

        return tempList;
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
    }

    public void SmoothGrid()
    {
        foreach (Tile[,] subgrid in selectedGrids)
        {
            foreach (Tile tile in subgrid)
                tile.SetHeightWithSmoothing(false, 0);
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
}
