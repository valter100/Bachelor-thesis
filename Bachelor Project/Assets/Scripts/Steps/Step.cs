using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    [SerializeField] protected AI clappy;
    [SerializeField] protected int index;
    protected List<string> options = new List<string>();
    [SerializeField] protected Grid grid;
    [SerializeField] protected bool giveAdvice;
    protected TextHandler textHandler;
    protected PreferenceHandler preferenceHandler;
    [SerializeField] protected List<GameObject> UIElements;
    protected List<int> userAnswers = new List<int>();


    public delegate void StepStart();
    public static event StepStart OnStepStart;
	
    protected string question, optOne, optTwo, optThree;


    protected virtual void Start()
    {
        clappy = FindObjectOfType<AI>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        textHandler = FindObjectOfType<TextHandler>();
        preferenceHandler = clappy.gameObject.GetComponent<PreferenceHandler>();
        SetText();
        GetUserAnswers();
    }

    public virtual void GiveTip()
    {
        if (clappy.inactive) return;

        clappy.GiveTip(this);
    }

    public virtual void DoAction(int actionIndex)
    {
       
    }

    public virtual void StartStep()
    {
        try
        {
            OnStepStart();
        }
        catch
        {

        }
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

    protected virtual void SetText()
    {
        options.Add(optOne);
        options.Add(optTwo);
        options.Add(optThree);
    }

    private void GetUserAnswers()
    {
        userAnswers = textHandler.GetAnswerData(index);
    }
}
