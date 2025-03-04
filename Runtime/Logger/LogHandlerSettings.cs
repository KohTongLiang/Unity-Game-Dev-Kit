using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Logger
{
    /// <summary>
    /// Acts as a buffer between receiving requests to display error messages to the player and running the pop-up UI to do so.
    /// Game specific way to handle logs.
    /// </summary>
    public class LogHandlerSettings : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Only logs of this level or higher will appear in the console.")]
        private LogMode m_editorLogVerbosity = LogMode.Critical;

        public static LogHandlerSettings Instance
        {
            get
            {
                if (s_LogHandlerSettings != null) return s_LogHandlerSettings;
                return s_LogHandlerSettings = FindObjectOfType<LogHandlerSettings>();
            }
        }
        
        public delegate void LogCallbackDelegate(LogType logType, Object context, string message, params object[] args);
        private void LogCallBack(LogType logType, Object context, string message, params object[] args)
        {
            if (logType is LogType.Error or LogType.Assert)
            {
                GenerateLogText(logType, args);
                return;
            }

            if (m_editorLogVerbosity == LogMode.Critical)
            {
                GenerateLogText(logType, args);
                return;
            }
                
            if (logType == LogType.Warning)
            {
                GenerateLogText(logType, args);
                return;
            }

            if (m_editorLogVerbosity != LogMode.Verbose) return;                
            GenerateLogText(logType, args);
        }

        private void GenerateLogText(LogType logType, object[] logText)
        {
            if (logText is not { Length: > 0 }) return;
            foreach (string s in logText)
            {
                // logEvent?.FireEvent($"{DateTime.Now} {logType}: {s}. \n");
            }
        }
        
        static LogHandlerSettings s_LogHandlerSettings;
        private void Awake()
        {
            LogHandler.Get().mode = m_editorLogVerbosity;
            LogHandler.Get().SetLogCallback(LogCallBack);
            Debug.Log($"Starting project with Log Level : {m_editorLogVerbosity.ToString()}");
        }


        /// <summary>
        /// For convenience while in the Editor, update the log verbosity when its value is changed in the Inspector.
        /// </summary>
        public void OnValidate()
        {
            LogHandler.Get().mode = m_editorLogVerbosity;
        }
    }
}
