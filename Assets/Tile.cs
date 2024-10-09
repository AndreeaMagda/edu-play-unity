using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color baseColor, offsetColor;
    public SpriteRenderer render;
    public GameObject highlight;
    
    public void Init(bool isOffset)
    {
        render.color = isOffset ? offsetColor : baseColor;// if for seeing what color to use
    }
    private void OnMouseEnter()
    {
        if (highlight != null)
        {
            highlight.SetActive(true);
            Debug.Log("Mouse Enter");
        }
        else
        {
            Debug.LogWarning("Highlight GameObject is not assigned.");
        }
    }

    private void OnMouseExit()
    {
        if (highlight != null)
        {
            highlight.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Highlight GameObject is not assigned.");
        }
    }
}
