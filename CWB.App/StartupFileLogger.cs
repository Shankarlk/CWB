using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App
{
    public class StartupFileLogger
    {
        private static readonly string LogFilePath = "startup_log.txt";

        public static void Log(string message)
        {
            try
            {
                File.AppendAllText(
                    LogFilePath,
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}{Environment.NewLine}"
                );
            }
            catch
            {
                // NEVER throw from logging
            }
        }
    }
}
