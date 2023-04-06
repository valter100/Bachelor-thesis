using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class TextHandler : MonoBehaviour
{
    //TextAsset questions, answers;
    string aPath, pPath;


    StreamReader sr;
    StreamWriter sw;

    List<Vector2> userData = new List<Vector2>();
    Dictionary<int, List<int>> userDataMatrix = new Dictionary<int, List<int>>();

    void Awake()
    {
        aPath = "Assets/TextFiles/answers.txt";
        pPath = "Assets/TextFiles/preferences.txt";
        ReadUserData();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public int GetPreferenceAmount(string pref)
    {
        List<string> lines = new List<string>();

        using (sr = new StreamReader(pPath))
        {
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();

                if (IsNullOrWhiteSpace(line)) continue;

                if (line.Contains(pref)) lines.Add(line);
            }
        }
        sr.Close();

        return lines.Count;
    }

    public int GetPreferenceAmount(string pref, string secondPref)
    {
        List<string> lines = new List<string>();

        using (sr = new StreamReader(pPath))
        {
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();

                if (IsNullOrWhiteSpace(line)) continue;

                if (line.Contains(pref) || line.Contains(secondPref)) lines.Add(line);
            }
        }
        sr.Close();

        return lines.Count;
    }

    public int GetPreferences(string pref)
    {
        int sum = 0;
        List<string> lines = new List<string>();
        List<string> results = new List<string>();

        using (sr = new StreamReader(pPath))
        {
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();

                if (IsNullOrWhiteSpace(line)) continue;

                if (line.Contains(pref)) lines.Add(line);
            }
        }
        sr.Close();


        for (int i = 0; i < lines.Count; i++)
        {
            string line = lines[i];
            string result = "";

            foreach (char c in line)
            {
                if (pref.Contains(c)) continue;

                result += c;
            }
            results.Add(result);
        }

        for (int i = 0; i < results.Count; i++)
        {
            try
            {
                sum += int.Parse(results[i]);
            }
            catch 
            {
                Debug.Log(results[i]);
            }
        }

        return sum;
    }

    public void SavePreferenses(string pref)
    {
        using (FileStream fs = new FileStream(pPath, FileMode.Append, FileAccess.Write))
        using (sw = new StreamWriter(fs)) sw.WriteLine(pref);
        sw.Close();
    }

    public void SaveAnswers(int index, int answerIndex, string answer)
    {
        using (FileStream fs = new FileStream(aPath, FileMode.Append, FileAccess.Write))
        using (sw = new StreamWriter(fs)) sw.WriteLine("questionIndex:" + index + "/answerIndex:" + answerIndex + "/answer:" + answer);
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
            string line = lines[i];
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
        foreach (Vector2 vec in userData)
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

    public List<int> GetAnswerData(int index)
    {
        if (!userDataMatrix.ContainsKey(index)) return null;

        return userDataMatrix[index];
    }
}
