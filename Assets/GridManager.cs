using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width;
    public int height;

    public Tile tilePrefab;

    public Transform cam;

    void GenerateGrid() // Generate a grid of cubes
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {   //To create only the edge of the grid
                if (x == 0 || y == 0 || y == height - 1 || x == width - 1)
                {
                    // Create a new cube
                    Tile tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    tile.name = "Tile " + x + ", " + y;
                    Debug.Log(tile);
                    var isOffset = (x + y) % 2 == 1; // Check if the tile is offset
                    tile.Init(isOffset); // Set the color of the tile
                }
            }
        }


        cam.transform.position = new Vector3((width / 2f) - 0.5f, (height / 2f) - 0.5f, -10); // Move the camera to the center of the grid
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

}
