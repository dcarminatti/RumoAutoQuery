using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumoAutoQuery.Files
{
    internal class ExcelFile
    {
        private string _path;

        public ExcelFile(string path)
        {
            var extension = Path.GetExtension(path);
            if (!extension.Contains("xls"))
            {
                MyLogEvent.Writer($"Invalid file extension - File type provided: {extension}");
            }
            else
            {
                this._path = path;
            }
        }

        public string GetPath()
        {
            return this._path;
        }

        public List<string> ExtractValuesFromColumn(string columnName)
        {
            var extractedValues = new List<string>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(this._path)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int columnIndex = -1;

                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    if (worksheet.Cells[1, col].Text.Trim().Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        columnIndex = col;
                        break;
                    }
                }

                if (columnIndex == -1)
                {
                    MyLogEvent.Writer($"Column {columnName} not found in file {this._path}");
                    return extractedValues;
                }
                else
                {
                    MyLogEvent.Writer($"Column {columnName} found in file {this._path}");
                }


                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    string value = worksheet.Cells[row, columnIndex].Text.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        extractedValues.Add(value.ToString());
                    }
                }
            }

            return extractedValues;
        }

        public void WriteValuesToColumn(string columnName, List<string> values)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Consulta Base");

                worksheet.Cells[1, 1].Value = columnName;

                for (int i = 0; i < values.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = values[i];
                }

                var fileInfo = new FileInfo(this._path);
                package.SaveAs(fileInfo);
            }
        }
    }
}
