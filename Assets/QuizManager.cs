using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public QuestionAndAnswers QnA;
    public GameObject[] options;
    public int currentQuestion;

    public TMP_Text QuestionText;

    public void Start()
    {
        GenerateQuestions();
    }

    public void Correct()
    {

        QnA.questions.RemoveAt(currentQuestion);
        GenerateQuestions();
    }

    void SetAnswers()
    {
        for (int i = 0; i< options.Length; i++)
        {
            options[i].GetComponent<Answers>().isCorrect = false; // Corrected from Answer to Answers

            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA.questions[currentQuestion].answers[i];

            if (QnA.questions[currentQuestion].correctAnswer == i)
            {
                options[i].GetComponent<Answers>().isCorrect = true;
            }

        }

    }
    void GenerateQuestions()
    {
        if (QnA.questions.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.questions.Count);
            QuestionText.text = QnA.questions[currentQuestion].Intrebare;
            SetAnswers();
        }
        else
        {
            Debug.Log("No more questions available.");
        }
    }
}
