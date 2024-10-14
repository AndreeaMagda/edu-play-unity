using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; // Add this for Tilemap

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private Tilemap groundMap, specialMap;
    [SerializeField] private int LevelIndex;

    public void SaveMap()
    {
        var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();

        newLevel.LevelIndex = LevelIndex;
        newLevel.name = "Level " + LevelIndex;

        newLevel.GroundTiles = GetTilesFromMap(groundMap).ToList();
        newLevel.SpecialTiles = GetTilesFromMap(specialMap).ToList();

        ScriptableObjectUtility.SaveLevelFile(newLevel);

        IEnumerable <SavedTile> GetTilesFromMap(Tilemap map)
        {
            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                if (map.HasTile(pos))
                {
                    var LevelTile = map.GetTile<LevelTile>(pos);

                    yield return new SavedTile
                    {
                        position = pos,
                        Tile = LevelTile
                    };
                }
            }
        }
    }

    public void LoadMap()
    {

    }

    public void ClearMap()
    {

    }
}
#if UNITY_EDITOR

public static class ScriptableObjectUtil{
    public static void SaveLevelFile(ScriptableLevel level)
    {
        string path = "Assets/Resources/Levels/"+ level.name + ".asset";
        UnityEditor.AssetDatabase.CreateAsset(level, path);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }
}
#endif

