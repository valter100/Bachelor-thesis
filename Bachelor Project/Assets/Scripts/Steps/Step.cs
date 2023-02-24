using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Step : MonoBehaviour
{
    [SerializeField] protected AI clappy;
    [SerializeField] protected int index;
    [SerializeField] protected string question;
    [SerializeField] protected Grid grid;
    [SerializeField] protected bool giveAdvice;

    [SerializeField] protected List<GameObject> UIElements;

    protected virtual void Start()
    {
        clappy = FindObjectOfType<AI>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();   
    }

    public virtual void DoAction()
    {
        clappy.GiveTip(this);
    }
    public int Index() => index;
    public string Question() => question;

    public void SetUIActive(bool state)
    {
        foreach(GameObject UIElement in UIElements)
        {
            UIElement.SetActive(state);
        }
    }
}
