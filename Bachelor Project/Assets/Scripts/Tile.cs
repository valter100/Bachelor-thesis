using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Vector2 coordinates;
    [SerializeField] float height = 0;
    [SerializeField] float heightScale;
    
    List<Tile> adjacentTiles;
    Grid grid;
    Tile[,] subgrid;
    bool partOfSubgrid;

    Vector3 closestPeak;
    float closestDistance;
    GameObject placedObject;
    int biomeIndex;
    int peakHeight;
    
    bool visited;
    bool impassable;
    bool highlighted;

    Color highlightedColor;
    [SerializeField] Color baseColor;
    Color previousColor;
    Color selectedColor;

    Color oldBaseColor;
    Color oldPreviousColor;
    Color oldSelectedColor;

    Vector3 oldPosition;
    Vector3 oldScale;

    //Pathfinding variables
    float fValue;
    float gValue;
    float hValue;
    Tile parent;

    [SerializeField] float difficultyLevel;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        adjacentTiles = new List<Tile>();
        biomeIndex = 0;
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
                if (i == j) continue;

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
        transform.localScale = new Vector3(1, (int)Mathf.Clamp(height, 1, Mathf.Infinity), 1);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(height - (float)(transform.localScale.y * 0.5), 0, Mathf.Infinity), transform.position.z);
        this.height = height;
    }

    public void SetHeightWithSmoothing(float heightDifference)
    {
        int heightFactor = (int)((grid.MapDimensions().x / peakHeight * 1.4f) / Mathf.Clamp(closestDistance, 1, closestDistance));
        height = (int)(heightFactor * heightScale);

        height = Mathf.Clamp(height, 0, peakHeight) + 1;

        transform.localScale += new Vector3(0, (int)Mathf.Clamp(height, 1, Mathf.Infinity), 0);
        transform.position += new Vector3(0, height - (float)(transform.localScale.y * 0.5), 0);
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

    public void SetToOldColor()
    {
        baseColor = oldBaseColor;
        GetComponent<Renderer>().material.color = baseColor;
        selectedColor = oldSelectedColor;
        previousColor = oldPreviousColor;
        selectedColor.a /= 2;
    }

    public void SetColors(Color baseColor, Color highlightedColor)
    {
        oldBaseColor = baseColor;
        oldPreviousColor = previousColor;
        oldSelectedColor = selectedColor;

        this.baseColor = baseColor;
        this.highlightedColor = highlightedColor;
        GetComponent<Renderer>().material.color = baseColor;
        selectedColor = baseColor;
        selectedColor.a /= 2;
    }
    public void SetColor(Color color)
    {
        oldBaseColor = baseColor;
        oldPreviousColor = previousColor;
        oldSelectedColor = selectedColor;

        baseColor = color;
        previousColor = color;
        GetComponent<Renderer>().material.color = color;
        selectedColor = color;
        selectedColor /= 2;
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

    public void ReapplyColor()
    {
        SetColor(baseColor);
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

    public void InstantiateObjectOnTile(GameObject go)
    {
        GameObject instantiatedGo = Instantiate(go, transform.position + new Vector3(0, transform.localScale.y/2, 0), Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));

        placedObject = instantiatedGo;
        placedObject.transform.parent = transform;
        impassable = true;
        StartCoroutine(PlaceObjects(placedObject));
    }

    public void PlaceObjectOnTile(GameObject go, bool _impassable)
    {
        go.transform.position = transform.position + new Vector3(0, transform.localScale.y / 2, 0);
        go.transform.parent = transform;
        impassable = _impassable;
        StartCoroutine(PlaceObjects(go));
    }

    IEnumerator PlaceObjects(GameObject go)
    {
        float progress = 0;

        Vector3 goalPosition = go.transform.position;
        go.transform.position += new Vector3(0, 2, 0);
        Vector3 startPosition = go.transform.position;

        while (progress < 1)
        {
            if (1 - progress < 0.1)
            {
                progress = 1;
            }

            go.transform.position = Vector3.Lerp(startPosition, goalPosition, progress);

            progress += Time.deltaTime * 2;
            yield return 0;
        }


        yield return null;
    }

    public void BumpAnimation()
    {
        StartCoroutine(_BumpAnimation());
    }

    IEnumerator _BumpAnimation()
    {
        float progress = 0;

        Vector3 startPosition = transform.position;
        Vector3 goalPosition = startPosition + new Vector3(0, 0.5f, 0);

        while (progress < 1)
        {
            if (1 - progress < 0.1f)
            {
                progress = 1;
            }

            transform.position = Vector3.Lerp(startPosition, goalPosition, progress);

            progress += Time.deltaTime * 3;
            yield return 0;
        }

        progress = 0;

        (startPosition, goalPosition) = (goalPosition, startPosition);

        while (progress < 1)
        {
            if (1 - progress < 0.1f)
            {
                progress = 1;
            }

            transform.position = Vector3.Lerp(startPosition, goalPosition, progress);

            progress += Time.deltaTime * 3;
            yield return 0;
        }


        yield return null;
    }

    public void CalculateDifficultyLevel()
    {
        difficultyLevel = height;
    }

    public void AddDifficulty(float addedDifficulty)
    {
        difficultyLevel+=addedDifficulty;
    }

    public GameObject PlacedObject() => placedObject;

    public void SetImpassable(bool state)
    {
        impassable = state;
    }
    public void SetF(float value) { fValue = value; }
    public void SetG(float value) { gValue = value; }
    public void SetH(float value) { hValue = value; }
    public void SetParent(Tile newParent) { parent = newParent; }

    public List<Tile> AdjacentTiles() => adjacentTiles;
    public float Height() => height;
    public bool Highlighted() => highlighted;
    public bool PartOfSubgrid() => partOfSubgrid;
    public Tile[,] Subgrid() => subgrid;
    public Color Color() => GetComponent<Renderer>().material.color;
    public bool Impassable() => impassable;
    public int BiomeIndex() => biomeIndex;
    public float F() => fValue;
    public float G() => gValue;
    public float H() => hValue;
    public Tile Parent() => parent;

}
