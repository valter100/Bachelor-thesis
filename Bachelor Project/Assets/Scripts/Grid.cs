using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject tile;
    [SerializeField] Vector3 mapDimensions;
    [SerializeField] float tileBuffer;
    Tile[,] grid;
    bool perlin;

    [SerializeField] Color baseTileColor;
    [SerializeField] AI ai;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CreateGrid()
    {
        if(grid != null)
        {
            foreach(Tile tile in grid)
            {
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
                grid[i, j].SetColor(baseTileColor);
            }
        }

        foreach (Tile tile in grid)
        {
            tile.SetAdjacentTiles(this);
            tile.SetHeight(perlin);
        }

        ai.GiveTip();

        //Create subgrids that spans a part of the grid that allows different settings
        //Create the subgrid yourself by dragging or having the assistant provide one for you
        //Mountainous grid that has higher height span to create more extreme terrain
    }

    public Tile[,] GetGrid() => grid;
    public Tile getTileByCoordinate(int x, int y) => grid[x, y];
}
