using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceStartAndGoal : Step
{
    [SerializeField] Color startColor;
    [SerializeField] Color goalColor;

    [SerializeField] GameObject startObject;
    [SerializeField] GameObject goalObject;

    public void PlaceStart()
    {
        if (GameObject.Find("Start"))
        {
            Destroy(GameObject.Find("Start"));
        }

        GameObject start = Instantiate(startObject);
        start.name = "Start";

        grid.SetLocked(true);
    }

    public void PlaceGoal()
    {
        if (GameObject.Find("Goal"))
        {
            Destroy(GameObject.Find("Goal"));
        }
        GameObject goal = Instantiate(goalObject);
        goal.name = "Goal";
    }

    public Color StartColor() => startColor;
    public Color GoalColor => goalColor;
}
