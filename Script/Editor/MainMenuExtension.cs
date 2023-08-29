using UnityEditor;

public class MainMenuExtension
{

    #region Menu : Directories

    [MenuItem("ArmyAnt/Directories/Current Directory")]
    public static void Directories_CurrentDirectory()
    {
        ShowInSystemExplorer(System.Environment.CurrentDirectory);
    }

    [MenuItem("ArmyAnt/Directories/Data Path")]
    public static void Directories_DataPath()
    {
        ShowInSystemExplorer(UnityEngine.Application.dataPath);
    }

    [MenuItem("ArmyAnt/Directories/Persistent Data Path")]
    public static void Directories_PersistentDataPath()
    {
        ShowInSystemExplorer(UnityEngine.Application.persistentDataPath);
    }

    [MenuItem("ArmyAnt/Directories/Streaming Assets Path")]
    public static void Directories_StreamingAssetsPath()
    {
        ShowInSystemExplorer(UnityEngine.Application.streamingAssetsPath);
    }

    [MenuItem("ArmyAnt/Directories/Temporary Cache Path")]
    public static void Directories_TemporaryCachePath()
    {
        ShowInSystemExplorer(UnityEngine.Application.temporaryCachePath);
    }

    [MenuItem("ArmyAnt/Directories/Console Log Path")]
    public static void Directories_ConsoleLogPath()
    {
        ShowInSystemExplorer(UnityEngine.Application.consoleLogPath);
    }

    private static void ShowInSystemExplorer(string path)
    {
#if UNITY_EDITOR_OSX
        EditorUtility.RevealInFinder(path);
#else
        EditorUtility.DisplayDialog("功能未完成", "功能在此平台上没有完成！", "ok");
#endif
    }

    #endregion Menu : Directories
}
