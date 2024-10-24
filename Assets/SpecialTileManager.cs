using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpecialTileManager : MonoBehaviour
{
    // Listele de tiles speciale
    public Vector2[] chanceTiles;  // "Sansa" challenges
    public Vector2[] trapTiles;    // "Capcane" challenges
    public Vector2 stoneTile;      // "Pietre" challenge
    public Vector2 swampTile;      // "Mlastina" challenge
    public Vector2 moonTile;       // "Moon" challenge
    public Vector2 ladderTile;     // "Scara" challenge
    public Vector2 mapTile;        // "Map" challenge
    public Vector2 wingsTile;      // "Wings" challenge

    public IEnumerator CheckForSpecialTile(Vector2 position)
    {
        // Provocările "Sansa"
        Vector2[] chanceTiles = { new Vector2(15, 2), new Vector2(13, 0), new Vector2(10, 0), new Vector2(0, 7), new Vector2(0, 1), new Vector2(3, 8), new Vector2(9, 8), new Vector2(14, 8) };
        foreach (Vector2 tile in chanceTiles)
        {
            if (position == tile)
            {
                Debug.Log("Chance challenge! Loading one random question.");
                SceneManager.LoadScene("DialogueScene");
                yield break;
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
                        int diceResult = FindObjectOfType<PlayerController>().RollDice();
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
                        // Aici vei implementa logica pentru a sări o tură
                        break;
                }
                yield break;
            }
        }

        // Provocarea "Pietre" (3 întrebări)
        if (position == new Vector2(15, 5))
        {
            Debug.Log("Stone challenge! 3 questions in a row.");
            yield return FindObjectOfType<PlayerController>().HandleMultipleQuestions(3);
            yield break;
        }

        // Provocarea "Mlastina" (3 întrebări, răspuns greșit = o tură pierdută)
        if (position == new Vector2(15, 0))
        {
            Debug.Log("Swamp challenge! 3 questions with penalty.");
            yield return FindObjectOfType<PlayerController>().HandleMultipleQuestionsWithPenalty(3);
            yield break;
        }

        // Provocarea "Moon" (pierzi o tură)
        if (position == new Vector2(0, 6))
        {
            Debug.Log("Moon challenge! Skip a turn.");
            // Implement logic to skip a turn
            yield break;
        }

        // Provocarea "Scara" (2 întrebări)
        if (position == new Vector2(13, 8))
        {
            Debug.Log("Ladder challenge! 2 questions.");
            yield return FindObjectOfType<PlayerController>().HandleMultipleQuestions(2);
            yield break;
        }

        // Provocarea "Map" (aruncă zarul și te întorci)
        if (position == new Vector2(2, 0))
        {
            Debug.Log("Map challenge! Roll the dice and move back by that number.");
            int diceResult = FindObjectOfType<PlayerController>().RollDice();
            FindObjectOfType<PlayerController>().MovePawnBack(diceResult);
            yield break;
        }

        // Provocarea "Wings" (avansezi 3 tile-uri)
        if (position == new Vector2(5, 8))
        {
            Debug.Log("Wings challenge! The pawn advances 3 positions.");
            FindObjectOfType<PlayerController>().MovePawnForward(3);
            yield break;
        }
    }
}
