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

        if (GUILayout.Button("Test"))
        {
            mockTest.OnTest();
        }
    }
}