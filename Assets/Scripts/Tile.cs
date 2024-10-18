using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer render;
    public GameObject highlight;
    public GameObject highlightPlayer;

    string baseHex= "FFF7D1";
    Color baseColor;

    string offsetHex = "FFD09B";
    Color offsetColor;

    string highlightHex = "373333";
    Color highlightColor;
   
    private GridManager gridManager;
    public Vector2 gridPosition;

    void Awake()
    {
        // Initialize colors in Awake method
        baseColor = ColorUtility.TryParseHtmlString("#" + baseHex, out baseColor) ? baseColor : Color.white;
        offsetColor = ColorUtility.TryParseHtmlString("#" + offsetHex, out offsetColor) ? offsetColor : Color.white;
        highlightColor = ColorUtility.TryParseHtmlString("#" + highlightHex, out highlightColor) ? highlightColor : Color.white;

        highlightColor.a = 190 / 255f;

        // Set the highlight color if highlight GameObject is assigned
        if (highlight != null)
        {
            var highlightRenderer = highlight.GetComponent<SpriteRenderer>();
            if (highlightRenderer != null)
            {
                highlightRenderer.color = highlightColor;
            }
            else
            {
                Debug.LogWarning("Highlight GameObject does not have a SpriteRenderer component.");
            }
        }
    }

    public void Init(bool isOffset)
    {
        render.color = isOffset ? offsetColor : baseColor;// if for seeing what color to use
    }
    private void OnMouseEnter()
    {
        if (highlight != null)
        {
            highlight.SetActive(true);
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


    private void OnMouseDown()
    {
        // Handle the tile click, pass the control to GridManager to handle the logic
        if (gridManager != null)
        {
            gridManager.TileClicked(this);
        }
    }


    public string GetTileColor()
    {
        return ColorUtility.ToHtmlStringRGBA(render.color);
    }

    public string GetHighlightColor()
    {
        if (highlight != null)
        {
            var highlightRenderer = highlight.GetComponent<SpriteRenderer>();
            if (highlightRenderer != null)
            {
                return ColorUtility.ToHtmlStringRGBA(highlightRenderer.color);
            }
        }
        return null;
    }

    /* public string GetHighlightColorPlayer()
     {
         if (highlightPlayer != null)
         {
             var highlightRenderer = highlightPlayer.GetComponent<SpriteRenderer>();
             if (highlightRenderer != null)
             {
                 return ColorUtility.ToHtmlStringRGBA(highlightRenderer.color);
             }
         }
         return null;
     }*/

    public void HighlightTilePlayer(bool highlight)
    {
        if (highlightPlayer != null)
        {
            highlightPlayer.SetActive(highlight);
        }
        else
        {
            Debug.LogWarning("HighlightPlayer GameObject is not assigned.");
        }
    }

    public void SetTileColor(string color)
    {
        if (ColorUtility.TryParseHtmlString("#" + color, out Color parsedColor))
        {
            render.color = parsedColor;
        }
    }

    public void SetHighlightColor(string color)
    {
        if (highlight != null)
        {
            var highlightRenderer = highlight.GetComponent<SpriteRenderer>();
            if (highlightRenderer != null && ColorUtility.TryParseHtmlString("#" + color, out Color parsedColor))
            {
                highlightRenderer.color = parsedColor;
            }
        }
    }

    public void ResetHighlight()
    {
        if (highlight != null)
        {
            highlight.SetActive(false);
        }
        if (highlightPlayer != null)
        {
            highlightPlayer.SetActive(false);
        }
    }
}
