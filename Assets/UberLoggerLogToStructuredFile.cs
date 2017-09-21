using UnityEngine;
using UberLogger;
using System;

/// <summary>
/// Place this component in the scene to log all console output to a file with a structured format
/// </summary>
public class UberLoggerLogToStructuredFile : MonoBehaviour
{
    public enum LogLocations { None, DataPath, PersistentDataPath, DataPathAndPersistentDataPath };

    private const string DefaultOutputFile = "output_log_structured.txt";

    [Serializable]
    public class Config
    {
        public LogLocations OutputFileLocations;
        public string OutputFile;
        public UberLoggerStructuredFile.IncludeCallstackMode IncludeCallStacks;
        public UberLoggerStructuredFile.ExistingFileMode ExistingFile;
    }

    public UberLoggerStructuredFile.IndentationSettings Indentation;

    public Config EditorPlayModeConfig = new Config {
        OutputFileLocations = LogLocations.PersistentDataPath,
        OutputFile = DefaultOutputFile,
        IncludeCallStacks = UberLoggerStructuredFile.IncludeCallstackMode.WarningsAndErrorsOnly,
        ExistingFile = UberLoggerStructuredFile.ExistingFileMode.DoNotOverwrite };
    public Config DevelopmentBuildConfig = new Config {
        OutputFileLocations = LogLocations.DataPathAndPersistentDataPath,
        OutputFile = DefaultOutputFile,
        IncludeCallStacks = UberLoggerStructuredFile.IncludeCallstackMode.WarningsAndErrorsOnly,
        ExistingFile = UberLoggerStructuredFile.ExistingFileMode.DoNotOverwrite };
    public Config ReleaseBuildConfig = new Config {
        OutputFileLocations = LogLocations.DataPathAndPersistentDataPath,
        OutputFile = DefaultOutputFile,
        IncludeCallStacks = UberLoggerStructuredFile.IncludeCallstackMode.WarningsAndErrorsOnly,
        ExistingFile = UberLoggerStructuredFile.ExistingFileMode.DoNotOverwrite };

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

#if UNITY_EDITOR
        Config config = EditorPlayModeConfig;
#else
#if DEVELOPMENT_BUILD
        Config config = DevelopmentBuildConfig;
#else
        Config config = ReleaseBuildConfig;
#endif
#endif

        // Create a logger that writes to persistentDataPath
        if (config.OutputFileLocations == LogLocations.PersistentDataPath || config.OutputFileLocations == LogLocations.DataPathAndPersistentDataPath)
        {
            UberLoggerStructuredFile uberLoggerFile = new UberLoggerStructuredFile(System.IO.Path.Combine(Application.persistentDataPath, config.OutputFile), Indentation, config.IncludeCallStacks, config.ExistingFile);
            UberLogger.Logger.AddLogger(uberLoggerFile);
        }

        // Create a logger that writes to dataPath
        // The file location is easy for people to find, but sometimes the location is read-only
        if (config.OutputFileLocations == LogLocations.DataPath || config.OutputFileLocations == LogLocations.DataPathAndPersistentDataPath)
        {
            UberLoggerStructuredFile uberLoggerFile = new UberLoggerStructuredFile(System.IO.Path.Combine(Application.dataPath, config.OutputFile), Indentation, config.IncludeCallStacks, config.ExistingFile);
            UberLogger.Logger.AddLogger(uberLoggerFile);
        }
    }
}