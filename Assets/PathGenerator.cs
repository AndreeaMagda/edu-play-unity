using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public List<Vector2> GeneratePath()
    {
        List<Vector2> path = new List<Vector2>();

        path.Add(new Vector2(8, 8));

       
        for (int i = 1; i <= 8; i++)
        {
            path.Add(new Vector2(8, 8 - i)); // De la (15, 7) la (15, 0)
        }
        for (int i = 1; i <= 8; i++)
        {
            path.Add(new Vector2(8 - i, 0)); // De la (14, 0) la (0, 0)
        }
        for (int i = 1; i <= 8; i++)
        {
            path.Add(new Vector2(0, i)); // De la (0, 1) la (0, 8)
        }
        for (int i = 1; i <= 7; i++)
        {
            path.Add(new Vector2(i, 8)); // De la (1, 8) la (14, 8)
        }

        return path;
    }
}