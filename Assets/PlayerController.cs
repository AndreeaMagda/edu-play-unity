using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject playerInstance;
    public Image diceImage;
    public Sprite[] diceSprites;
    private int currentTileIndex = 0;
    private List<Vector2> path;
    private bool gameStarted=false;


    // Special tile lists
    public Vector2[] chanceTiles;  // "Sansa" challenges
    public Vector2[] trapTiles;    // "Capcane" challenges
    public Vector2 stoneTile;      // "Pietre" challenge
    public Vector2 swampTile;      // "Mlastina" challenge
    public Vector2 moonTile;       // "Moon" challenge
    public Vector2 ladderTile;     // "Scara" challenge
    public Vector2 mapTile;        // "Map" challenge
    public Vector2 wingsTile;      // "Wings" challenge

    void Start()
    {
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

        gameStarted = PlayerPrefs.GetInt("GameStarted", 0) == 1;
        currentTileIndex = PlayerPrefs.GetInt("CurrentTileIndex", 0);

        ContinueFromCurrentPosition(); // Mută pionul la poziția de start
    }

    void Update()
    {
        PlayerPrefs.SetInt("GameStarted", gameStarted ? 1 : 0);
        PlayerPrefs.SetInt("CurrentTileIndex", currentTileIndex);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int diceResult =RollDice();
            Debug.Log("Dice result: " + diceResult);
            StartCoroutine(MovePawn(diceResult));
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.DeleteAll();
    }

    IEnumerator MovePawn(int diceResult)
    {
        for (int i = 0; i < diceResult; i++)
        {
            currentTileIndex++;

            if (currentTileIndex >= path.Count)
            {
                Debug.Log("You've reached the end of the path.");
                currentTileIndex = path.Count - 1;
                break;
            }

            Vector2 startPosition = playerInstance.transform.position;
            Vector2 endPosition = path[currentTileIndex];

            float journeyLength = Vector2.Distance(startPosition, endPosition);
            float startTime = Time.time;

            while (Vector2.Distance(playerInstance.transform.position, endPosition) > 0.01f)
            {
                float distCovered = (Time.time - startTime) * 3f;
                float fractionOfJourney = distCovered / journeyLength;

                playerInstance.transform.position = Vector2.Lerp(startPosition, endPosition, fractionOfJourney);
                yield return null;
            }

            MovePawnToPosition(endPosition);
        }

        CheckForSpecialTile(path[currentTileIndex]); // Verifică dacă se află pe un tile special
    }
    int RollDice()
    {
        int diceResult = Random.Range(1, 7);

        diceImage.sprite = diceSprites[diceResult - 1]; // Actualizează imaginea zarului

        return diceResult;
    }

    void MovePawnToPosition(Vector2 newPosition)
    {
        playerInstance.transform.position = newPosition;

        Debug.Log("Pawn moved to: " + newPosition);
    }


    private IEnumerator HandleMultipleQuestions(int numberOfQuestions)
    {
        for (int i = 0; i < numberOfQuestions; i++)
        {
            Debug.Log("Loading DialogueScene for question " + (i + 1) + "/" + numberOfQuestions);
            SceneManager.LoadScene("DialogueScene");  // Încarcă scena pentru întrebare

            // Așteaptă până când quiz-ul se termină (implementat în `QuizManager`)
            yield return new WaitUntil(() => FindObjectOfType<QuizManager>().IsQuizComplete());

            // Verifică dacă răspunsul a fost corect sau greșit
            if (FindObjectOfType<QuizManager>().WasAnswerCorrect())
            {
                Debug.Log("Răspuns corect la întrebarea " + (i + 1));
            }
            else
            {
                Debug.Log("Răspuns greșit la întrebarea " + (i + 1) + ". Penalitate aplicată.");
                // Aici poți adăuga penalitățile (de exemplu, sări o tură)
                break;  // Ieși din buclă dacă răspunsul este greșit, dacă este cazul
            }

            // Adaugă un mic delay între întrebări pentru a lăsa feedback-ul să fie vizibil
            yield return new WaitForSeconds(2.0f);  // Așteaptă 2 secunde înainte de următoarea întrebare
        }

        Debug.Log(numberOfQuestions + " întrebări completate.");
        // La final, poți reveni la jocul principal
    }




    void CheckForSpecialTile(Vector2 position)
    {
        // Provocările "Sansa"
        Vector2[] chanceTiles = { new Vector2(15, 2), new Vector2(13, 0), new Vector2(10, 0), new Vector2(0, 7), new Vector2(0, 1), new Vector2(3, 8), new Vector2(9, 8), new Vector2(14, 8) };
        foreach (Vector2 tile in chanceTiles)
        {
            if (position == tile)
            {
                Debug.Log("Chance challenge! Loading one random question.");
                SceneManager.LoadScene("DialogueScene");
                return;
            }
        }

        // Provocările "Capcana"
        Vector2[] trapTiles = { new Vector2(15, 3), new Vector2(5, 0), new Vector2(0, 4), new Vector2(7, 8) };
        foreach (Vector2 tile in trapTiles)
        {
            if (position == tile)
            {
                int randomChoice = UnityEngine.Random.Range(0, 3);
                switch (randomChoice)
                {
                    case 0:
                        Debug.Log("Trap: Random question.");
                        SceneManager.LoadScene("DialogueScene");
                        break;
                    case 1:
                        Debug.Log("Trap: Roll the dice, need between 3 and 8 to continue.");
                        int diceResult = RollDice();
                        if (diceResult >= 3 && diceResult <= 8)
                        {
                            Debug.Log("Correct roll, player can continue.");
                        }
                        else
                        {
                            Debug.Log("Incorrect roll, player stays.");
                        }
                        break;
                    case 2:
                        Debug.Log("Trap: Skip a turn.");
                        // Implement logic to skip a turn
                        break;
                }
                return;
            }
        }

        // Provocarea "Pietre" (3 întrebări)
        if (position == new Vector2(15, 5))
        {
            Debug.Log("Stone challenge! 3 questions in a row.");
            StartCoroutine(HandleMultipleQuestions(3));  // Afișează 3 întrebări pe rând
            return;
        }

        // Provocarea "Mlastina" (3 întrebări, răspuns greșit = o tură pierdută)
        if (position == new Vector2(15, 0))
        {
            Debug.Log("Swamp challenge! 3 questions with penalty.");
            StartCoroutine(HandleMultipleQuestionsWithPenalty(3));
            return;
        }

        // Provocarea "Moon" (pierzi o tură)
        if (position == new Vector2(0, 6))
        {
            Debug.Log("Moon challenge! Skip a turn.");
            // Implement skip turn logic here
            return;
        }

        // Provocarea "Scara" (2 întrebări)
        if (position == new Vector2(13, 8))
        {
            Debug.Log("Ladder challenge! 2 questions.");
            StartCoroutine(HandleMultipleQuestions(2));  // Afișează 2 întrebări pe rând
            return;
        }

        // Provocarea "Map" (aruncă zarul și te întorci)
        if (position == new Vector2(2, 0))
        {
            Debug.Log("Map challenge! Roll the dice and move back by that number.");
            int diceResult = RollDice();
            currentTileIndex -= diceResult;
            if (currentTileIndex < 0) currentTileIndex = 0;
            MovePawnToPosition(path[currentTileIndex]);
            return;
        }

        // Provocarea "Wings" (avansezi 3 tile-uri)
        if (position == new Vector2(5, 8))
        {
            Debug.Log("Wings challenge! The pawn advances 3 positions.");
            currentTileIndex += 3;
            if (currentTileIndex >= path.Count) currentTileIndex = path.Count - 1;
            MovePawnToPosition(path[currentTileIndex]);
            return;
        }
    }


    private IEnumerator HandleMultipleQuestionsWithPenalty(int numberOfQuestions)
    {
        for (int i = 0; i < numberOfQuestions; i++)
        {
            SceneManager.LoadScene("DialogueScene");
            yield return new WaitUntil(() => FindObjectOfType<QuizManager>().IsQuizComplete()); // Așteaptă finalizarea întrebării

            if (!FindObjectOfType<QuizManager>().WasAnswerCorrect()) // Verifică dacă răspunsul a fost greșit
            {
                Debug.Log("Wrong answer! Penalty applied (skip turn or other penalty).");
                // Aplică penalitatea - de exemplu, sări o tură
                break;
            }
        }
    }


    // Adaugă această metodă pentru a salva indexul curent al tile-ului în timpul interacțiunii cu DialogueScene
    public void ContinueFromCurrentPosition()
    {
        MovePawnToPosition(path[currentTileIndex]);
    }
}
