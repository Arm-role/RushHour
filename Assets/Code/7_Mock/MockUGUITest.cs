using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemMockTest))]
public class MockUGUITest : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemMockTest mockTest = (ItemMockTest)target;

        GUILayout.Space(10);

        if (GUILayout.Button("spawn1"))
        {
            mockTest.OnSpawn1();
        }
        if (GUILayout.Button("spanwn2"))
        {
            mockTest.OnSpawn2();
        }
    }
}