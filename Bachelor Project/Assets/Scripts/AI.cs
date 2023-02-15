using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    StreamReader sr;
    StreamWriter sw;

    TextAsset memory;
    string path;
    Grid grid;


    private void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        path = "Assets/TextFiles/memory.txt";
    }

    public enum ChatOption
    {
        one, two, three
    }

    public void GiveTip(int tipIndex)
    {
        Toggle(true);
        tipIndex = 0;

        if (tipIndex == 0)
        {
            tipMessage.text = "Hey there partner! It's time for us to start constructing our first grid! What size do you want the grid to be?";
        }
        else
        {
            tipMessage.text = "I noticed you created your grid! Are you happy with how smooth the terrain is? Or do you want it smoother or rougher?";
        }
    }

    public void ChooseOption(int index)
    {
        chatOption = (ChatOption)index;

        sw = new StreamWriter(path);

        if (index == 0)
        {
            grid.SetGridDimension(32, 32, 32);
            sw.Write("GridDimensions: 32, 32, 32");
            
        }
        else if (index == 1)
        {
            grid.SetGridDimension(64, 64, 64);
            sw.Write("GridDimensions: 64, 64, 64");
        }
        else
        {
            grid.SetGridDimension(96, 96, 96);
            sw.Write("GridDimensions: 96, 96, 96");
        }

        sw.Close();
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
