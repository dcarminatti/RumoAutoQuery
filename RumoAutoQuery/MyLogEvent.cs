using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumoAutoQuery
{
    internal class MyLogEvent
    {
        public static void Writer(string message)
        {
            var newFolder = Path.Combine(ConfigurationManager.AppSettings["FilesFolder"], "Logs");
            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }

            var logFile = Path.Combine(newFolder, "log.txt");
            using (var writer = new StreamWriter(logFile, true))
            {
                writer.WriteLine($"{DateTime.Now} - {message}");
            }
        }
    }
}
