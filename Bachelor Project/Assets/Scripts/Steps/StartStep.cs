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

        DoAction();
    }

    public override void DoAction()
    {
        base.DoAction();
    }
}
