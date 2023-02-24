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

                //if(line.IsNullOrWhiteSpace()) continue;

                if (!startFound) startFound = line.Contains(startSearchString);

                if (startFound)
                {
                    bool isEnd = line.Contains(endSearchString);

                    if (!isEnd) lines.Add(line);

                    if(isEnd) break;
                }
               
            }
        }

        sr.Close(); 

        return question;
    }
}
