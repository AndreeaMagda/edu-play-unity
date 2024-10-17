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
        //clear the grid before loading
        ClearGrid();
        gridManager.LoadGridFromJson(filePath);
    }

    public void ClearGrid()
    {
        gridManager.ClearTiles();  // Use the public method to clear the grid
    }
}
