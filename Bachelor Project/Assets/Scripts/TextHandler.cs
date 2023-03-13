using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class TextHandler : MonoBehaviour
{
    TextAsset questions, answers;
    string aPath, qPath;


    StreamReader sr;
    StreamWriter sw;
    
    List<Vector2> userData = new List<Vector2>();
    Dictionary<int, List<int>> userDataMatrix = new Dictionary<int, List<int>>();

    void Awake()
    {
        aPath = "Assets/TextFiles/answers.txt";
        qPath = "Assets/TextFiles/questions.txt";
    }

    void Start()
    {
        ReadUserData();
        //DebugDictionary(); if we use a loop to go through values, make sure it wont skip. Can not simply use "count" function
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

        sw.WriteLine("questionIndex:" + index + "/answerIndex:" + answerIndex + "/answer:" + answer);

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

    public void ResetClappysMemory()
    {
        sw = new StreamWriter(aPath);
        sw.WriteLine();
        sw.Close();
    }

    private void ReadUserData()
    {
        List<string> lines = new List<string>();

        using (FileStream fs = new FileStream(aPath, FileMode.Open, FileAccess.Read))
        using (sr = new StreamReader(fs))
        {
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                lines.Add(line);
            }
        }
        sr.Close();

        for (int i = 0; i < lines.Count; i++)
        {
            string line;
            line = lines[i];
            List<float> values = new List<float>();

            if (IsNullOrWhiteSpace(line)) continue;

            Regex regex = new Regex(@"[0-9]+");
            var matches = regex.Matches(line);

            foreach (Match match in matches)
            {
                values.Add(float.Parse(match.Value));
            }
           
            Vector2 data = new Vector2(values[0], values[1]);

            userData.Add(data);
        }

        AddValuesToDictionary();
    }

    private void AddValuesToDictionary()
    {
        foreach(Vector2 vec in userData)
        {
            int x, y;
            x = (int)vec.x;
            y = (int)vec.y;

            if (userDataMatrix.ContainsKey(x))
            {
                userDataMatrix[x].Add(y);
                continue;
            }

            List<int> value = new List<int>();
            value.Add(y);

            userDataMatrix.Add(x, value);
        }
    }

    private void DebugDictionary()
    {
        for (int i = 0; i < userDataMatrix.Count; i++)
        {
            if (!userDataMatrix.ContainsKey(i)) continue;

            for (int j = 0; j < userDataMatrix[i].Count; j++)
            {
                Debug.Log("Key " + i + " / Value " + userDataMatrix[i][j]);
            }
        }
    }
}
