using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;  // TMP pentru întrebare
    public Button[] answerButtons;          // Butoanele pentru variantele de răspuns

    private List<Question> questions = new List<Question>();  // Lista de întrebări
    private Question currentQuestion;

    void Start()
    {
        LoadQuestionsFromCSV("intrebari_romana"); // Asigură-te că numele fișierului este corect
        ShowNextQuestion();
    }

    // Structură pentru o întrebare
    public class Question
    {
        public string question;
        public string[] answers;
        public int correctAnswer;
    }

    // Funcție pentru încărcarea întrebărilor din CSV
    void LoadQuestionsFromCSV(string fileName)
    {
        // Încărcați fișierul CSV din Resources
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError("Fișierul CSV nu a fost găsit: " + fileName);
            return;
        }

        Debug.Log("Conținut CSV: " + csvFile.text);

        // Sparge conținutul pe linii
        string[] lines = csvFile.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] values = line.Split(',');
            if (values.Length < 6) continue;  // Saltă rândurile incomplete

            Question q = new Question();
            q.question = values[0];
            q.answers = new string[] { values[1], values[2], values[3], values[4] };

            // Convertim literele A, B, C, D la valori numerice
            string correctAnswerLetter = values[5].Trim().ToUpper();  // Asigură-te că este uppercase și fără spații
            switch (correctAnswerLetter)
            {
                case "A":
                    q.correctAnswer = 0;
                    break;
                case "B":
                    q.correctAnswer = 1;
                    break;
                case "C":
                    q.correctAnswer = 2;
                    break;
                case "D":
                    q.correctAnswer = 3;
                    break;
                default:
                    Debug.Log("Răspuns invalid: " + correctAnswerLetter);
                    continue;  // Sari peste această întrebare dacă răspunsul este invalid
            }

            questions.Add(q);
        }

        Debug.Log("Întrebări încărcate: " + questions.Count);
    }


    // Funcție pentru afișarea unei întrebări noi
    void ShowNextQuestion()
    {
        if (questions.Count > 0)
        {
            // Alege un index aleatoriu
            int randomIndex = UnityEngine.Random.Range(0, questions.Count);
            currentQuestion = questions[randomIndex];  // Ia întreba aleatoare

            // Debug pentru a vedea dacă întrebarea este setată corect
            Debug.Log("Întrebare afișată: " + currentQuestion.question);
            questionText.text = currentQuestion.question;  // Setează întrebarea în UI

            // Verifică răspunsurile și asigură-te că sunt afișate corect
            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];
                Debug.Log("Răspuns buton " + i + ": " + currentQuestion.answers[i]);

                // Atribuie click listener pentru fiecare buton
                int answerIndex = i; // Capturare corectă a indexului
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => CheckAnswer(answerIndex));
            }

            // Scoate întrebarea curentă din listă pentru a evita repetarea
            questions.RemoveAt(randomIndex);
        }
        else
        {
            Debug.Log("Nu există întrebări de afișat.");
        }
    }



    // Funcție pentru verificarea răspunsului
    void CheckAnswer(int index)
    {
        if (index == currentQuestion.correctAnswer)
        {
            Debug.Log("Răspuns corect!");
        }
        else
        {
            Debug.Log("Răspuns greșit!");
        }

        // Afișează întrebarea următoare
        ShowNextQuestion();
    }

    public void OnAnswerButtonClicked(int answerIndex)
    {
        CheckAnswer(answerIndex);
    }

}
