using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Step : MonoBehaviour
{
    [SerializeField] protected AI clappy;
    [SerializeField] protected int index;
    [SerializeField] protected string question;
    protected List<string> options = new List<string>();
    [SerializeField] protected Grid grid;
    [SerializeField] protected bool giveAdvice;
    TextHandler textHandler;
    [SerializeField] protected List<GameObject> UIElements;

    protected virtual void Start()
    {
        clappy = FindObjectOfType<AI>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        textHandler = clappy.gameObject.GetComponent<TextHandler>();
        SetQuestion();
        SetOptions();
    }

    public virtual void GiveTip()
    {
        clappy.GiveTip(this);
    }

    public virtual void DoAction()
    {
        clappy.GiveTip(this);
    }

    public int Index() => index;
    public string Question() => question;
    public string Option(int index) => options[index];

    public void SetUIActive(bool state)
    {
        foreach(GameObject UIElement in UIElements)
        {
            UIElement.SetActive(state);
        }
    }

    private void SetQuestion()
    {
        question = textHandler.GetQuestion(index);
    }

    private void SetOptions()
    {
        options = textHandler.GetOptions(index);
    }
}
