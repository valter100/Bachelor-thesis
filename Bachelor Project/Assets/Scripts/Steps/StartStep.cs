using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStep : Step
{
    void Start()
    {
        base.Start();
        StartCoroutine(ActivateClappy(0.4f));
    }

    IEnumerator ActivateClappy(float delay)
    {
        yield return new WaitForSeconds(delay);

        //Activate clappy and animate him to appear

        GiveTip();
    }

    public override void GiveTip()
    {
        base.GiveTip();
    }

    public override void DoAction(int actionIndex)
    {
        if (actionIndex == 0)
        {

        }
        if (actionIndex == 1)
        {

        }
        if (actionIndex == 2)
        {
            //clappy.SetINactive();
        }
    }
}
