using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferencePicker : MonoBehaviour
{
    [SerializeField] List<Grid> grids = new List<Grid>();
    Grid currentGrid;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ActivateNewGrid();
        }
    }

    public void ActivateNewGrid()
    {
        if(currentGrid)
            currentGrid.gameObject.SetActive(false);

        int randomIndex = Random.Range(0, grids.Count);

        grids[randomIndex].gameObject.SetActive(true);
        currentGrid = grids[randomIndex];
        grids.RemoveAt(randomIndex);
    }
}
