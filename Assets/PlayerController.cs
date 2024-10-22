using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Pentru imagini UI

public class PlayerController : MonoBehaviour
{
    public GameObject playerInstance;
    public Image diceImage; // Pentru a afișa imaginea zarului
    public Sprite[] diceSprites; // Aici adăugăm sprite-urile cu zarurile
    private int currentTileIndex = 0;
    private List<Vector2> path;

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

        MovePawnToPosition(new Vector2(15, 8));
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

    
        diceImage.sprite = diceSprites[diceResult - 1]; 

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
        playerInstance.transform.position = new Vector2(newPosition.x, newPosition.y);
        Debug.Log("Pionul mutat la: " + newPosition);
    }
}
