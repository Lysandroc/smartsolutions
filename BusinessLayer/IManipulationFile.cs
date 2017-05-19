using System;
using System.IO;
namespace BusinessLayer
{
	public interface IManipulationFile
	{
		String ConvertFileToCsv(FileStream file);
	}
}
