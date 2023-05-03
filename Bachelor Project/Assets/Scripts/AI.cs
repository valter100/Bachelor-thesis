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
    [SerializeField] public List<Step> steps = new List<Step>();
    [SerializeField] Step activeStep;
    [SerializeField] TMP_Text tipMessage;
    [SerializeField] Image image;
    ChatOption chatOption;

    private bool inactive;
    GameObject mascotText;
    GameObject mascot;

    [SerializeField] int answerAmount;
    [SerializeField] Button[] buttons;

    TextHandler textHandler;
    Grid grid;


    private void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        textHandler = FindObjectOfType<TextHandler>();
        mascotText = GameObject.FindGameObjectWithTag("mascotText");
        mascot = GameObject.FindGameObjectWithTag("mascot");
        Toggle(true);
        inactive = false;
    }

    public enum ChatOption
    {
        one, two, three
    }

    public void GiveTip(Step currentStep)
    {
        if (inactive) return;

        tipMessage.text = currentStep.Question();
        activeStep = currentStep;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TMP_Text>().text = currentStep.Option(i);
        }
    }

    public void ChooseOption(int index)
    {
        chatOption = (ChatOption)index;

        textHandler.SaveAnswers(activeStep.Index(), index, activeStep.Option(index));

        activeStep.DoAction(index);

        //Toggle(false);
    }

    public void Toggle(bool state)
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
        Toggle(false);
        mascotText.SetActive(false);
        mascot.SetActive(false);
    }

    public void SetStep(int index)
    {
        activeStep = steps[index];
        activeStep.GiveTip();
    }

    public void setNextStep()
    {
        activeStep = steps[activeStep.Index() + 1];
    }

    public Step GetStep() => activeStep;
}
