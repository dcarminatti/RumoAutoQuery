using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumoAutoQuery.Files
{
    internal class ExcelFileRepository
    {
        private readonly string _path;

        public ExcelFileRepository()
        {
            var path = Path.Combine(ConfigurationManager.AppSettings["FilesFolder"], "Files");
            if (!Directory.Exists(path))
            {
                MyLogEvent.Writer($"Creating directory: {path};");
                Directory.CreateDirectory(path);
            }
            this._path = path;
        }

        public List<ExcelFile> Index()
        {
            List<ExcelFile> files = new List<ExcelFile>();
            foreach (var file in Directory.GetFiles(this._path))
            {
                var excelFile = new ExcelFile(file);
                if (excelFile.GetPath() != null)
                {
                    files.Add(excelFile);
                }
            }
            return files;
        }

        public ExcelFile Create()
        {
            string baseName = "ConsultaBase";
            string datePart = DateTime.Now.ToString("yyyyMMdd");
            string fileName = $"{baseName}-{datePart}.xlsx";

            int counter = 2;

            while (this.Exists(fileName))
            {
                fileName = $"{fileName}-{datePart}-parte{counter:D2}.txt";
                counter++;
            }

            return new ExcelFile(Path.Combine(this._path, fileName));
        }

        public bool Exists(string fileName)
        {
            return File.Exists(Path.Combine(this._path, fileName));
        }
    }
}
