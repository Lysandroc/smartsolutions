using System;
using System.IO;
using System.Text;
using OfficeOpenXml;

namespace BusinessLayer
{
	public class ManipulatorFile : IManipulationFile
	{
		readonly char separator = ';';

		public ManipulatorFile()
		{
		}

		public String ConvertFileToCsv(FileStream fileStream)
		{
			try
			{
				using (ExcelPackage package = new ExcelPackage(fileStream))
				{
					var sb = new StringBuilder();
					ExcelWorksheet excelWorkSheet = package.Workbook.Worksheets[1];

					int rows = excelWorkSheet.Dimension.Rows;
					int columns = excelWorkSheet.Dimension.Columns - 1;

					using (var memoryStream = new MemoryStream())
					using (var writer = new StreamWriter(memoryStream))
					{
						for (int row = 1; row <= rows; row++)
						{
							for (int column = 1; column <= columns; column++)
							{
								var textCell = (excelWorkSheet.Cells[row, column].Value.ToString());
								sb.Append(textCell.Replace(";", "")); //TODO "replace" change for regex
								sb.Append(separator);
							}
							sb.Append("\n");

						}
						return sb.ToString();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine();
				Console.WriteLine(ex.StackTrace);
				throw ex;
			}

		}
	}
}
