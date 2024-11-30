using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDNS
{
    internal enum LogLevel
    {
        Trace = 0,
        Debug=1,
        Info=2,
        Warn=3,
        Error=4,
        Fatal =5
    }
    internal struct Log
    {
        public DateTime time;
        public string message;
        public LogLevel level;
    }
    internal class Logger
    {
        public static LogLevel ParseLevel(string level)
        {
            switch (level)
            {
                case "Trace": return LogLevel.Trace;
                case "Debug": return LogLevel.Debug;
                case "Info": return LogLevel.Info;
                case "Warn": return LogLevel.Warn;
                case "Error": return LogLevel.Error;
                case "Fatal": return LogLevel.Fatal;
            }
            return LogLevel.Trace;
        }
        public static LogLevel Level { get;set; }
        public static int MaxCount { get; } = 2000;
        public static ConcurrentStack<Log> Logs { get;private set; }=new ConcurrentStack<Log>();

        public static Action<Log>? OnAppended { get; set; }
        private static void AppendLog(string message, LogLevel level)
        {
            Log log = new Log { message = message, time = DateTime.Now, level = level };
            Logs.Push(log);
            if(OnAppended != null)
            {
                OnAppended(log);
            }
            if (Logs.Count > MaxCount)
            {
                Logs.TryPop(out var x);
            }
        }
        public static void Trace(string message) { 
           AppendLog(message, LogLevel.Trace);
        }
        public static void Debug(string message) {
            AppendLog(message, LogLevel.Debug);
        }
        public static void Info(string message) {
            AppendLog(message, LogLevel.Info);
        }
        public static void Warn(string message) {
            AppendLog(message, LogLevel.Warn);
        }
        public static void Error(string message) {
            AppendLog(message, LogLevel.Error);
        }
        public static void Fatal(string message) {
            AppendLog(message, LogLevel.Fatal);
        }

    }
}
