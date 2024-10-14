using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Level Tile", menuName = "Tiles/Level Tile")]

public class LevelTile : Tile
{
    public TileType Tile;
}

[Serializable]
public enum TileType
{
    Normal=0,
    Question=1,
    Rocks=2,
    Water=3,
    Angel=4,
}
