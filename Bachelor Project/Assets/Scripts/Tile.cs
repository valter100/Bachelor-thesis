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
    [SerializeField] GameObject placedObject;
    [SerializeField] int biomeIndex;
    int peakHeight;
    bool visited;
    [SerializeField] bool impassable;
    bool highlighted;
    Color highlightedColor;
    Color baseColor;

    Color previousColor;
    Color selectedColor;

    [SerializeField] Vector3 oldPosition;
    [SerializeField] Vector3 oldScale;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        adjacentTiles = new List<Tile>();
    }

    void Start()
    {
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
        transform.localScale = new Vector3(1, Mathf.Clamp(height, 1, Mathf.Infinity), 1);
        transform.position = new Vector3(coordinates.x, height, coordinates.y);

        //oldPosition = transform.position;
        //oldScale = transform.localScale;
    }

    public void SetHeightWithSmoothing(float heightDifference)
    {
        int heightFactor = (int)((grid.MapDimensions().x / peakHeight * 1.4f) / Mathf.Clamp(closestDistance, 1, closestDistance));
        height = (int)(heightFactor * heightScale);

        height = Mathf.Clamp(height, 0, peakHeight) + 1;

        transform.localScale += new Vector3(0, (int)Mathf.Clamp(height, 1, Mathf.Infinity), 0);
        transform.position += new Vector3(0, height - (float)(transform.localScale.y * 0.5), 0);

        //oldPosition = transform.position;
        //oldScale = transform.localScale;
    }

    public void StartSpawnAnimation()
    {
        StartCoroutine(spawnAnimation());
    }

    IEnumerator spawnAnimation()
    {
        float progress = 0;

        Vector3 goalPosition = transform.position;
        transform.position += new Vector3(0, 3, 0);
        Vector3 startPosition = transform.position;

        while (progress < 1)
        {
            if(1 - progress < 0.1)
            {
                progress = 1;
            }

            transform.position = Vector3.Lerp(startPosition, goalPosition, progress);
            transform.localScale = new Vector3(progress, transform.localScale.y, progress);

            progress += Time.deltaTime * 3;
            yield return 0;
        }

        transform.localScale = new Vector3(1, transform.localScale.y, 1);
        transform.position = goalPosition;

        yield return null;
    }

    public void SetColors(Color baseColor, Color highlightedColor)
    {
        this.baseColor = baseColor;
        this.highlightedColor = highlightedColor;
        GetComponent<Renderer>().material.color = baseColor;
        selectedColor = baseColor;
        selectedColor.a /= 2;
    }

    public void SetBiome(int newIndex, bool rememberOldTransform)
    {
        if(!rememberOldTransform)
        {
            if(oldPosition != Vector3.zero && oldScale != Vector3.zero)
            {
                transform.localScale = oldScale;
                transform.position = oldPosition;
            }
        }
        else
        {
            oldScale = transform.localScale;
            oldPosition = transform.position;
        }

        biomeIndex = newIndex;
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
        transform.position -= new Vector3(0, 0.5f, 0);
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

    public void PlaceObjectOnTile(GameObject go)
    {
        GameObject instantiatedGo = Instantiate(go, transform.position + new Vector3(0, transform.localScale.y/2, 0), Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));

        placedObject = instantiatedGo;
        placedObject.transform.parent = transform;
        impassable = true;
        StartCoroutine(PlaceObjects());
    }

    IEnumerator PlaceObjects()
    {
        float progress = 0;

        Vector3 goalPosition = placedObject.transform.position;
        placedObject.transform.position += new Vector3(0, 2, 0);
        Vector3 startPosition = placedObject.transform.position;

        while (progress < 1)
        {
            if (1 - progress < 0.1)
            {
                progress = 1;
            }

            placedObject.transform.position = Vector3.Lerp(startPosition, goalPosition, progress);

            progress += Time.deltaTime * 2;
            yield return 0;
        }


        yield return null;
    }

    public GameObject PlacedObject() => placedObject;

    public void SetImpassable(bool state)
    {
        impassable = state;
    }

    public List<Tile> AdjacentTiles() => adjacentTiles;
    public float Height() => height;
    public bool Highlighted() => highlighted;
    public bool PartOfSubgrid() => partOfSubgrid;
    public Tile[,] Subgrid() => subgrid;
    public Color Color() => GetComponent<Renderer>().material.color;
    public bool Impassable() => impassable;
    public int BiomeIndex() => biomeIndex;

}
