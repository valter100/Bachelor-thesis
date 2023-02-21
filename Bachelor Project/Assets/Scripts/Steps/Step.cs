using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Step : MonoBehaviour
{
    [SerializeField] protected AI clappy;
    [SerializeField] protected int index;
    [SerializeField] protected string question;
    [SerializeField] protected Grid grid;
    protected TextHandler textHandler;

    protected virtual void Start()
    {
        clappy = FindObjectOfType<AI>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        textHandler = clappy.gameObject.GetComponent<TextHandler>();
        SetQuestion();
    }

    public virtual void DoAction()
    {
        clappy.GiveTip(this);
    }
    public int Index() => index;
    public string Question() => question;

    private void SetQuestion()
    {
        question = textHandler.GetQuestion(index);
    }
}
