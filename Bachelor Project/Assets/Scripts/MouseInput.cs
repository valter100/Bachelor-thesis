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

    Tile[,] selectedGrid;

    Vector2 startCoordinates, endCoordinates;

    void Start()
    {
        creatingSubgrid = false;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (selectedGrid != null)
            {
                foreach (Tile tile in selectedGrid)
                {
                    if (tile != null)
                        tile.Deselect();
                }
            }
        }

        Tile clickedTile = GetTileFromMousePos();

        if (!clickedTile)
            return;

        if (Input.GetMouseButtonDown(0) && creatingSubgrid)
        {
            startCoordinates = clickedTile.GetCoordinates();
            StartCoroutine("CreateSubgrid");
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (clickedTile.PartOfSubgrid())
            {
                selectedGrid = clickedTile.Subgrid(); //Work from here

                foreach (Tile tile in selectedGrid)
                {
                    if (tile != null)
                        tile.Select();
                }
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
}
