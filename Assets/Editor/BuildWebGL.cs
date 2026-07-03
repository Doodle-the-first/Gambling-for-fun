using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
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

            // Add GameManager
            var gmGO = new GameObject("GameManager");
            var gm = gmGO.AddComponent<GameManager>();
            gm.startingBankroll = 1000;
            gm.minBet = 10;

            // Add SaveManager
            var smGO = new GameObject("SaveManager");
            smGO.AddComponent<SaveManager>();

            // Create Canvas for UI
            var canvasGO = new GameObject("Canvas");
            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();

            // HUD Text
            var hudTextGO = new GameObject("HUDText");
            hudTextGO.transform.SetParent(canvasGO.transform);
            var hudText = hudTextGO.AddComponent<Text>();
            hudText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            hudText.rectTransform.anchorMin = new Vector2(0, 1);
            hudText.rectTransform.anchorMax = new Vector2(0, 1);
            hudText.rectTransform.anchoredPosition = new Vector2(10, -10);
            hudText.alignment = TextAnchor.UpperLeft;
            hudText.fontSize = 18;
            hudText.text = "Bankroll: --";

            // Pot Text
            var potTextGO = new GameObject("PotText");
            potTextGO.transform.SetParent(canvasGO.transform);
            var potText = potTextGO.AddComponent<Text>();
            potText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            potText.rectTransform.anchorMin = new Vector2(0.5f, 1);
            potText.rectTransform.anchorMax = new Vector2(0.5f, 1);
            potText.rectTransform.anchoredPosition = new Vector2(0, -10);
            potText.alignment = TextAnchor.UpperCenter;
            potText.fontSize = 18;
            potText.text = "Pot: 0";

            // Result Text (center)
            var resultTextGO = new GameObject("ResultText");
            resultTextGO.transform.SetParent(canvasGO.transform);
            var resultText = resultTextGO.AddComponent<Text>();
            resultText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            resultText.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            resultText.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            resultText.rectTransform.anchoredPosition = new Vector2(0, -50);
            resultText.alignment = TextAnchor.MiddleCenter;
            resultText.fontSize = 16;
            resultText.text = "Results will show here.";

            // Bet Input
            var betInputGO = new GameObject("BetInput");
            betInputGO.transform.SetParent(canvasGO.transform);
            var input = betInputGO.AddComponent<InputField>();
            var inputTextGO = new GameObject("Text");
            inputTextGO.transform.SetParent(betInputGO.transform);
            var inputText = inputTextGO.AddComponent<Text>();
            inputText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            inputText.text = "100";
            input.textComponent = inputText;
            input.placeholder = null;
            inputText.alignment = TextAnchor.MiddleCenter;
            inputText.fontSize = 16;
            input.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, -30);

            // Roll Button
            var rollButtonGO = new GameObject("RollButton");
            rollButtonGO.transform.SetParent(canvasGO.transform);
            var btn = rollButtonGO.AddComponent<Button>();
            var btnTextGO = new GameObject("Text");
            btnTextGO.transform.SetParent(rollButtonGO.transform);
            var btnText = btnTextGO.AddComponent<Text>();
            btnText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            btnText.text = "Roll";
            btnText.alignment = TextAnchor.MiddleCenter;
            btnText.fontSize = 16;

            // Position UI elements roughly
            var rtInput = input.GetComponent<RectTransform>();
            rtInput.anchorMin = new Vector2(0.5f, 0);
            rtInput.anchorMax = new Vector2(0.5f, 0);
            rtInput.anchoredPosition = new Vector2(-60, 40);
            rtInput.sizeDelta = new Vector2(100, 30);

            var rtBtn = btn.GetComponent<RectTransform>();
            rtBtn.anchorMin = new Vector2(0.5f, 0);
            rtBtn.anchorMax = new Vector2(0.5f, 0);
            rtBtn.anchoredPosition = new Vector2(60, 40);
            rtBtn.sizeDelta = new Vector2(100, 30);

            // Wire UI to GameManager
            gm.hudText = hudText;
            gm.potText = potText;
            gm.rollButton = btn;
            gm.betInput = input;
            gm.resultText = resultText;

            // Add HUD component
            var hudGO = new GameObject("HUD");
            var hud = hudGO.AddComponent<HUD>();
            hud.gameManager = gm;
            hud.bankrollText = hudText;
            hud.potText = potText;
            hud.resultText = resultText;

            // Add WebInputHelper to camera
            camGO.AddComponent<WebInputHelper>();

            // Save scene
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
