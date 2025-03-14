using UnityEditor;
using UnityEngine;

public class BuildToWebGLUnSwitch : MonoBehaviour
{
    [MenuItem("Build/Build WebGL")]
    public static void BuildWebGL()
    {
        string[] scenes = {
            "Assets/Resources/Scene/Start.unity",  // ลำดับที่ 1
             "Assets/Resources/Scene/Load.unity",    // ลำดับที่ 2
             "Assets/Resources/Scene/Game.unity",    // ลำดับที่ 3
        };

        BuildPipeline.BuildPlayer(scenes, "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
        Debug.Log("WebGL Build Completed.");
    }
}
