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

    void CheckForSpecialTile(Vector2 position)
    {
        foreach (Vector2 tile in chanceTiles)
        {
            if (position == tile)
            {
                Debug.Log("Chance challenge! Loading question.");
                SceneManager.LoadScene("DialogueScene");
                return;
            }
        }

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
                        Debug.Log("Trap: Roll the dice and need to get between 3 and 8 to continue.");
                        break;
                    case 2:
                        Debug.Log("Trap: Skip a turn.");
                        break;
                }
                return;
            }
        }

        // Verifică alte tile-uri speciale
        if (position == stoneTile)
        {
            Debug.Log("Stone challenge! 3 questions in a row.");
            SceneManager.LoadScene("DialogueScene");
            return;
        }

        if (position == swampTile)
        {
            Debug.Log("Swamp challenge! 3 questions with penalty.");
            SceneManager.LoadScene("DialogueScene");
            return;
        }

        if (position == moonTile)
        {
            Debug.Log("Moon challenge! Skip a turn.");
            return; // Implement logic to skip a turn
        }

        if (position == ladderTile)
        {
            Debug.Log("Ladder challenge! 2 questions.");
            SceneManager.LoadScene("DialogueScene");
            return;
        }

        if (position == mapTile)
        {
            Debug.Log("Map challenge! Move back by the number indicated by the dice.");
            int diceResult = UnityEngine.Random.Range(1, 7);
            currentTileIndex -= diceResult;
            if (currentTileIndex < 0) currentTileIndex = 0;
            MovePawnToPosition(path[currentTileIndex]);
            return;
        }

        if (position == wingsTile)
        {
            Debug.Log("Wings challenge! The pawn advances 3 positions.");
            currentTileIndex += 3;
            if (currentTileIndex >= path.Count) currentTileIndex = path.Count - 1;
            MovePawnToPosition(path[currentTileIndex]);
        }
    }

    // Adaugă această metodă pentru a salva indexul curent al tile-ului în timpul interacțiunii cu DialogueScene
    public void ContinueFromCurrentPosition()
    {
        MovePawnToPosition(path[currentTileIndex]);
    }
}
