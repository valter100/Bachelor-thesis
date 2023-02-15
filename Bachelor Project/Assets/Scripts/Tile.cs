using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Vector2 coordinates;
    float height = 0;
    [SerializeField] List<Tile> adjacentTiles;
    [SerializeField] Grid grid;
    Tile[,] subgrid;
    [SerializeField] float heightScale;
    [SerializeField] bool partOfSubgrid;
    bool visited;
    bool highlighted;
    Color highlightedColor;
    Color baseColor;

    Color previousColor;
    Color selectedColor;

    private void Awake()
    {
        adjacentTiles = new List<Tile>();
    }

    void Start()
    {
        grid = FindObjectOfType<Grid>();
        previousColor = baseColor;
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

    public Vector2 GetCoordinates() => coordinates; 

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
    public void SetColors(Color baseColor, Color highlightedColor)
    {
        this.baseColor = baseColor;
        this.highlightedColor = highlightedColor;
        GetComponent<Renderer>().material.color = baseColor;
        selectedColor = baseColor;
        selectedColor.a /= 2;
    }

    public void SetColor(Color color)
    {
        previousColor = color;
        GetComponent<Renderer>().material.color = color;
        selectedColor = color;
        selectedColor /= 2;
    }

    public void Highlight()
    {
        previousColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = highlightedColor;
        highlighted = true;
    }

    public void UnHighlight()
    {
        GetComponent<Renderer>().material.color = previousColor;
        highlighted = false;
    }

    public void Select()
    {
        //previousColor = GetComponent<Renderer>().material.color;
        transform.position += new Vector3(0, 0.5f, 0);
        GetComponent<Animator>().SetBool("Selected", true);
        //GetComponent<Renderer>().material.color = selectedColor;
    }

    public void Deselect()
    {
        GetComponent<Renderer>().material.color = previousColor;
        transform.position = new Vector3(transform.position.x, height / 2, transform.position.z);
        GetComponent<Animator>().SetBool("Selected", false);
    }

    public void SetPartOfSubgrid(Tile[,] _subgrid)
    {
        partOfSubgrid = true;
        subgrid = _subgrid;
    }

    public void RemoveFromSubgrid()
    {
        for(int i = 0; i < subgrid.GetLength(0); i++)
        {
            for(int j = 0; j < subgrid.GetLength(1); j++)
            {
                if (subgrid[i,j] == this)
                {
                    subgrid[i, j] = null;
                }
            }
        }
    }
    public List<Tile> AdjacentTiles() => adjacentTiles;
    public float Height() => height;
    public bool Highlighted() => highlighted;
    public bool PartOfSubgrid() => partOfSubgrid;
    public Tile[,] Subgrid() => subgrid;

}
