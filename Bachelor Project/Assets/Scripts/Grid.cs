using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject tile;
    [SerializeField] Vector3 mapDimensions;
    [SerializeField] float tileBuffer;
    Tile[,] grid;
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
        ai.GiveTip(0);
        subgridColor = Random.ColorHSV();
    }

    private void Update()
    {

    }

    public void CreateGrid()
    {
        if(grid != null)
        {
            foreach(Tile tile in grid)
            {
                subgridList.Clear();
                Destroy(tile.gameObject);
            }
        }

        grid = new Tile[(int)mapDimensions.x, (int)mapDimensions.z];

        for (int i = 0; i < mapDimensions.x; i++)
        {
            for (int j = 0; j < mapDimensions.z; j++)
            {
                grid[i, j] = Instantiate(tile, new Vector3(i, 0, j) + new Vector3(i * tileBuffer, 0, j * tileBuffer), Quaternion.identity).GetComponent<Tile>(); //Add Height later
                grid[i, j].transform.parent = gameObject.transform;
                grid[i, j].gameObject.name = (i + 1) + "," + (j + 1);
                grid[i, j].SetCoordinates(i, j);
                grid[i, j].SetColors(baseTileColor, highlightedTileColor);
            }
        }

        foreach (Tile tile in grid)
        {
            tile.SetAdjacentTiles(this);
            tile.SetHeight(perlin);
        }

        ai.GiveTip(1);
    }

    public void CreateSubgrid(Vector2 startCoordinates, Vector2 endCoordinates)
    {
        Tile[,] subgrid = GetTilesBetween(startCoordinates, endCoordinates);

        foreach(Tile tile in subgrid)
        {
            if(tile.PartOfSubgrid())
            {
                tile.RemoveFromSubgrid();
            }

            tile.SetColor(subgridColor);
            tile.SetPartOfSubgrid(subgrid);
        }

        subgridColor = Random.ColorHSV();

        subgridList.Add(subgrid);
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

    public Tile[,] GetGrid() => grid;
    public Tile getTileByCoordinate(int x, int y) => grid[x, y];

    public void SetGridDimension(int x, int y, int z)
    {
        mapDimensions = new Vector3(x, y, z);
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
        
        if(startX > endX)
        {
            (startX, endX) = (endX, startX);
        }

        if(startY > endY)
        {
            (startY, endY) = (endY, startY);
        }

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                tempGrid[i, j] = grid[x, y];
                j++;
            }
            i++;
            j = 0;
        }

        return tempGrid;
    }
}
