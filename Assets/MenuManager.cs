using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    void Awake()
    {
        GameManager.OnStateChange += HandleOnStateChange;
    }

    void OnDestroy()
    {
        GameManager.OnStateChange -= HandleOnStateChange;
    }

    private void HandleOnStateChange(GameState state)
    {
        switch (state)
        {
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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
