using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GridManager : MonoBehaviour
{
    public int width = 16;
    public int height = 9;

    public Tile tilePrefab;

    public Transform cam;

    public Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
    private bool isGridLoadedFromJson = false; // Flag to track if grid is loaded from JSON

    public void GenerateGrid()
    {
        if (isGridLoadedFromJson)
        {
            Debug.Log("Grid is already loaded from JSON. Skipping GenerateGrid.");
            return;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // To create only the edge of the grid
                if (x == 0 || y == 0 || y == height - 1 || x == width - 1)
                {
                    Tile tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    tile.name = "Tile " + x + ", " + y;

                    var isOffset = (x + y) % 2 == 1; // Check if the tile is offset
                    tile.Init(isOffset); // Set the color of the tile

                    tiles[new Vector2(x, y)] = tile; // Add the tile to the dictionary
                }
            }
        }

        cam.transform.position = new Vector3((width / 2f) - 0.5f, (height / 2f) - 0.5f, -10); // Move the camera to the center of the grid
    }

    public void SaveGridToJson(string filePath)
    {
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath); // Create the directory if it doesn't exist
        }

        if (!File.Exists(filePath))
        {
            // Create the file if it does not exist
            File.WriteAllText(filePath, "{}"); // Example of creating an empty JSON file
        }

        GridData gridData = new GridData();
        foreach (var tileEntry in tiles)
        {
            Tile tile = tileEntry.Value;
            TileData tileData = new TileData()
            {
                x = (int)tileEntry.Key.x,
                y = (int)tileEntry.Key.y,
                tileType = tile.GetType().Name,
                tileColor = tile.GetTileColor(),
                highlightColor = tile.GetHighlightColor()
            };
            gridData.tiles.Add(tileData);
        }

        string json = JsonUtility.ToJson(gridData, true);
        File.WriteAllText(filePath, json);

        Debug.Log("Grid saved to: " + filePath);
    }

    public void LoadGridFromJson(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return;
        }

        ClearTiles(); // Clear the grid before loading

        string json = File.ReadAllText(filePath);
        GridData gridData = JsonUtility.FromJson<GridData>(json);

        foreach (var tileData in gridData.tiles)
        {
            Vector2 position = new Vector2(tileData.x, tileData.y);
            Tile tile = Instantiate(tilePrefab, new Vector3(tileData.x, tileData.y, 0), Quaternion.identity);
            tile.name = $"Tile {tileData.x}, {tileData.y}";

            // Initialize the tile if necessary, based on the tileType
            var isOffset = (tileData.x + tileData.y) % 2 == 1;
            tile.Init(isOffset);

            // Set the colors
            tile.SetTileColor(tileData.tileColor);
            tile.SetHighlightColor(tileData.highlightColor);
            Debug.Log("Colors loaded");
            tiles[position] = tile;
        }

        isGridLoadedFromJson = true; // Set the flag to true after loading from JSON
        Debug.Log($"Grid loaded from {filePath}");
    }

    public void ClearTiles()
    {
        foreach (var tile in tiles.Values)
        {
            if (Application.isPlaying)
            {
                Destroy(tile.gameObject);  // Destroy all tiles in play mode
            }
            else
            {
                DestroyImmediate(tile.gameObject);  // Destroy all tiles in edit mode
            }
        }
        tiles.Clear();  // Clear the dictionary of tiles
    }

    // Start is called before the first frame update
    void Start()
    {
/*        GenerateGrid();*/
    }
}
