#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RemoteTest))]
public class RemoteEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector();

    //    RemoteTest remote = (RemoteTest)target;

    //    GUILayout.Space(10);

    //    if (GUILayout.Button("Spawn Buttons"))
    //    {
    //        remote.OnUpdateData();
    //    }
    //    if (GUILayout.Button("SetUp Buttons"))
    //    {
    //        remote.FoodSetup();
    //    }
    //}
}
#endif
