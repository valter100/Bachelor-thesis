using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FillSack : MonoBehaviour
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

                if (!clickedTile.PlacedObject() && !clickedTile.Impassable())
                {
                    transform.position = clickedTile.transform.position + new Vector3(0, clickedTile.transform.localScale.y / 2, 0) + new Vector3(0, transform.localScale.y / 2, 0);
                    if (Input.GetMouseButtonDown(0))
                    {
                        Fill(clickedTile);
                    }
                }
            }
        }

    }

    public void Fill(Tile tile)
    {
        tile.SetHeight(tile.Height() + 1);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

