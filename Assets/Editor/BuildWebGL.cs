using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.IO;

public static class BuildWebGL
{
    // Call with -executeMethod BuildWebGL.PerformBuild in CI or run from Editor menu (Tools/Build WebGL)
    [MenuItem("Tools/Build WebGL")]
    public static void PerformBuild()
    {
        // Ensure Scenes folder exists
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        string scenePath = "Assets/Scenes/WebGLDemo.unity";
        if (!File.Exists(scenePath))
        {
            // Create a new scene and populate it with demo objects
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Add chunk generator
            var chunkGO = new GameObject("Chunk");
            chunkGO.AddComponent(typeof(VoxelEngine.VoxelChunkGenerator));

            // Add player capsule
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.transform.position = new Vector3(0, 10, 0);
            var cc = player.AddComponent<CharacterController>();
            var pc = player.AddComponent(typeof(VoxelEngine.PlayerController));

            // Add camera as child
            var camGO = new GameObject("Main Camera");
            var cam = camGO.AddComponent<Camera>();
            cam.tag = "MainCamera";
            cam.transform.SetParent(player.transform);
            cam.transform.localPosition = new Vector3(0, 1.2f, 0);
            cam.transform.localEulerAngles = Vector3.zero;

            // Save the scene
            EditorSceneManager.SaveScene(scene, scenePath);
            Debug.Log("Created demo scene at " + scenePath);
        }
        else
        {
            Debug.Log("Demo scene already exists: " + scenePath);
        }

        // Set WebGL player settings
#if UNITY_2020_1_OR_NEWER
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.WebGL, ScriptingImplementation.IL2CPP);
        PlayerSettings.WebGL.memorySize = 256;
        PlayerSettings.WebGL.decompressionFallback = false;
        PlayerSettings.WebGL.emscriptenArgs = "";
        // Set Brotli compression if available
        #if UNITY_2020_2_OR_NEWER
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
        #endif
#endif

        // Build
        string buildPath = Path.Combine("Builds", "WebGL");
        if (!Directory.Exists(buildPath)) Directory.CreateDirectory(buildPath);

        string[] scenes = { "Assets/Scenes/WebGLDemo.unity" };
        Debug.Log("Building WebGL to: " + buildPath);
        var opts = new BuildPlayerOptions();
        opts.scenes = scenes;
        opts.locationPathName = buildPath;
        opts.target = BuildTarget.WebGL;
        opts.options = BuildOptions.None;

        var report = BuildPipeline.BuildPlayer(opts);
        if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log("WebGL build succeeded: " + report.summary.totalSize + " bytes");
        }
        else
        {
            Debug.LogError("WebGL build failed: " + report.summary.result);
        }
    }
}
