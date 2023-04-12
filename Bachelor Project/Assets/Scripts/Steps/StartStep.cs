using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStep : Step
{
    int[] mapSizePreferense = new int[3];
    int[] peakPreferenses = new int[4];


    void Start()
    {
        base.Start();
        StartCoroutine(ActivateClappy(0.1f));
    }

    IEnumerator ActivateClappy(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Activate clappy and animate him to appear
        LoadPreferences();
        GiveTip();
    }

    protected override void SetText()
    {
        question = "Hey there! I see you are trying to create some terrain! Would you like some help with that?";
        optOne = "Please help me Clappy!";
        optTwo = "Get out of my face!";
        optThree = "I'll let you live, but you're on thin ice...";
        base.SetText();
    }

    public override void GiveTip()
    {
        base.GiveTip();
    }

    void LoadPreferences()
    {
        mapSizePreferense[0] = textHandler.GetPreferences("mapSizeX");
        mapSizePreferense[1] = textHandler.GetPreferences("mapSizeZ");
        mapSizePreferense[2] = textHandler.GetPreferenceAmount("mapSizeX");

        peakPreferenses[0] = textHandler.GetPreferences("peakHeight");
        peakPreferenses[1] = textHandler.GetPreferences("peakHeightRange");
        peakPreferenses[2] = textHandler.GetPreferences("peakAmount");
        peakPreferenses[3] = textHandler.GetPreferenceAmount("peakAmount");
    }

    public override void DoAction(int actionIndex)
    {
        if (actionIndex == 0)
        {
            int mapSizeX = mapSizePreferense[0] / mapSizePreferense[2];
            int mapSizeZ = mapSizePreferense[1] / mapSizePreferense[2];

            int peakHeight = peakPreferenses[0] / peakPreferenses[3];
            int peakHeightRange = peakPreferenses[1] / peakPreferenses[3];
            int peakAmount = peakPreferenses[2] / peakPreferenses[3];

            grid.CreateGrid(mapSizeX, mapSizeZ, peakHeight, peakHeightRange, peakAmount);

            //Debug.Log("mapx " + mapSizeX);
            //Debug.Log("mapz " + mapSizeZ);
            //Debug.Log("peak height " + peakHeight);
            //Debug.Log("peak height range " + peakHeightRange);
            //Debug.Log("peak anount " + peakAmount);
        }
        if (actionIndex == 1)
        {
            clappy.SetInactive();
        }
        if (actionIndex == 2)
        {
            
        }
    }
}
