using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableLevel : ScriptableObject
{
    public int LevelIndex;
    public List<SavedTile> GroundTiles;
    public List<SavedTile> SpecialTiles;
}

public class SavedTile
{
    public Vector3Int position;
    public LevelTile Tile;
}
