using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextHandler : MonoBehaviour
{
    TextAsset questions, answers;
    string aPath, qPath;

    StreamReader sr;
    StreamWriter sw;

    string startSearchString, endSearchString;
    

    void Awake()
    {
        aPath = "Assets/TextFiles/answers.txt";
        qPath = "Assets/TextFiles/questions.txt";
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public string GetQuestion(int index)
    {
        string question = "";
        List<string> lines = new List<string>();

        using (sr = new StreamReader(qPath))
        {
            bool startFound = false;

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();

                if(IsNullOrWhiteSpace(line)) continue;

                if (!startFound) startFound = line.Contains("index" + index.ToString());

                if (startFound)
                {
                    bool isEnd = line.Contains("index" + (index + 1).ToString());

                    if (!isEnd) lines.Add(line);

                    if(isEnd) break;
                }
            }
        }
        sr.Close();

        foreach (string s in lines)
        {
            question += s;
            Debug.Log(s);
        }

        return question;
    }

    private bool IsNullOrWhiteSpace(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            if (!line[i].Equals("") && !line[i].Equals(" ")) return false;
        }

        return true;
    }
}
