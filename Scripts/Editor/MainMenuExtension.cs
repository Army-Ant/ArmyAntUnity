using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MainMenuExtension {

    #region Menu : Directories

    [MenuItem("ArmyAnt/Directories/Current Directory")]
    public static void Directories_CurrentDirectory() {
        ShowInSystemExplorer(System.Environment.CurrentDirectory);
    }

    [MenuItem("ArmyAnt/Directories/DataSource Path")]
    public static void Directories_DataPath() {
        ShowInSystemExplorer(Application.dataPath);
    }

    [MenuItem("ArmyAnt/Directories/Persistent DataSource Path")]
    public static void Directories_PersistentDataPath() {
        ShowInSystemExplorer(Application.persistentDataPath);
    }

    [MenuItem("ArmyAnt/Directories/Streaming Assets Path")]
    public static void Directories_StreamingAssetsPath() {
        ShowInSystemExplorer(Application.streamingAssetsPath);
    }

    [MenuItem("ArmyAnt/Directories/Temporary Cache Path")]
    public static void Directories_TemporaryCachePath() {
        ShowInSystemExplorer(Application.temporaryCachePath);
    }

    [MenuItem("ArmyAnt/Directories/Console Log Path")]
    public static void Directories_ConsoleLogPath() {
        ShowInSystemExplorer(Application.consoleLogPath);
    }

    private static void ShowInSystemExplorer(string path) {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
        EditorUtility.RevealInFinder(path);
#else
        EditorUtility.DisplayDialog("功能未完成", "功能在此平台上没有完成！", "ok");
#endif
    }

    #endregion Menu : Directories

    #region Menu : Tools

    [MenuItem("ArmyAnt/Tools/Clear Cache DataSource")]
    public static void Tools_ClearData() {
        if(EditorUtility.DisplayDialog("Clear Cache DataSource", "是否确实要清空所有缓存？", "OK", "Cancel")) {
            PlayerPrefs.DeleteAll();
            Caching.ClearCache();
            DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);
            foreach(FileInfo file in di.GetFiles())
                file.Delete();
            foreach(DirectoryInfo dir in di.GetDirectories())
                dir.Delete(true);
            Debug.Log("所有缓存已清空!");
        }
    }

    [MenuItem("ArmyAnt/Tools/PlayStartScene", true)]
    static bool ValidatePlayModeUseFirstScene() {
        Menu.SetChecked("ArmyAnt/Tools/PlayStartScene", EditorSceneManager.playModeStartScene != null);
        return !EditorApplication.isPlaying;
    }

    [MenuItem("ArmyAnt/Tools/PlayStartScene")]
    static void UpdatePlayModeUseFirstScene() {
        if(Menu.GetChecked("ArmyAnt/Tools/PlayStartScene")) {
            EditorSceneManager.playModeStartScene = null;
        } else {
            SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
            EditorSceneManager.playModeStartScene = scene;
        }
    }

    #endregion Menu : Tools
}
