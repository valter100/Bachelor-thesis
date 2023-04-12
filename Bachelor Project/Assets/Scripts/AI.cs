using System.Collections;
using System.Collections.Generic;
using System.Data;
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


        if (index == 0)
        {
           
        }
        else if (index == 1)
        {
           
        }
        else
        {
            
        }

        textHandler.SaveAnswers(activeStep.Index(), index, activeStep.Option(index));

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
}
