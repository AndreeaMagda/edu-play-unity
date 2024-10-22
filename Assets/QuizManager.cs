using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public QuestionAndAnswers QnA;
    public GameObject[] options;
    public int currentQuestion;

    public Text QuestionText;

    public void Start()
    {
        GenerateQuestions();
    }

    void SetAnswers()
    {
        for (int i = 0; i< options.Length; i++)
        {
            options[i].GetComponent<Answers>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].answers[i];

            if (QnA[currentQuestion].correctAnswer == i)
            {
                options[i].GetComponent<Answers>().isCorrect = true;
            }

        }

    }

    void GenerateQuestions()
    {
        currentQuestion = Random.Range(0, QnA.questions.Count);

        QuestionText.text = QnA.questions[currentQuestion].Intrebare;
        SetAnswers();
    }
}
