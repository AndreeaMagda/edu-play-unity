using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(TilemapManager))]
public class TileMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (TilemapManager) target;

        if(GUILayout.Button("Save Map"))
        {
            script.SaveMap();
        }
    }
}
