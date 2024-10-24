using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necesită pentru încărcarea scenei

public class PlayerController : MonoBehaviour
{
    public GameObject playerInstance;
    public Image diceImage;
    public Sprite[] diceSprites;
    private int currentTileIndex = 0;
    private List<Vector2> path;

    // Adaugă lista de tile-uri speciale
    public Vector2[] specialTiles;  // Poți seta din Inspector pozițiile speciale

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

        MovePawnToPosition(new Vector2(15, 8)); // Poziția de start
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int diceResult = RollDice();
            Debug.Log("Rezultatul zarului: " + diceResult);
            StartCoroutine(MovePawn(diceResult));
        }
    }

    int RollDice()
    {
        int diceResult = Random.Range(1, 7);

        diceImage.sprite = diceSprites[diceResult - 1]; // Actualizează imaginea zarului

        return diceResult;
    }

    IEnumerator MovePawn(int diceResult)
    {
        for (int i = 0; i < diceResult; i++)
        {
            currentTileIndex++;

            if (currentTileIndex >= path.Count)
            {
                Debug.Log("Ai ajuns la finalul traseului.");
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
    }

    void MovePawnToPosition(Vector2 newPosition)
    {
        playerInstance.transform.position = newPosition;
        Debug.Log("Pionul mutat la: " + newPosition);

        // Verifică dacă pionul a ajuns pe un tile special
        foreach (Vector2 tile in specialTiles)
        {
            if (newPosition == tile)
            {
                Debug.Log("Tile special atins! Încarcă scena de dialog.");
                SceneManager.LoadScene("DialogueScene"); // Încarcă DialogueScene
                return;
            }
        }
    }
}
