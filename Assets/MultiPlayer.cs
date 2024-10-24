/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayer : MonoBehaviour
{
    public GameObject currentPlayer;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    void Turn(PlayerController player)
    {
        player.Update();
    }

    public void ChangeTurn()
    {
        switch(currentPlayer)
        {
            case player1:
                PlayerPrefs.SetInt("CurrentTileIndexP1", currentTileIndex);
                currentPlayer = player2;
                break;
            case player2:
                PlayerPrefs.SetInt("CurrentTileIndexP2", currentTileIndex);
                currentPlayer = player1;
                break;
            case player3:
                PlayerPrefs.SetInt("CurrentTileIndexP3", currentTileIndex);
                currentPlayer = player4;
                break;
            case player4:
                PlayerPrefs.SetInt("CurrentTileIndexP4", currentTileIndex);
                currentPlayer = player3;
                break;
            default:
                break;
        }
    }
    
}
*/