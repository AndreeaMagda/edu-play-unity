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
    Normal,
    Question,
    Rocks,
    Water,
    Angel,
}
