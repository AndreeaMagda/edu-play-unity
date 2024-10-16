using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TilemapManager : MonoBehaviour
{
    public GridManager gridManager;
    public void SaveMap()
    {
        string filePath = Application.dataPath + "SavedData/grid.json";
        gridManager.SaveGridToJson(filePath);
    }

    public void LoadMap()
    {
        string filePath = Application.dataPath + "SavedData/grid.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GridData gridData = JsonUtility.FromJson<GridData>(json); // Deserialize

            // Now you can loop through the gridData.tiles to recreate the grid
            foreach (var tileData in gridData.tiles)
            {
                Debug.Log("Loaded tile at: " + tileData.x + ", " + tileData.y);
                // Use the loaded data to recreate the grid, e.g., instantiate tiles again
                gridManager.GenerateGrid();

            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }

    public void ClearGrid()
    {
        gridManager.ClearTiles();  // Use the public method to clear the grid
    }
}
