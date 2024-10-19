using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintsManager : MonoBehaviour
{
    public GameObject sprint; // Pionul tău (veverița)
    private Vector2 currentTilePosition = new Vector2(15, 8); // Poziția curentă a pionului pe grilă
    private int currentTileIndex = 0; // Indexul poziției curente din traseu

    // Lista cu coordonatele traseului
    private List<Vector2> path = new List<Vector2>()
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

    // Start is called before the first frame update
    void Start()
    {
        // Mutăm pionul la poziția de start
        MovePawnToPosition(path[currentTileIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int diceResult = RollDice();
            Debug.Log("Rezultatul zarului: " + diceResult);
            StartCoroutine(MovePawn(diceResult)); // Mută pionul în funcție de rezultatul zarului
        }
    }

    // Funcție pentru a genera rezultatul zarului (între 1 și 6)
    int RollDice()
    {
        return Random.Range(1, 7);
    }

    // Funcție pentru a muta pionul pe tabla de joc
    IEnumerator MovePawn(int diceResult)
    {
        for (int i = 0; i < diceResult; i++)
        {
            currentTileIndex++; // Avansează la următoarea poziție

            // Dacă depășim limita traseului, putem fie să oprim, fie să începem din nou (în funcție de regulile jocului)
            if (currentTileIndex >= path.Count)
            {
                Debug.Log("Ai ajuns la finalul traseului.");
                currentTileIndex = path.Count - 1; // Sau resetezi la 0 dacă vrei să reînceapă traseul
                break;
            }

            Vector2 startPosition = sprint.transform.position; // Poziția de start a pionului
            Vector2 endPosition = path[currentTileIndex]; // Poziția finală

            float journeyLength = Vector2.Distance(startPosition, endPosition); // Distanța totală
            float startTime = Time.time; // Timpul de start

            while (Vector2.Distance(sprint.transform.position, endPosition) > 0.01f)
            {
                // Calculăm cât timp a trecut
                float distCovered = (Time.time - startTime) * 3f; // Viteza de mișcare
                float fractionOfJourney = distCovered / journeyLength; // Proporția de parcurs

                // Interpolăm între cele două poziții
                sprint.transform.position = Vector2.Lerp(startPosition, endPosition, fractionOfJourney);

                yield return null; // Așteptăm următorul frame
            }

            // Asigură-te că pionul ajunge exact la poziția finală
            MovePawnToPosition(endPosition);
        }
    }

    // Funcție pentru a muta efectiv pionul la o poziție din lista de vectori
    void MovePawnToPosition(Vector2 newPosition)
    {
        sprint.transform.position = new Vector3(newPosition.x, newPosition.y, 0); // Mutăm pionul la noua poziție
        Debug.Log("Pionul mutat la: " + newPosition);
    }
}
