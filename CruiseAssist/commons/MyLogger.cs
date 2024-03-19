using System;
using System.IO;

namespace tanu.CruiseAssist
{
    public static class MyLogger
    {
        public static string LogString = "";
        private static int _logStringMaxLength = 2000;
        public static void LogToGame(string message)
        {
            string newLog = $"\r\n{message}";
            if (LogString.Length + newLog.Length > _logStringMaxLength)
            {
                int excessLength = (LogString.Length + newLog.Length) - _logStringMaxLength;
                LogString = LogString.Substring(excessLength);
            }
            LogString += newLog;
        }
        public static void LogMessageToFile(string message, string filePath = "MyDspLog.txt")
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"[{DateTime.Now}] Message :");
                writer.WriteLine($"{message}");
            }
        }
        public static void LogExceptionToFile(Exception ex, string filePath ="MyDspLog.txt")
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"[{DateTime.Now}] An exception occurred:");
                writer.WriteLine($"Message: {ex.Message}");

                // Extract file name and line number from stack trace
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    string[] lines = ex.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length > 0)
                    {
                        string line = lines[0]; // Get the first line of the stack trace
                        int index = line.LastIndexOf(" in "); // Find the " in " text
                        if (index != -1)
                        {
                            // Extract the file name and line number
                            string fileNameAndLine = line.Substring(index + 4);
                            writer.WriteLine($"File and Line: {fileNameAndLine}");
                        }
                    }
                }

                writer.WriteLine($"StackTrace: {ex.StackTrace}");

                // If the exception has a target site, log the method name
                if (ex.TargetSite != null)
                {
                    writer.WriteLine($"TargetSite: {ex.TargetSite}");
                }

                // Log additional information if available
                if (ex.Data.Count > 0)
                {
                    writer.WriteLine("Additional Information:");
                    foreach (var key in ex.Data.Keys)
                    {
                        writer.WriteLine($"- {key}: {ex.Data[key]}");
                    }
                }

                writer.WriteLine();
            }
        }
        
    }
}