using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    List<GameObject> highlightedGameObjects= new List<GameObject>();
    bool checkForInput;
    bool hasStartCoord;
    [SerializeField] Grid grid;

    Vector2 startCoordinates, endCoordinates;

    void Start()
    {
        checkForInput = false;
    }

    void Update()
    {
        if (checkForInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(!hasStartCoord)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit tileHit;

                    if (Physics.Raycast(ray, out tileHit, Mathf.Infinity))
                    {
                        if(tileHit.transform.gameObject.tag == "tile")
                        startCoordinates = tileHit.transform.gameObject.GetComponent<Tile>().GetCoordinates();
                        Debug.Log("startCoordinates are: " + startCoordinates); 
                        hasStartCoord = true;
                    }
                }
                else if(hasStartCoord)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit tileHit;

                    if (Physics.Raycast(ray, out tileHit, 100))
                    {
                        if (tileHit.transform.gameObject.tag == "tile")
                        endCoordinates = tileHit.transform.gameObject.GetComponent<Tile>().GetCoordinates();
                        Debug.Log("endCoordinates are: " + endCoordinates);
                    }

                    grid.CreateSubgrid(startCoordinates, endCoordinates);
                    checkForInput = false;
                    hasStartCoord = false;
                }
            }
        }
    }

    public void CheckForInput() => checkForInput = true;

    public Vector2 GetStartCoordinates() => startCoordinates;
    public Vector2 GetEndCoordinates() => endCoordinates;
}
