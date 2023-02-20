using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    List<GameObject> highlightedGameObjects = new List<GameObject>();
    [SerializeField] bool creatingSubgrid;
    bool hasStartCoord;
    [SerializeField] Grid grid;
    Tile clickedTile;
    Tile[,] selectedGrid;
    [SerializeField] Color selectedGridColor;

    Vector2 startCoordinates, endCoordinates;

    void Start()
    {
        creatingSubgrid = false;
    }

    void Update()
    {
        clickedTile = GetTileFromMousePos();

        if (!clickedTile) //If no tile was clicked, we end the update
            return;

        if (Input.GetMouseButtonDown(0) && creatingSubgrid)
        {
            startCoordinates = clickedTile.GetCoordinates();
            StartCoroutine("CreateSubgrid");
        }
        else if (Input.GetMouseButtonDown(0)) //If we press the left mouse button
        {
            if (clickedTile.PartOfSubgrid()) //And the tile we clicked is part of a subgrid
            {
                if (clickedTile.Subgrid() == selectedGrid) //and the tile's subgrid is the already selected grid
                {
                    DeselectSubgrid(); //We deselect the grid and end the update
                    return;
                }
                else
                {
                    SelectSubgrid(); //We select the tile's subgrid
                }
            }
            else
            {
                DeselectSubgrid();
            }
        }
    }

    public IEnumerator CreateSubgrid()
    {
        Tile[,] highlightedTiles = null;

        while (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit tileHit;

            foreach (Tile tile in grid.GetGrid())
            {
                tile.UnHighlight();
            }

            if (Physics.Raycast(ray, out tileHit, Mathf.Infinity))
            {
                if (tileHit.transform.gameObject.tag == "tile")
                {
                    endCoordinates = tileHit.transform.gameObject.GetComponent<Tile>().GetCoordinates();
                }
                highlightedTiles = grid.GetTilesBetween(startCoordinates, endCoordinates);

                foreach (Tile tile in highlightedTiles)
                {
                    tile.Highlight();
                }
            }

            yield return null;
        }

        if (highlightedTiles.Length > 0)
        {
            grid.CreateSubgrid(startCoordinates, endCoordinates);
        }
        else
        {

        }

        creatingSubgrid = false;

        yield return 0;
    }

    public Tile GetTileFromMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit tileHit;

        if (Physics.Raycast(ray, out tileHit, Mathf.Infinity))
        {
            if (tileHit.transform.gameObject.tag == "tile")
            {
                Tile clickedTile = tileHit.transform.gameObject.GetComponent<Tile>();

                return clickedTile;
            }
        }

        return null;
    }

    public void CheckForInput() => creatingSubgrid = true;

    public Vector2 GetStartCoordinates() => startCoordinates;
    public Vector2 GetEndCoordinates() => endCoordinates;

    public void DeselectSubgrid()
    {
        if (selectedGrid == null)
            return;

        foreach (Tile tile in selectedGrid)
        {
            if (tile != null)
                tile.Deselect();
        }
        selectedGridColor = Color.white;
        selectedGrid = null;
    }

    public void SelectSubgrid()
    {
        DeselectSubgrid();

        selectedGrid = clickedTile.Subgrid(); //Work from here
        foreach (Tile tile in selectedGrid)
        {
            if (tile != null)
                tile.Select();
        }
        selectedGridColor = clickedTile.Color();
    }
}
