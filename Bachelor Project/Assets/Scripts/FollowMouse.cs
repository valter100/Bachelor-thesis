using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] bool placed;
    [SerializeField] bool isStart;

    Tile clickedTile;

    private void OnEnable()
    {
        Step.OnStepStart += DestroyUnplaced;
    }

    private void OnDisable()
    {
        Step.OnStepStart -= DestroyUnplaced;
    }

    void Update()
    {
        if (placed)
            return;

        if(Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit tileHit;

        if (Physics.Raycast(ray, out tileHit, Mathf.Infinity))
        {
            if (tileHit.transform.gameObject.tag == "tile" && !EventSystem.current.IsPointerOverGameObject())
            {
                clickedTile = tileHit.transform.gameObject.GetComponent<Tile>();

                if (!clickedTile.PlacedObject() && !clickedTile.Impassable())
                {
                    transform.position = clickedTile.transform.position + new Vector3(0, clickedTile.transform.localScale.y / 2, 0) + new Vector3(0, transform.localScale.y / 2, 0);

                    if (Input.GetMouseButtonDown(0))
                    {
                        SetPlaced(true);
                        clickedTile.PlaceObjectOnTile(gameObject, false);
                    }

                    if (placed && isStart)
                    {
                        FindObjectOfType<PlaceStartAndGoal>().PlaceGoal();
                        FindObjectOfType<Pathfind>().SetStartTile(clickedTile);
                    }
                    else if (placed && !isStart)
                    {
                        FindObjectOfType<Pathfind>().SetEndTile(clickedTile);
                        FindObjectOfType<Pathfind>().PathfindStartToGoal();
                    }
                }
            }
        }

    }
    public void SetPlaced(bool state)
    {
        placed = state;
        if (state == true)
            Step.OnStepStart -= DestroyUnplaced;
    }

    public void DestroyUnplaced()
    {
        Destroy(gameObject);
    }
}
