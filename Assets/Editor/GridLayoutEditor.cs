using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridLayoutManager))]
public class GridLayoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // แสดง UI ปกติของ Inspector

        GridLayoutManager grid = (GridLayoutManager)target;

        if (GUILayout.Button("Arrange Children"))
        {
            grid.ArrangeChildren();
        }
    }
}
