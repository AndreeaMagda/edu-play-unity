using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnStateChange;

    private GridManager gridManager;

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

    void Start()
    {
        // Find the GridManager in the scene .....if it would actually work
        gridManager = FindObjectOfType<GridManager>();

        if (gridManager == null)
        {
            Debug.LogError("GridManager not found.");
        }
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                // Generate the grid
                break;
            case GameState.MainMenu:
                // Show the main menu
                break;
            case GameState.InGame:
                // Start the game
                break;
            case GameState.GameOver:
                // Show the game over screen
                break;
        }

        OnStateChange?.Invoke(newState);
    }
}

public enum GameState
{
    GenerateGrid=0,
    MainMenu=1,
    SpawnCharacters=2,
    InGame=3,
    GameOver=4,
}

