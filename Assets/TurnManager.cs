using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject[] players;  // Array of player GameObjects
    private int activePlayerIndex = 0; // Tracks the active player's turn
    private PlayerController currentActivePlayer;

    void Start()
    {
        if (players.Length == 0)
        {
            Debug.LogError("No players assigned!");
            return;
        }

        // Start with the first player as active
        SetActivePlayer(0);
    }

    void Update()
    {
        // Example of triggering a turn change with space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeTurn();
        }
    }

    // Function to change the active player
    public void ChangeTurn()
    {
        // Deactivate the current player
        currentActivePlayer.SetPlayerActive(false);

        // Move to the next player's turn
        activePlayerIndex = (activePlayerIndex + 1) % players.Length;

        // Set the new player as active
        SetActivePlayer(activePlayerIndex);
    }

    private void SetActivePlayer(int playerIndex)
    {
        currentActivePlayer = players[playerIndex].GetComponent<PlayerController>();

        if (currentActivePlayer != null)
        {
            currentActivePlayer.SetPlayerActive(true);
            Debug.Log("Player " + playerIndex + " turn.");
        }
        else
        {
            Debug.LogError("PlayerController component missing on player " + playerIndex);
        }
    }
}