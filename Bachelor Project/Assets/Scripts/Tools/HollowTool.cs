using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HollowTool : MonoBehaviour
{
    private void OnEnable()
    {
        Step.OnStepStart += DestroyObject;
    }

    private void OnDisable()
    {
        Step.OnStepStart -= DestroyObject;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit tileHit;

        if (Physics.Raycast(ray, out tileHit, Mathf.Infinity))
        {
            if (tileHit.transform.gameObject.tag == "tile" && !EventSystem.current.IsPointerOverGameObject())
            {
                Tile clickedTile = tileHit.transform.gameObject.GetComponent<Tile>();

                transform.position = new Vector3(clickedTile.transform.position.x, (int)tileHit.point.y + 0.5f, clickedTile.transform.position.z);

                if (Input.GetMouseButtonDown(0))
                {
                    HollowCave(clickedTile, tileHit.point);
                    return;
                }

            }
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void HollowCave(Tile tile, Vector3 impactPoint)
    {
        Vector3 tilePosition = tile.transform.position;
        float yScale = tile.transform.localScale.y;

        float tileTopPosition = tile.transform.position.y + tile.transform.localScale.y / 2;
        float tileBotPosition = tile.transform.position.y + tile.transform.localScale.y / 2 - tile.Height();


        float heightDistribution = transform.position.y / tile.Height();
        float lowerScale = (1 - (1 - heightDistribution)) * yScale - 0.5f;
        float upperScale = (1 - heightDistribution) * yScale - 0.5f;

        if (upperScale >= 1)
        {
            GameObject upperObject = Instantiate(tile.gameObject, transform.position, Quaternion.identity);
            upperObject.transform.localScale = new Vector3(upperObject.transform.localScale.x, upperScale, upperObject.transform.localScale.z);
            upperObject.transform.position = new Vector3(upperObject.transform.position.x, tileTopPosition - upperScale / 2, upperObject.transform.position.z);
            upperObject.name = tile.name + ": upper";

            Tile upperTile = upperObject.GetComponent<Tile>();
            upperTile.SetHeightWithoutTransform(upperObject.transform.position.y + upperObject.transform.localScale.y / 2);
            upperTile.SetColor(tile.Color());
            upperTile.SetCoordinates((int)tile.GetCoordinates().x, (int)tile.GetCoordinates().y);
            Grid grid = FindObjectOfType<Grid>();
            grid.AddToBaseGrid(upperTile);

            foreach(Tile adjacentTile in tile.AdjacentTiles())
            {
                upperTile.AdjacentTiles().Add(adjacentTile);

                if(!adjacentTile.AdjacentTiles().Contains(upperTile))
                {
                    adjacentTile.AdjacentTiles().Add(upperTile);
                }
            }
        }

        if (lowerScale >= 1)
        {
            GameObject lowerObject = Instantiate(tile.gameObject, transform.position, Quaternion.identity);
            lowerObject.transform.localScale = new Vector3(lowerObject.transform.localScale.x, lowerScale, lowerObject.transform.localScale.z);
            lowerObject.transform.position = new Vector3(lowerObject.transform.position.x, tileBotPosition + lowerScale / 2, lowerObject.transform.position.z);
            lowerObject.name = tile.name + ": lower";

            Tile lowerTile = lowerObject.GetComponent<Tile>();
            lowerTile.SetHeightWithoutTransform(lowerObject.transform.position.y + lowerObject.transform.localScale.y / 2);
            lowerTile.SetColor(tile.Color());
        }

        Destroy(tile.gameObject);
    }
}
