using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridEditor))]
[CanEditMultipleObjects]
public class GridEditorEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GridEditor script = (GridEditor)target;

        if (GUILayout.Button("generateMap")) {
            script.constructGrid(script.map);
        }
    }
}
