using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigCave : MonoBehaviour
{
    [SerializeField] GameObject caveOutObject;

    public void SpawnTool()
    {
        Instantiate(caveOutObject);
    }
}
