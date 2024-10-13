using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;


    public int width;
    public int height;

    public Tile tilePrefab;

    public Transform cam;

    private Dictionary<Vector2, Tile> tiles= new Dictionary<Vector2, Tile>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GenerateGrid() // Generate a grid of cubes
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

                    var isOffset = (x + y) % 2 == 1; // Check if the tile is offset
                    tile.Init(isOffset); // Set the color of the tile

                    tiles[new Vector2(x, y)] = tile; // Add the tile to the dictionary
                }
            }
        }


        cam.transform.position = new Vector3((width / 2f) - 0.5f, (height / 2f) - 0.5f, -10); // Move the camera to the center of the grid


        GameManager.Instance.ChangeState(GameState.MainMenu); // Change the state to the main menu
    }

    public Tile GetTile(Vector2 position)
    {
        if (tiles.ContainsKey(position))
        {
            return tiles[position];
        }
        return null;
     
    }

    // Start is called before the first frame update
    void Start()
    {
        this.GenerateGrid();
    }

}
