using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SledgeHammer : MonoBehaviour
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

                if (!clickedTile.PlacedObject())
                {
                    transform.position = clickedTile.transform.position + new Vector3(0, clickedTile.transform.localScale.y / 2, 0) + new Vector3(0, transform.localScale.y / 2, 0);
                }
                else if(clickedTile.PlacedObject())
                {
                    transform.position = clickedTile.PlacedObject().transform.position + new Vector3(0, clickedTile.PlacedObject().GetComponent<Renderer>().bounds.size.y, 0) + new Vector3(0, transform.localScale.y / 2, 0);
                    if (Input.GetMouseButtonDown(0))
                    {
                        DestroyPlacedObject(clickedTile.PlacedObject());
                        clickedTile.SetImpassable(false);
                    }
                }
            }
        }

    }

    public void DestroyPlacedObject(GameObject placedObject)
    {
        Destroy(placedObject);
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
