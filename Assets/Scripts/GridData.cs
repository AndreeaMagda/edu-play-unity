using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GridData 
{
    public List<TileData> tiles = new List<TileData>();
}

[System.Serializable]
public class TileData
{
    public int x;
    public int y;
    public string tileType;
    public string tileColor;
    public string highlightColor;
}