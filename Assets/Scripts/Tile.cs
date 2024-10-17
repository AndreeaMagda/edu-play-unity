using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer render;
    public GameObject highlight;

    string baseHex= "FFF7D1";
    Color baseColor;

    string offsetHex = "FFD09B";
    Color offsetColor;

    string highlightHex = "373333";
    Color highlightColor;
   
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
}
