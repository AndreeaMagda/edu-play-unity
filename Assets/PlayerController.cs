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

    // Referință către SpecialTileManager
    private SpecialTileManager specialTileManager;

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
        PlayerPrefs.SetInt("CurrentTileIndex", currentTileIndex);
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

        for (int i = 0; i < diceResult; i++)
        {
            currentTileIndex++;

            if (currentTileIndex >= path.Count)
            {
                Debug.Log("You've reached the end of the path.");
                currentTileIndex = path.Count - 1;
                break;
            }

            Vector2 startPosition = currentPawn.transform.position;
            Vector2 endPosition = path[currentTileIndex];

            float journeyLength = Vector2.Distance(startPosition, endPosition);
            float startTime = Time.time;

            while (Vector2.Distance(currentPawn.transform.position, endPosition) > 0.01f)
            {
                float distCovered = (Time.time - startTime) * 3f;
                float fractionOfJourney = distCovered / journeyLength;

                currentPawn.transform.position = Vector2.Lerp(startPosition, endPosition, fractionOfJourney);
                yield return null;
            }

            MovePawnToPosition(endPosition);
        }

        // Verificăm dacă pionul a aterizat pe un tile special
        StartCoroutine(specialTileManager.CheckForSpecialTile(path[currentTileIndex]));

        // După ce pionul se mișcă, trecem la următorul jucător
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
        currentPlayerIndex = (currentPlayerIndex + 1) % pawns.Length;
        Debug.Log("It's now Player " + (currentPlayerIndex + 1) + "'s turn.");
    }

    public void ContinueFromCurrentPosition()
    {
        MovePawnToPosition(path[currentTileIndex]);
    }

    public void MovePawnForward(int steps)
    {
        currentTileIndex += steps;
        if (currentTileIndex >= path.Count) currentTileIndex = path.Count - 1;
        MovePawnToPosition(path[currentTileIndex]);
    }

    public void MovePawnBack(int steps)
    {
        currentTileIndex -= steps;
        if (currentTileIndex < 0) currentTileIndex = 0;
        MovePawnToPosition(path[currentTileIndex]);
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
