using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Player selectedPlayer = null;
    private GridManager gridManager;

    public void HandleTileClicked(Tile tile)
    {
        if (selectedPlayer == null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Player player = hit.collider.GetComponent<Player>();
                if (player != null)
                {
                    SelectPlayer(player, tile);
                }
            }
        }
        else
        {
            MovePlayer(tile);
        }
    }

    void SelectPlayer(Player player, Tile tile)
    {
        selectedPlayer = player;
        selectedPlayer.Select();

        // Highlight available moves
        List<Vector2> availableMoves = selectedPlayer.GetAvailableMoves(tile.gridPosition);
        foreach (var move in availableMoves)
        {
            Tile targetTile = gridManager.GetTilePosition(move); // Assuming GetTilePosition takes a Vector2
            if (targetTile != null)
            {
                targetTile.HighlightTilePlayer(true); // Highlight available move
            }
        }
    }

    void MovePlayer(Tile targetTile)
    {
        if (selectedPlayer != null)
        {
            // Move the piece
            selectedPlayer.transform.position = targetTile.transform.position;

            // Reset highlights
            foreach (var tile in gridManager.GetComponentsInChildren<Tile>())
            {
                tile.ResetHighlight();
            }

            // Deselect piece
            selectedPlayer.Deselect();
            selectedPlayer = null;
        }
    }
}
