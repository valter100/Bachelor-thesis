using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Vector2 coordinates;
    [SerializeField] float height = 0;
    [SerializeField] List<Tile> adjacentTiles;
    [SerializeField] Grid grid;
    Tile[,] subgrid;
    [SerializeField] float heightScale;
    [SerializeField] bool partOfSubgrid;

    [SerializeField] Vector3 closestPeak;
    [SerializeField] float closestDistance;
    int peakHeight;
    bool visited;
    bool highlighted;
    Color highlightedColor;
    Color baseColor;

    Color previousColor;
    Color selectedColor;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        adjacentTiles = new List<Tile>();
    }

    void Start()
    {
        GetComponent<Animator>().Play("Created");
        previousColor = baseColor;
    }

    public void SetAdjacentTiles(Grid grid)
    {
        for (int i = -1; i <= 0; i++)
        {
            for (int j = -1; j <= 0; j++)
            {
                if (i == 0 && j == 0) continue;

                try
                {
                    Tile tile = grid.getTileByCoordinate((int)coordinates.x + i, (int)coordinates.y + j);

                    adjacentTiles.Add(tile);

                    if (!tile.AdjacentTiles().Contains(this))
                    {
                        tile.AdjacentTiles().Add(this);
                    }
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

    public void SetHeight(float height)
    {
        //height = Mathf.RoundToInt(height);

        transform.localScale = new Vector3(1, Mathf.Clamp(height, 1, Mathf.Infinity), 1);
        transform.position = new Vector3(coordinates.x, height, coordinates.y);
    }

    public void SetHeightWithSmoothing(float heightDifference)
    {
        //height = Random.Range(0, 10) * heightScale;

        //if (adjacentTiles.Count > 0) // if the tile has adjacent tiles
        //{
        //    float totalHeight = 0;
        //    int tilesCounted = 0;
        //    foreach (Tile tile in adjacentTiles)
        //    {
        //        //if (tile.subgrid != subgrid) //Skip if the tiles are not in the same subgrid
        //        //    continue;

        //        //foreach (Tile adjacentTile in tile.adjacentTiles)
        //        //{
        //        //    if (tile.height > 0)
        //        //    {
        //        //        totalHeight += tile.Height();
        //        //        tilesCounted++;
        //        //    }
        //        //}

        //        if (tile.height > 0)
        //        {
        //            totalHeight += tile.Height();
        //            tilesCounted++;
        //        }
        //    }


        //    float averageHeight = totalHeight / tilesCounted;

        //    float clampLow = averageHeight - heightScale;
        //    float clampHigh = averageHeight + heightScale;

        //    height = Mathf.Clamp(height, clampLow, clampHigh);
        //    height += heightDifference;

        //    height = Mathf.RoundToInt(height);
        //}

        int heightFactor = (int)((grid.MapDimensions().x / 3) / Mathf.Clamp(closestDistance, 1, closestDistance));

        height = (int)(heightFactor * heightScale);

        height = Mathf.Clamp(height, 0, peakHeight);
        height++;

        transform.localScale += new Vector3(0, (int)Mathf.Clamp(height, 1, Mathf.Infinity), 0);
        transform.position += new Vector3(0, height, 0);
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
        baseColor = color;
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
        transform.position += new Vector3(0, 0.5f, 0);
    }

    public void Deselect()
    {
        GetComponent<Renderer>().material.color = previousColor;
        transform.position = new Vector3(transform.position.x, height / 2, transform.position.z);
    }

    public void SetPartOfSubgrid(Tile[,] _subgrid)
    {
        partOfSubgrid = true;
        subgrid = _subgrid;
    }

    public void RemoveFromSubgrid()
    {
        for (int i = 0; i < subgrid.GetLength(0); i++)
        {
            for (int j = 0; j < subgrid.GetLength(1); j++)
            {
                if (subgrid[i, j] == this)
                {
                    subgrid[i, j] = null;
                }
            }
        }
    }

    public void FindClosestPeak(List<Vector3> peakList)
    {
        closestDistance = float.MaxValue;

        foreach (Vector3 peakPosition in peakList)
        {
            float distance = Vector2.Distance(coordinates, new Vector2(peakPosition.x, peakPosition.z));

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPeak = peakPosition;
                peakHeight = (int)peakPosition.y;
            }
        }
    }
    //public void DeactiveAnimator()
    //{
    //    GetComponent<Animator>().enabled = false;
    //}

    public List<Tile> AdjacentTiles() => adjacentTiles;
    public float Height() => height;
    public bool Highlighted() => highlighted;
    public bool PartOfSubgrid() => partOfSubgrid;
    public Tile[,] Subgrid() => subgrid;
    public Color Color() => GetComponent<Renderer>().material.color;

}
