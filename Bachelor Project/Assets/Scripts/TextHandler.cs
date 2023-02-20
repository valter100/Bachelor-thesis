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
}
