using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using System.Text;

namespace CastFiles.Controllers
{
    public class HomeController : Controller
    {
		private IHostingEnvironment _environment;
		private IManipulationFile _manipulationFile;

		public HomeController(IHostingEnvironment environment, IManipulationFile manipulationFile)
		{
			_environment = environment;
			_manipulationFile = manipulationFile;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public FileResult Index(IFormFile files)
        {
			try
			{
				Console.WriteLine(files.FileName);

				var uploads = Path.Combine(_environment.WebRootPath, "uploads");

				if (files.Length > 0)
				{
					//Escreve o arquivo em um local temporário
					byte[] arquivo = new byte[files.Length];
					files.OpenReadStream().Read(arquivo, 0, (int)files.Length);

					var filePath = Path.GetTempFileName();
					Console.WriteLine(filePath);

					var logFile = System.IO.File.Create(filePath);
					var logWriter = new BinaryWriter(logFile);
					logWriter.Write(arquivo);
					logWriter.Dispose();

					//Lê arquivo temporário e converte para CSV
					var fileToSendBack = _manipulationFile.ConvertFileToCsv(new FileStream(filePath, FileMode.Open));

					//Apaga arquivo temporário
					System.IO.File.Delete(filePath);

					//Retorna arquivo CSV
					return File(Encoding.Unicode.GetBytes(fileToSendBack), "text/csv", "Teste.csv");
				}
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine();
				Console.WriteLine(e.StackTrace);
				return File(new byte[0], "text/csv", "tste.csv");
			}
        }
    }
}
