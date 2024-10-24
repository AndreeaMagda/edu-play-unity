using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject[] pawns; // Lista cu pionii jucătorilor
    public Image diceImage;
    public Sprite[] diceSprites;
    private int currentPlayerIndex = 0; // Jucătorul curent
    private int currentTileIndex = 0;
    private List<Vector2> path;
    private bool gameStarted = false;

    private int[] currentTileIndices;

    // Referință către SpecialTileManager
    private SpecialTileManager specialTileManager;

    void Start()
    {
        // Initializează array-ul de poziții pentru fiecare jucător
        currentTileIndices = new int[pawns.Length];

        // Încarcă pozițiile pionilor din PlayerPrefs
        for (int i = 0; i < pawns.Length; i++)
        {
            currentTileIndices[i] = PlayerPrefs.GetInt("CurrentTileIndex_Player" + i, 0);


            switch (i)
            {
                case 1:
                    currentTileIndices[i] = PlayerPrefs.GetInt("CurrentTileIndex_Player1");
                    break;
                case 2:
                    currentTileIndices[i] = PlayerPrefs.GetInt("CurrentTileIndex_Player2");
                    break;
                case 3:
                    currentTileIndices[i] = PlayerPrefs.GetInt("CurrentTileIndex_Player3");
                    break;
                case 4:
                    currentTileIndices[i] = PlayerPrefs.GetInt("CurrentTileIndex_Player4");
                    break;
                default:
                    break;
            }
        }


        PathGenerator pathGenerator = GetComponent<PathGenerator>();
        if (pathGenerator != null)
        {
            path = pathGenerator.GeneratePath();
        }
        else
        {
            Debug.LogError("PathGenerator component is missing.");
            return;
        }

        gameStarted = PlayerPrefs.GetInt("GameStarted") == 1;
        currentTileIndex = PlayerPrefs.GetInt("CurrentTileIndex");
        specialTileManager = GetComponent<SpecialTileManager>();
        if (specialTileManager == null)
        {
            Debug.LogError("SpecialTileManager component is missing.");
            return;
        }

        ContinueFromCurrentPosition(); // Mută pionul la poziția de start
    }

    void Update()
    {
        PlayerPrefs.SetInt("GameStarted", gameStarted ? 1 : 0);
        for (int i = 0; i < pawns.Length; i++)
        {
            switch (i)
            { case 1:
                    PlayerPrefs.SetInt("CurrentTileIndex_Player1", currentTileIndices[1]);
                    break;
                case 2:
                    PlayerPrefs.SetInt("CurrentTileIndex_Player2", currentTileIndices[2]);
                    break;
                case 3:
                    PlayerPrefs.SetInt("CurrentTileIndex_Player3", currentTileIndices[3]);
                    break;
                case 4:
                    PlayerPrefs.SetInt("CurrentTileIndex_Player4", currentTileIndices[4]);
                    break;
                default:
                    break;
            }
            PlayerPrefs.SetInt("CurrentTileIndex_Player" + i, currentTileIndices[i]);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int diceResult = RollDice();
            Debug.Log("Dice result: " + diceResult);
            StartCoroutine(MovePawn(diceResult));
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }

    IEnumerator MovePawn(int diceResult)
    {
        GameObject currentPawn = pawns[currentPlayerIndex]; // Pionul jucătorului curent
        int currentPawnTileIndex = currentTileIndices[currentPlayerIndex]; // Poziția pionului curent

        for (int i = 0; i < diceResult; i++)
        {
            currentPawnTileIndex++; // Mut pionul la următoarea poziție

            if (currentPawnTileIndex >= path.Count) // Verific dacă pionul a ajuns la finalul traseului
            {
                Debug.Log("Ai ajuns la finalul traseului.");
                currentPawnTileIndex = path.Count - 1;
                break;
            }

            Vector2 startPosition = currentPawn.transform.position;
            Vector2 endPosition = path[currentPawnTileIndex];

            float journeyLength = Vector2.Distance(startPosition, endPosition);
            float startTime = Time.time;

            while (Vector2.Distance(currentPawn.transform.position, endPosition) > 0.01f)
            {
                float distCovered = (Time.time - startTime) * 3f;
                float fractionOfJourney = distCovered / journeyLength;

                currentPawn.transform.position = Vector2.Lerp(startPosition, endPosition, fractionOfJourney);
                yield return null;
            }

            // Actualizează poziția pionului curent în array
            MovePawnToPosition(endPosition);
        }

        currentTileIndices[currentPlayerIndex] = currentPawnTileIndex; // Actualizează array-ul cu poziția pionului curent

        // Verificăm dacă pionul a aterizat pe un tile special
        StartCoroutine(specialTileManager.CheckForSpecialTile(path[currentTileIndices[currentPlayerIndex]]));

        // După mutare, trecem la următorul jucător
        ChangeTurn();
    }


    public int RollDice()
    {
        int diceResult = Random.Range(1, 7);

        diceImage.sprite = diceSprites[diceResult - 1]; // Actualizează imaginea zarului

        return diceResult;
    }

    void MovePawnToPosition(Vector2 newPosition)
    {
        pawns[currentPlayerIndex].transform.position = newPosition;
        Debug.Log("Pawn moved to: " + newPosition);
    }

    void ChangeTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % pawns.Length; // Treci la următorul jucător
        Debug.Log("Este rândul jucătorului " + (currentPlayerIndex + 1));
    }


    public void ContinueFromCurrentPosition()
    {
        MovePawnToPosition(path[currentTileIndices[currentPlayerIndex]]);
    }
    public void MovePawnForward(int steps)
    {
        currentTileIndices[currentPlayerIndex] += steps;
        if (currentTileIndices[currentPlayerIndex] >= path.Count)
            currentTileIndices[currentPlayerIndex] = path.Count - 1;

        MovePawnToPosition(path[currentTileIndices[currentPlayerIndex]]);
    }

    public void MovePawnBack(int steps) 
    {
        currentTileIndices[currentPlayerIndex] -= steps;
        if (currentTileIndices[currentPlayerIndex] < 0)
            currentTileIndices[currentPlayerIndex] = 0;

        MovePawnToPosition(path[currentTileIndices[currentPlayerIndex]]);
    }


    public IEnumerator HandleMultipleQuestions(int numQuestions)
    {
        for (int i = 0; i < numQuestions; i++)
        {
            Debug.Log("Displaying question " + (i + 1));
            SceneManager.LoadScene("DialogueScene");
            yield return new WaitForSeconds(2); // Aștepți până se închide scena
        }
    }

    public IEnumerator HandleMultipleQuestionsWithPenalty(int numQuestions)
    {
        for (int i = 0; i < numQuestions; i++)
        {
            Debug.Log("Displaying question " + (i + 1));
            SceneManager.LoadScene("DialogueScene");
            yield return new WaitForSeconds(2);

            // Aici poți implementa logica pentru a verifica dacă răspunsul a fost greșit și să sari o tură
        }
    }
}
