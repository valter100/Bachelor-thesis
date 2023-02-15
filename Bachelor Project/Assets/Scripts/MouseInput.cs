using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    List<GameObject> highlightedGameObjects = new List<GameObject>();
    [SerializeField] bool checkForInput;
    bool hasStartCoord;
    [SerializeField] Grid grid;

    Vector2 startCoordinates, endCoordinates;

    void Start()
    {
        checkForInput = false;
    }

    void Update()
    {
        if (!checkForInput)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit tileHit;

            if (Physics.Raycast(ray, out tileHit, Mathf.Infinity))
            {
                if (tileHit.transform.gameObject.tag == "tile")
                {
                    startCoordinates = tileHit.transform.gameObject.GetComponent<Tile>().GetCoordinates();
                    StartCoroutine("CreateSubgrid");
                }
                Debug.Log("startCoordinates are: " + startCoordinates);
            }
            else
                return;
        }

    }

    public IEnumerator CreateSubgrid()
    {
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
                Tile[,] highlightedTiles = grid.GetTilesBetween(startCoordinates, endCoordinates);

                foreach (Tile tile in highlightedTiles)
                {
                    tile.Highlight();
                }

            }

            yield return null;
        }

        grid.CreateSubgrid(startCoordinates, endCoordinates);

        checkForInput = false;

        yield return 0;
    }

    public void CheckForInput() => checkForInput = true;

    public Vector2 GetStartCoordinates() => startCoordinates;
    public Vector2 GetEndCoordinates() => endCoordinates;
}
