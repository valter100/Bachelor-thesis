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

                    if (!isEnd && !line.Contains("index" + index.ToString())) lines.Add(line);

                    if(isEnd) break;
                }
            }
        }
        sr.Close();

        question = lines[0];

        return question;
    }

    public List<string> GetOptions(int index)
    {
        List<string> options = new List<string>();
        List<string> lines = new List<string>();

        using (sr = new StreamReader(qPath))
        {
            bool startFound = false;

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();

                if (IsNullOrWhiteSpace(line)) continue;

                if (!startFound) startFound = line.Contains("index" + index.ToString());

                if (startFound)
                {
                    bool isEnd = line.Contains("index" + (index + 1).ToString());

                    if (!isEnd && !line.Contains("index" + index.ToString())) lines.Add(line);

                    if (isEnd) break;
                }
            }
        }
        sr.Close();

        for (int i = 1; i < lines.Count; i++)
        {
            options.Add(lines[i]);
        }

        return options;
    }

    public void SaveAnswers(int index, int answerIndex, string answer)
    {
        using (FileStream fs = new FileStream(aPath, FileMode.Append, FileAccess.Write))
        using (sw = new StreamWriter(fs))

        sw.WriteLine("question:" + index + "/ answerIndex:" + answerIndex + "/ answer:" + answer);

        sw.Close();
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
