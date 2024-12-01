using RumoAutoQuery.Database;
using RumoAutoQuery.Files;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumoAutoQuery
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyLogEvent.Writer("Application started;");

            ExcelFileRepository fileRepository = new ExcelFileRepository();
            DatabaseRepository databaseRepository = new DatabaseRepository();

            List<ExcelFile> files = fileRepository.Index();

            if (files.Count == 0)
            {
                MyLogEvent.Writer("No files found;");
                MyLogEvent.Writer("Application finished;\n");
                return;
            }

            if (files.Count == 1)
            {
                MyLogEvent.Writer($"1 file was found;");
            }
            else
            {
                MyLogEvent.Writer($"{files.Count} files were found;");
            }

            foreach (var file in files)
            {
                MyLogEvent.Writer($"Processing file: {file.GetPath()};");
                List<string> values = file.ExtractValuesFromColumn("Chave");

                if (values.Count == 0)
                {
                    MyLogEvent.Writer("No keys found in the excel file;");
                    continue;
                }

                MyLogEvent.Writer($"Total of keys found in the excel file: {values.Count}");

                List<string> foundKeys = databaseRepository.ExistKeys(values);

                if (foundKeys.Count == 0)
                {
                    MyLogEvent.Writer("No keys found in the database;");
                    continue;
                }

                MyLogEvent.Writer($"Total of keys found in the database: {foundKeys.Count}");

                ExcelFile excelFile = fileRepository.Create();

                MyLogEvent.Writer($"Creating file: {excelFile.GetPath()};");

                excelFile.WriteValuesToColumn("Chave Documento", foundKeys);

                MyLogEvent.Writer($"Save found keys into xlsx file;");
                MyLogEvent.Writer($"End process;");
            }

            MyLogEvent.Writer("Application finished;\n");
        }
    }
}
