using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;  // TMP for question
    public Button[] answerButtons;        // Buttons for answer options
    public TextMeshProUGUI feedbackText;

    private List<Question> questions = new List<Question>();  // List of questions
    private Question currentQuestion;
    private int questionsToAnswer = 1;    // Number of questions to answer based on challenge

    private bool quizComplete = false;
    private bool lastAnswerCorrect = false;


    void Start()
    {
        LoadQuestionsFromCSV("intrebari_romana");  
        ShowNextQuestion();
        if (feedbackText == null)
        {
            Debug.LogError("FeedbackText nu este setat în Inspector!");
        }
    }

    public class Question
    {
        public string question;
        public string[] answers;
        public int correctAnswer;
    }

    void LoadQuestionsFromCSV(string fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);
        if (csvFile == null)
        {
            Debug.LogError("CSV file not found: " + fileName);
            return;
        }

        Debug.Log("CSV content: " + csvFile.text);
        string[] lines = csvFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] values = line.Split(',');
            if (values.Length < 6) continue;

            Question q = new Question();
            q.question = values[0];
            q.answers = new string[] { values[1], values[2], values[3], values[4] };

            string correctAnswerLetter = values[5].Trim().ToUpper();
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

        Debug.Log("Questions loaded: " + questions.Count);
    }



    public void CheckAnswer(bool isCorrect)
    {
        lastAnswerCorrect = isCorrect;
        quizComplete = true;

        if (isCorrect)
        {
            feedbackText.text = "Corect!";
            feedbackText.color = Color.green;
            Debug.Log("Feedback text setat la 'Corect!'");
        }
        else
        {
            feedbackText.text = "Greșit!";
            feedbackText.color = Color.red;
            Debug.Log("Feedback text setat la 'Greșit!'");
        }

     
        StartCoroutine(ClearFeedbackAfterDelay(5.0f));
    }
    public bool IsQuizComplete()
    {
        return quizComplete;
    }

    public bool WasAnswerCorrect()
    {
        return lastAnswerCorrect;
    }


    private IEnumerator ClearFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        feedbackText.text = "";  // Golește textul feedback-ului
    }

    void ShowNextQuestion()
    {
        quizComplete = false;  // Resetăm starea la fiecare întrebare nouă

        if (questions.Count > 0)
        {
            int randomIndex = Random.Range(0, questions.Count);
            currentQuestion = questions[randomIndex];
            questions.RemoveAt(randomIndex);

            questionText.text = currentQuestion.question;

            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.answers[i];
                answerButtons[i].onClick.RemoveAllListeners();
                int answerIndex = i;
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(answerIndex));
            }
        }
        else
        {
            Debug.Log("No more questions available.");
            EndQuiz();
        }
    }


    void OnAnswerSelected(int answerIndex)
    {
        bool isCorrect = (answerIndex == currentQuestion.correctAnswer);
        CheckAnswer(isCorrect);
        if (answerIndex == currentQuestion.correctAnswer)
        {
            feedbackText.text = "Corect!";
            feedbackText.color = Color.green;
            Debug.Log("Correct answer!");
            
        }
        else
        {
            Debug.Log("Wrong answer.");
            feedbackText.text = "Greșit!";
            feedbackText.color = Color.red;
           
        }
      //  StartCoroutine(ClearFeedbackAfterDelay(5.0f));
        ShowNextQuestion();

        questionsToAnswer--;
        if (questionsToAnswer > 0)
        {
            ShowNextQuestion();
        }
        else
        {
            EndQuiz();
        }
    }

    // În DialogueScene.cs
    public void OnDialogueComplete()
    {
        // Cod pentru a închide DialogueScene
        SceneManager.UnloadSceneAsync("DialogueScene");
        
        // Apelăm funcția din PlayerController pentru a continua de la poziția curentă
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.ContinueFromCurrentPosition();
        }
        else
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
    }


    void EndQuiz()
    {
        Debug.Log("Challenge completed. Returning to game.");

        // Unload dialogue scene and return to main scene
        SceneManager.UnloadSceneAsync("DialogueScene");  // Unload the dialogue scene
        // Load the main game scene if necessary
        SceneManager.LoadScene("GameScene");  // Load your game scene
    }
}
