using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isSelected = false;
    public new Renderer renderer;
    private Color OrigColor;

    private void Start()
    {
       renderer = GetComponent<Renderer>();
       OrigColor = renderer.material.color;
    }

    public void Select() 
    { 
        isSelected = true;
        renderer.material.color = Color.red;
    }

    public void Deselect()
    {
        isSelected = false;
        renderer.material.color = OrigColor;
    }

    public List<Vector2> GetAvailableMoves(Vector2 currentPos)
    {

        // Basic movement for chess-like logic (e.g., up, down, left, right)
        List<Vector2> moves = new List<Vector2>();
        moves.Add(currentPos + Vector2.up);
        moves.Add(currentPos + Vector2.down);
        moves.Add(currentPos + Vector2.left);
        moves.Add(currentPos + Vector2.right);

        return moves;
    }
}
