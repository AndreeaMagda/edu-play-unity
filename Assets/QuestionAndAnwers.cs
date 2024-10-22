using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Question
{
    public string Intrebare;
    public string A;
    public string B;
    public string C;
    public string D;
    public string RaspunsCorect;
    public string[] answers;
    public int correctAnswer;
}

public class QuestionAndAnswers: MonoBehaviour
{
    public string question;
    public string[] answers;
    public int correctAnswer;

    public List<Question> questions = new List<Question>();

    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "intrebari.csv");
        ReadCSV(filePath);
    }

    void ReadCSV(string filePath)
    {
        StreamReader reader = new StreamReader(filePath);
        bool firstLine = true;

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            if (firstLine) // Skip the first line (headers)
            {
                firstLine = false;
                continue;
            }

            string[] values = line.Split(',');
            Question newQuestion = new Question
            {
                Intrebare = values[0],
                A = values[1],
                B = values[2],
                C = values[3],
                D = values[4],
                RaspunsCorect = values[5],
                answers = new string[] { values[1], values[2], values[3], values[4] },
                correctAnswer = System.Array.IndexOf(new string[] { values[1], values[2], values[3], values[4] }, values[5])
            };
            questions.Add(newQuestion);
            question = newQuestion.Intrebare;
        }
        reader.Close();
    }
}
