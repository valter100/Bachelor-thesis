using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StartStep : Step
{

    void Start()
    {
        base.Start();
        StartCoroutine(ActivateClappy(0.1f));
    }

    IEnumerator ActivateClappy(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Activate clappy and animate him to appear
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

    public override void DoAction(int actionIndex)
    {
        if (actionIndex == 0)
        {
            
            CreateNewGridWithPreferences();
            clappy.SetStep(1);
        }
        if (actionIndex == 1)
        {
            clappy.SetInactive();
        }
        if (actionIndex == 2)
        {
            
        }
    }

    public void CreateNewGridWithPreferences()
    {
        FindObjectOfType<CreateGrid>().SetUIActive(true);
        int mapSizeX = preferenceHandler.mapSizeXPref;
        int mapSizeZ = preferenceHandler.mapSizeZPref;

        int peakHeight = preferenceHandler.peakHeightPref;
        int peakHeightRange = preferenceHandler.peakHeightRangePref;
        int peakAmount = preferenceHandler.peakAmountPref;

        grid.CreateGrid(mapSizeX, mapSizeZ, peakHeight, peakHeightRange, peakAmount);
    }


}
