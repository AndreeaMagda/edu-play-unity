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

    }

    void GenerateQuestions()
    {
        currentQuestion = Random.Range(0, QnA.questions.Count);

        QuestionText.text = QnA.questions[currentQuestion].Intrebare;
    }
}
