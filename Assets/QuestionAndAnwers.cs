using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class QuestionAndAnswers : MonoBehaviour
{
    public string question; // Întrebarea curentă
    public string[] answers; // Răspunsurile curente
    public int correctAnswer; // Indexul răspunsului corect

    public List<Question> questions = new List<Question>(); // Lista de întrebări

    void Start()
    {
        // Definim calea fișierului CSV din folderul StreamingAssets
        string filePath = Path.Combine(Application.streamingAssetsPath, "intrebari_romana.csv");

        // Verificăm dacă fișierul există
        if (File.Exists(filePath))
        {
            ReadCSV(filePath);
        }
        else
        {
            Debug.LogError("Fișierul CSV nu a fost găsit la calea: " + filePath);
        }
    }

    // Funcția pentru citirea datelor din fișierul CSV
    void ReadCSV(string filePath)
    {
        StreamReader reader = new StreamReader(filePath);
        bool firstLine = true; // Flag pentru a sări peste prima linie (antet)

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            if (firstLine) // Sărim peste prima linie cu antet
            {
                firstLine = false;
                continue;
            }

            // Împărțim linia în valori individuale
            string[] values = line.Split(',');

            // Log pentru a vedea câte coloane sunt detectate
            Debug.Log("Număr de coloane citite: " + values.Length + " | Linie: " + line);

            // Verificăm dacă linia are exact 6 coloane
            if (values.Length != 6)
            {
                Debug.LogError("Linia din CSV nu are 6 coloane: " + line);
                continue; // Sărim peste această linie
            }

            // Creăm o nouă întrebare și o adăugăm la listă
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

            // Adăugăm întrebarea în lista de întrebări
            questions.Add(newQuestion);

            // Actualizăm întrebarea curentă
            question = newQuestion.Intrebare;
        }

        reader.Close(); // Închidem cititorul de fișiere
    }
} 

// Clasa simplă pentru întrebări, fără să fie derivată din MonoBehaviour
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
