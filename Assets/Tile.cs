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

    void onMouseEnter()
    {
        highlight.SetActive(true);
    }

    void onMouseExit()
    {
        highlight.SetActive(false);
    }

}
