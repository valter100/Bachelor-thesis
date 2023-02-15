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
    [SerializeField] AI ai;
    [SerializeField] MouseInput mouseInput;
    // Start is called before the first frame update
    void Start()
    {
        ai.GiveTip(0);
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

        //Create subgridList that spans a part of the grid that allows different settings
        //Create the subgrid yourself by dragging or having the assistant provide one for you
        //Mountainous grid that has higher height span to create more extreme terrain
    }

    public void CreateSubgrid(Vector2 startCoordinates, Vector2 endCoordinates)
    {
        Tile[,] subgrid = GetTilesBetween(startCoordinates, endCoordinates);

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
