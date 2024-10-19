using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SprintsManager : MonoBehaviour
{
    public GameObject[] players; // Array pentru cei 4 jucători
    public TextMeshProUGUI currentPlayerText; // Textul care va afișa cine este la rând
    private int currentPlayerIndex = 0; // Indexul jucătorului curent
    private Vector2[] path; // Lista de coordonate a traseului

    private void Start()
    {
        // Inițializare traseu
        path = new Vector2[]
        {
            new Vector2(15, 8), new Vector2(15, 7), new Vector2(15, 6), new Vector2(15, 5),
            new Vector2(15, 4), new Vector2(15, 3), new Vector2(15, 2), new Vector2(15, 1), new Vector2(15, 0),
            new Vector2(14, 0), new Vector2(13, 0), new Vector2(12, 0), new Vector2(11, 0), new Vector2(10, 0),
            new Vector2(9, 0), new Vector2(8, 0), new Vector2(7, 0), new Vector2(6, 0), new Vector2(5, 0),
            new Vector2(4, 0), new Vector2(3, 0), new Vector2(2, 0), new Vector2(1, 0), new Vector2(0, 0),
            new Vector2(0, 1), new Vector2(0, 2), new Vector2(0, 3), new Vector2(0, 4), new Vector2(0, 5),
            new Vector2(0, 6), new Vector2(0, 7), new Vector2(0, 8), new Vector2(1, 8), new Vector2(2, 8),
            new Vector2(3, 8), new Vector2(4, 8), new Vector2(5, 8), new Vector2(6, 8), new Vector2(7, 8),
            new Vector2(8, 8), new Vector2(9, 8), new Vector2(10, 8), new Vector2(11, 8), new Vector2(12, 8),
            new Vector2(13, 8), new Vector2(14, 8)
        };

        // Afișăm jucătorul curent
        UpdateCurrentPlayerText();
    }

    // Funcție pentru a arunca zarul
    public void RollDice() // Aici este funcția pe care o așteptai!
    {
        int diceResult = RollDiceValue();
        Debug.Log("Rezultatul zarului: " + diceResult);
        StartCoroutine(MovePawn(diceResult));
    }

    // Functia pentru a gestiona apasarea butonului Roll
    public void OnRollButtonClicked()
    {
        RollDice(); // Apelează funcția pentru a arunca zarul
    }

    // Funcție pentru a genera rezultatul zarului (între 1 și 6)
    private int RollDiceValue()
    {
        return Random.Range(1, 7);
    }

    // Funcție pentru a muta pionul pe tabla de joc
    private IEnumerator MovePawn(int diceResult)
    {
        GameObject currentPlayer = players[currentPlayerIndex]; // Jucătorul curent
        Vector2 startPosition = currentPlayer.transform.position; // Poziția de start
        int currentTileIndex = Mathf.RoundToInt(startPosition.x); // Indexul poziției curente

        for (int i = 0; i < diceResult; i++)
        {
            currentTileIndex++; // Avansează la următoarea poziție

            // Verificăm dacă depășim limita traseului
            if (currentTileIndex >= path.Length)
            {
                Debug.Log("Ai ajuns la finalul traseului.");
                currentTileIndex = path.Length - 1; // Sau resetezi la 0 dacă vrei să reînceapă traseul
                break;
            }

            Vector2 endPosition = path[currentTileIndex]; // Poziția finală
            float journeyLength = Vector2.Distance(startPosition, endPosition); // Distanța totală
            float startTime = Time.time; // Timpul de start

            while (Vector2.Distance(currentPlayer.transform.position, endPosition) > 0.01f)
            {
                // Calculăm cât timp a trecut
                float distCovered = (Time.time - startTime) * 3f; // Viteza de mișcare
                float fractionOfJourney = distCovered / journeyLength; // Proporția de parcurs

                // Interpolăm între cele două poziții
                currentPlayer.transform.position = Vector2.Lerp(startPosition, endPosition, fractionOfJourney);
                yield return null; // Așteptăm următorul frame
            }

            // Asigură-te că pionul ajunge exact la poziția finală
            currentPlayer.transform.position = endPosition;
            startPosition = endPosition; // Actualizăm poziția de start pentru următoarea iterație
        }

        // Trecem la următorul jucător
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length; // Rotim indexul
        UpdateCurrentPlayerText(); // Actualizăm textul cu jucătorul curent
    }

    // Funcție pentru a actualiza textul jucătorului curent
    private void UpdateCurrentPlayerText()
    {
        string playerName = "Veverita"; // Default
        switch (currentPlayerIndex)
        {
            case 0: playerName = "Veverita"; break;
            case 1: playerName = "Bufnita"; break;
            case 2: playerName = "Leu"; break;
            case 3: playerName = "Pisica"; break;
        }
        currentPlayerText.text = "Randul Echipei: " + playerName; // Actualizăm textul
    }
}
