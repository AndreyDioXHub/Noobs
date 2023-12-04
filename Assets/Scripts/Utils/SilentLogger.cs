using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilentLogger : ILogger {
    public ILogHandler logHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool logEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public LogType filterLogType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool IsLogTypeAllowed(LogType logType) {
        return false;
    }

    public void Log(LogType logType, object message) {
       
    }

    public void Log(LogType logType, object message, UnityEngine.Object context) {
       
    }

    public void Log(LogType logType, string tag, object message) {
       
    }

    public void Log(LogType logType, string tag, object message, UnityEngine.Object context) {
        
    }

    public void Log(object message) {
        
    }

    public void Log(string tag, object message) {
        
    }

    public void Log(string tag, object message, UnityEngine.Object context) {
        
    }

    public void LogError(string tag, object message) {
        
    }

    public void LogError(string tag, object message, UnityEngine.Object context) {
        
    }

    public void LogException(Exception exception) {
        
    }

    public void LogException(Exception exception, UnityEngine.Object context) {
        
    }

    public void LogFormat(LogType logType, string format, params object[] args) {
        
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args) {
        
    }

    public void LogWarning(string tag, object message) {
        
    }

    public void LogWarning(string tag, object message, UnityEngine.Object context) {
        
    }
}
