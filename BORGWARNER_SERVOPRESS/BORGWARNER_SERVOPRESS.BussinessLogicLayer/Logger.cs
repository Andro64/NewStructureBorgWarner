using System;
using System.Diagnostics;
using System.IO;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class Logger
    {
        private static readonly object lockObj = new object();
        private static Logger instance;
        private static string logFileName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        private static string logFilePath;
        
        private Logger()
        {
            Debug.Listeners.Clear();
            Debug.Listeners.Add(new TextWriterTraceListener(logFilePath)
            {
                TraceOutputOptions = TraceOptions.DateTime
            });
            Debug.AutoFlush = true;            
        }

        public static void SetLogFilePath(string path)
        {
            logFilePath = path + logFileName;            
            Debug.Listeners.Clear();
            Debug.Listeners.Add(new TextWriterTraceListener(logFilePath)
            {
                TraceOutputOptions = TraceOptions.DateTime
            });
            Debug.AutoFlush = true;
        }

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }

        public void Log(string message)
        {
            lock (lockObj)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        writer.WriteLine($"{DateTime.Now} - {message}");
                    }
                    //Debug.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{DateTime.Now} - "  + $"Error al escribir en el archivo de log: {ex.Message}");
                }
            }
        }
    }
}
