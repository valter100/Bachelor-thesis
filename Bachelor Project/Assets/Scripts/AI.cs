using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    [SerializeField] TMP_Text tipMessage;
    [SerializeField] Image image;
    ChatOption chatOption;

    [SerializeField] int answerAmount;
    [SerializeField] Button[] buttons;

    private void Start()
    {
        Toggle(false);
    }

    public enum ChatOption
    {
        one, two, three
    }

    public void GiveTip()
    {
        Toggle(true);
        tipMessage.text = "Hey! I noticed you created your grid! Are you happy with how smooth the terrain is? Or do you want it smoother or rougher?";

    }

    public void ChooseOption(int index)
    {
        chatOption = (ChatOption)index;

        //Do button effect

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
