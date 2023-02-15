using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Vector2 coordinates;
    float height = 0;
    [SerializeField] List<Tile> adjacentTiles;
    [SerializeField] Grid grid;
    [SerializeField] float heightScale;
    bool visited;

    private void Awake()
    {
        adjacentTiles = new List<Tile>();
    }

    void Start()
    {
        grid = FindObjectOfType<Grid>();
    }

    public void SetAdjacentTiles(Grid grid)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                try
                {
                    adjacentTiles.Add(grid.getTileByCoordinate((int)coordinates.x + i, (int)coordinates.y + j));
                }
                catch
                {

                }
            }
        }
    }

    public void SetCoordinates(int x, int y)
    {
        coordinates = new Vector2(x, y);
    }

    public void SetHeight(bool perlin)
    {
        if (perlin)
            height = Mathf.PerlinNoise(coordinates.x, coordinates.y) * heightScale;
        else
            height = Random.Range(0, 10) * heightScale;

        if(adjacentTiles.Count > 0)
        {
            float totalHeight = 0;
            int tilesCounted = 0;
            foreach(Tile tile in adjacentTiles)
            {
                foreach(Tile adjacentTile in tile.adjacentTiles)
                {
                    if(tile.height > 0)
                    {
                        totalHeight += tile.Height();
                        tilesCounted++;
                    }
                }
            }

            float averageHeight = totalHeight / tilesCounted;

            float clampLow =  averageHeight - heightScale;
            float clampHigh = averageHeight + heightScale;

            height = Mathf.Clamp(height, clampLow, clampHigh);
        }

        transform.localScale += new Vector3(0, height, 0);
        transform.position += new Vector3(0, height / 2, 0);
    }
    public void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
    public List<Tile> AdjacentTiles() => adjacentTiles;
    public float Height() => height;
    
}
