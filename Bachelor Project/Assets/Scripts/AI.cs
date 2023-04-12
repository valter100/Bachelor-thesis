using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    [SerializeField] List<Step> steps = new List<Step>();
    [SerializeField] Step activeStep;
    [SerializeField] TMP_Text tipMessage;
    [SerializeField] Image image;
    ChatOption chatOption;
    public bool inactive = false;

    [SerializeField] int answerAmount;
    [SerializeField] Button[] buttons;

    TextHandler textHandler;
    Grid grid;


    private void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        textHandler = gameObject.GetComponent<TextHandler>();
    }

    public enum ChatOption
    {
        one, two, three
    }

    public void GiveTip(Step currentStep)
    {
        tipMessage.text = currentStep.Question();
        activeStep = currentStep;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TMP_Text>().text = currentStep.Option(i);
        }

        Toggle(true);
    }

    public void ChooseOption(int index)
    {
        chatOption = (ChatOption)index;

        textHandler.SaveAnswers(activeStep.Index(), index, activeStep.Option(index));

        activeStep.DoAction(index);

        Toggle(false);
    }

    void Toggle(bool state)
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(state);
        }

        image.gameObject.SetActive(state);
    }

    public void SetInactive()
    {
        inactive = true;
        gameObject.SetActive(false);
    }
    
    public void SetStep(int index)
    {
        activeStep = steps[index];
    }

    public void setNextStep()
    {
        activeStep = steps[activeStep.Index() + 1];
    }
}
