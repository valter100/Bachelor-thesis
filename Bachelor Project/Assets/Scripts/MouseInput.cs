using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour
{
    List<GameObject> highlightedGameObjects = new List<GameObject>();
    [SerializeField] bool creatingSubgrid;
    bool hasStartCoord;
    [SerializeField] Grid baseGrid;
    Tile clickedTile;
    List<Tile[,]> selectedGrids = new List<Tile[,]>();
    [SerializeField] Color selectedGridColor;

    AI clappy;

    Vector2 startCoordinates, endCoordinates;

    private void Start()
    {
        clappy = FindObjectOfType<AI>();
    }

    void Update()
    {
        if (baseGrid.Locked())
            return;

        if (clickedTile)
            clickedTile.UnHighlight();

        clickedTile = GetTileFromMousePos();

        if (!clickedTile) //If no tile was clicked, we end the update
            return;

        if (creatingSubgrid && !Input.GetMouseButton(0))
        {
            clickedTile.Highlight();
        }

        if (Input.GetMouseButtonDown(0) && creatingSubgrid)
        {
            startCoordinates = clickedTile.GetCoordinates();
            StartCoroutine("CreateSubgrid");
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            creatingSubgrid = false;
            return;
        }

        if (Input.GetMouseButtonDown(0) && !GameObject.FindGameObjectWithTag("Tool")) //If we press the left mouse button
        {
            if (clickedTile.PartOfSubgrid()) //And the tile we clicked is part of a subgrid
            {
                if (selectedGrids.Contains(clickedTile.Subgrid()))
                {
                    DeselectSubgrid(); //We deselect the grid and end the update
                    return;
                }
                else if (!selectedGrids.Contains(clickedTile.Subgrid()) && Input.GetKey(KeyCode.LeftShift))
                {
                    SelectSubgrid();
                }
                else if (!selectedGrids.Contains(clickedTile.Subgrid()) && !Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectSubgrid();
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
        bool canceled = false;

        while (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit tileHit;

            foreach (Tile tile in baseGrid.GetBaseGrid())
            {
                tile.UnHighlight();
            }

            if (Input.GetMouseButtonDown(1))
            {
                canceled = true;
                break;
            }

            if (Physics.Raycast(ray, out tileHit, Mathf.Infinity))
            {
                if (tileHit.transform.gameObject.tag == "tile")
                {
                    endCoordinates = tileHit.transform.gameObject.GetComponent<Tile>().GetCoordinates();
                }
                highlightedTiles = baseGrid.GetTilesBetween(startCoordinates, endCoordinates/* + new Vector2(1, 1)*/);

                foreach (Tile tile in highlightedTiles)
                {
                    tile.Highlight();
                }
            }

            yield return null;
        }

        if (highlightedTiles.Length > 0 || highlightedTiles.LongLength > 0)
        {
            if (!canceled)
                baseGrid.CreateSubgrid(highlightedTiles, true);
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
            if (tileHit.transform.gameObject.tag == "tile" && !EventSystem.current.IsPointerOverGameObject())
            {
                Tile clickedTile = tileHit.transform.gameObject.GetComponent<Tile>();

                return clickedTile;
            }
        }

        return null;
    }

    public void CreatingSubgrid() => creatingSubgrid = true;

    public Vector2 GetStartCoordinates() => startCoordinates;
    public Vector2 GetEndCoordinates() => endCoordinates;

    public void DeselectSubgrid()
    {
        selectedGridColor = Color.white;
        selectedGrids.Clear();

        baseGrid.DeselectSubgrids();
    }

    public void SelectSubgrid()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        baseGrid.SelectSubgrid(clickedTile.Subgrid());

        selectedGrids.Add(clickedTile.Subgrid());
        selectedGridColor = clickedTile.Color();
    }
}
