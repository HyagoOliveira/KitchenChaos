using Serilog;
using Serilog.Core;
using UnityEngine;
using UnityEditor;

namespace Meryel.UnityCodeAssist.Editor.Logger
{

    [InitializeOnLoad]
    public static class ELogger
    {
        public static event System.Action OnVsInternalLogChanged;


        // Change 'new LoggerConfiguration().MinimumLevel.Debug();' if you change these values
        const Serilog.Events.LogEventLevel fileMinLevel = Serilog.Events.LogEventLevel.Debug;
        const Serilog.Events.LogEventLevel outputWindowMinLevel = Serilog.Events.LogEventLevel.Information;
        static LoggingLevelSwitch fileLevelSwitch, outputWindowLevelSwitch;

        static bool IsInitialized { get; set; }

        static ILogEventSink _outputWindowSink;
        static ILogEventSink _memorySink;


        public static string GetInternalLogContent() => ((MemorySink)_memorySink).Export();
        public static int GetErrorCountInInternalLog() => ((MemorySink)_memorySink).ErrorCount;
        public static int GetWarningCountInInternalLog() => ((MemorySink)_memorySink).WarningCount;

        public static string FilePath { get; private set; }
        public static string VSFilePath { get; private set; }

        //**-- make it work with multiple clients
        static string _vsInternalLog;
        public static string VsInternalLog
        {
            get => _vsInternalLog;
            set
            {
                _vsInternalLog = value;
                OnVsInternalLogChanged?.Invoke();
            }
        }



        static ELogger()
        {
            var isFirst = false;
            const string stateName = "isFirst";
            if (!SessionState.GetBool(stateName, false))
            {
                isFirst = true;
                SessionState.SetBool(stateName, true);
            }

            var projectPath = CommonTools.GetProjectPath();
            var outputWindowSink = new System.Lazy<ILogEventSink>(() => new UnityOutputWindowSink(null));

            Init(isFirst, projectPath, outputWindowSink);

            if (isFirst)
                LogHeader(Application.unityVersion, projectPath);
            Serilog.Log.Debug("PATH: {Path}", projectPath);
        }


        static void LogHeader(string unityVersion, string solutionDir)
        {
            var os = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            var assisterVersion = Assister.Version;
            var syncModel = Synchronizer.Model.Utilities.Version;
            var hash = CommonTools.GetHashOfPath(solutionDir);
            Serilog.Log.Debug(
                "Beginning logging {OS}, Unity {U}, Unity Code Assist {A}, Communication Protocol {SM}, Project: '{Dir}', Project Hash: {Hash}",
                os, unityVersion, assisterVersion, syncModel, solutionDir, hash);
        }







        static string GetFilePath(string solutionDir)
        {
            var solutionHash = CommonTools.GetHashOfPath(solutionDir);
            var tempDir = System.IO.Path.GetTempPath();
            var fileName = $"UCA_U_LOG_{solutionHash}_.TXT"; // hour code will be appended to the end of file, so add a trailing '_'
            var filePath = System.IO.Path.Combine(tempDir, fileName);
            return filePath;
        }

        static string GetVSFilePath(string solutionDir)
        {
            var solutionHash = CommonTools.GetHashOfPath(solutionDir);
            var tempDir = System.IO.Path.GetTempPath();
            var fileName = $"UCA_VS_LOG_{solutionHash}_.TXT"; // hour code will be appended to the end of file, so add a trailing '_'
            var filePath = System.IO.Path.Combine(tempDir, fileName);
            return filePath;
        }


        public static void Init(bool isFirst, string solutionDir, System.Lazy<ILogEventSink> outputWindowSink)
        {

            FilePath = GetFilePath(solutionDir);
            VSFilePath = GetVSFilePath(solutionDir);

            fileLevelSwitch = new LoggingLevelSwitch(fileMinLevel);
            outputWindowLevelSwitch = new LoggingLevelSwitch(outputWindowMinLevel);

            var config = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.With(new DomainHashEnricher());

            const string outputTemplate = "{Timestamp:HH:mm:ss.fff} [U] [{Level:u3}] [{DomainHash}] {Message:lj}{NewLine}{Exception}";

            config = config.WriteTo.PersistentFile(FilePath
                , outputTemplate: outputTemplate
                , shared: true
                , persistentFileRollingInterval: PersistentFileRollingInterval.Day
                , preserveLogFilename: true
                , levelSwitch: fileLevelSwitch
                , rollOnEachProcessRun: isFirst
                );

            if (_outputWindowSink == null)
                _outputWindowSink = outputWindowSink.Value;
            if (_outputWindowSink != null)
                config = config.WriteTo.Sink(_outputWindowSink, outputWindowMinLevel, outputWindowLevelSwitch);

            if (_memorySink == null)
                _memorySink = new MemorySink(outputTemplate);
            config = config.WriteTo.Sink(_memorySink, fileMinLevel, null);

            Serilog.Log.Logger = config.CreateLogger();
            //switchableLogger.Set(config.CreateLogger(), disposePrev: true);

            OnOptionsChanged();

            IsInitialized = true;
        }

        public static void OnOptionsChanged()
        {
            // Since we don't use LogEventLevel.Fatal, we can use it for disabling sinks

            var isLoggingToFile = OptionsIsLoggingToFile;
            var targetFileLevel = isLoggingToFile ? fileMinLevel : Serilog.Events.LogEventLevel.Fatal;
            fileLevelSwitch.MinimumLevel = targetFileLevel;

            var isLoggingToOutputWindow = OptionsIsLoggingToOutputWindow;
            var targetOutputWindowLevel = isLoggingToOutputWindow ? outputWindowMinLevel : Serilog.Events.LogEventLevel.Fatal;
            outputWindowLevelSwitch.MinimumLevel = targetOutputWindowLevel;
        }

        //**-- UI for these two
        static bool OptionsIsLoggingToFile => true;
        static bool OptionsIsLoggingToOutputWindow => true;
    }
}
