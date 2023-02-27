using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    List<GameObject> highlightedGameObjects = new List<GameObject>();
    [SerializeField] bool creatingSubgrid;
    bool hasStartCoord;
    [SerializeField] Grid baseGrid;
    Tile clickedTile;
    Tile[,] selectedGrid;
    [SerializeField] Color selectedGridColor;

    Vector2 startCoordinates, endCoordinates;

    void Update()
    {
        if (clickedTile)
            clickedTile.UnHighlight();

        clickedTile = GetTileFromMousePos();

        if (!clickedTile) //If no tile was clicked, we end the update
            return;


        if (creatingSubgrid && !Input.GetMouseButton(0))
        {
            clickedTile.Highlight();
        }
        else if (Input.GetMouseButtonDown(0) && creatingSubgrid)
        {
            startCoordinates = clickedTile.GetCoordinates();
            StartCoroutine("CreateSubgrid");
            return;
        }

        if (Input.GetMouseButtonDown(0)) //If we press the left mouse button
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
            else //If we did not click on a tile that is a part of a subgrid
            {
                DeselectSubgrid(); //We try to deselect the currently selected subgrid
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

            foreach (Tile tile in baseGrid.GetBaseGrid())
            {
                tile.UnHighlight();
            }

            if (Physics.Raycast(ray, out tileHit, Mathf.Infinity))
            {
                if (tileHit.transform.gameObject.tag == "tile")
                {
                    endCoordinates = tileHit.transform.gameObject.GetComponent<Tile>().GetCoordinates();
                }
                highlightedTiles = baseGrid.GetTilesBetween(startCoordinates, endCoordinates + new Vector2(1,1));

                foreach (Tile tile in highlightedTiles)
                {
                    tile.Highlight();
                }
            }

            yield return null;
        }

        if (highlightedTiles.Length > 0 || highlightedTiles.LongLength > 0)
        {
            baseGrid.CreateSubgrid(highlightedTiles);
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
        selectedGridColor = Color.white;
        selectedGrid = null;

        baseGrid.DeselectSubgrid();
    }

    public void SelectSubgrid()
    {
        baseGrid.SelectSubgrid(clickedTile.Subgrid());

        selectedGrid = clickedTile.Subgrid();
        selectedGridColor = clickedTile.Color();
    }
}
